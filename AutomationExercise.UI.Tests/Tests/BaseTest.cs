using AutomationExercise.UI.Tests.Utils;

namespace AutomationExercise.UI.Tests
{
    [TestClass]
    public abstract class BaseTest
    {
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;
        private IBrowserContext _context = null!;
        private string _videoDir = string.Empty;

        protected IPage Page { get; private set; } = null!;
        protected Logger Log { get; private set; } = null!;

        public TestContext TestContext { get; set; } = null!;

        [TestInitialize]
        public async Task BaseTestInitialize()
        {
            Log = new Logger(TestContext);
            Log.Info($"Starting test: {TestContext.TestName}");

            var config = TestConfig.Load();

            _playwright = await Playwright.CreateAsync();

            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = config.Headless,
                // --start-maximized tells the OS to maximise the window on launch.
                // This has no effect in headless mode — headless uses the viewport size instead.
                Args = new[] { "--start-maximized" }
            });

            _videoDir = Path.Combine(Path.GetTempPath(), "pw-video", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_videoDir);

            _context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                BaseURL = config.BaseUrl,

                // ViewportSize must be null when using --start-maximized.
                // A fixed viewport overrides the maximised window and you end up
                // with a maximised window showing a smaller fixed-size page.
                ViewportSize = ViewportSize.NoViewport,

                RecordVideoDir = _videoDir,

                // Video size is separate from viewport — keep a sensible fixed size
                // so recorded video is always consistent regardless of screen resolution.
                RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
            });

            await _context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

            Page = await _context.NewPageAsync();
            Page.SetDefaultTimeout(config.Timeout);

            Log.Info($"Browser context ready. BaseUrl: {config.BaseUrl} | Headless: {config.Headless}");
        }

        [TestCleanup]
        public async Task BaseTestCleanup()
        {
            var passed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;

            if (!passed)
            {
                Log.Warning($"Test failed: {TestContext.TestName} — capturing failure artifacts");
                await CaptureFailureArtifacts();
            }

            var tracePath = Path.Combine(Path.GetTempPath(), "pw-traces", $"{TestContext.TestName}.zip");
            Directory.CreateDirectory(Path.GetDirectoryName(tracePath)!);

            await _context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = passed ? null : tracePath
            });

            await _context.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();

            if (passed && Directory.Exists(_videoDir))
            {
                try { Directory.Delete(_videoDir, recursive: true); }
                catch { /* best-effort — don't fail a passing test over cleanup */ }
            }
            else if (!passed && Directory.Exists(_videoDir))
            {
                var videos = Directory.GetFiles(_videoDir, "*.webm");
                foreach (var video in videos)
                {
                    Log.Info($"Video artifact: {video}");
                    TestContext.AddResultFile(video);
                }
            }

            if (!passed && File.Exists(tracePath))
            {
                Log.Info($"Trace artifact: {tracePath}");
                Log.Info("Open trace at https://trace.playwright.dev — upload the .zip file");
                TestContext.AddResultFile(tracePath);
            }

            Log.Info($"Test complete: {TestContext.TestName} | Outcome: {TestContext.CurrentTestOutcome}");
        }

        private async Task CaptureFailureArtifacts()
        {
            try
            {
                var screenshotDir = Path.Combine(Path.GetTempPath(), "pw-screenshots");
                Directory.CreateDirectory(screenshotDir);
                var screenshotPath = Path.Combine(screenshotDir, $"{TestContext.TestName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png");

                await Page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = screenshotPath,
                    FullPage = true
                });

                Log.Info($"Screenshot saved: {screenshotPath}");
                TestContext.AddResultFile(screenshotPath);
            }
            catch (Exception ex)
            {
                Log.Error($"Screenshot capture failed: {ex.Message}");
            }
        }
    }
}