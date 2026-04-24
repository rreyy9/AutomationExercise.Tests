namespace AutomationExercise.UI.Tests.Utils
{
    /// <summary>
    /// Thin wrapper around TestContext.WriteLine that prepends a UTC timestamp.
    /// Use this instead of Console.WriteLine — output surfaces correctly in MSTest results.
    /// </summary>
    public class Logger
    {
        private readonly TestContext _testContext;

        public Logger(TestContext testContext)
        {
            _testContext = testContext;
        }

        public void Info(string message) =>
            _testContext.WriteLine($"[{Timestamp}] [INFO]  {message}");

        public void Warning(string message) =>
            _testContext.WriteLine($"[{Timestamp}] [WARN]  {message}");

        public void Error(string message) =>
            _testContext.WriteLine($"[{Timestamp}] [ERROR] {message}");

        public void Step(string message) =>
            _testContext.WriteLine($"[{Timestamp}] [STEP]  {message}");

        private static string Timestamp =>
            DateTime.UtcNow.ToString("HH:mm:ss.fff");
    }
}