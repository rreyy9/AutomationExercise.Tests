namespace AutomationExercise.UI.Tests.Tests
{
    [TestClass]
    [TestCategory("Smoke")]
    public class HomepageSmokeTests
    {
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;
        private IPage _page = null!;

        [TestInitialize]
        public async Task TestInitialize()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = false });
            _page = await _browser.NewPageAsync();
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [TestMethod]
        public async Task Homepage_Title_ContainsAutomationExercise()
        {
            await _page.GotoAsync("https://automationexercise.com");
            await Assertions.Expect(_page).ToHaveTitleAsync(
                new Regex("Automation Exercise"));
        }
    }
}