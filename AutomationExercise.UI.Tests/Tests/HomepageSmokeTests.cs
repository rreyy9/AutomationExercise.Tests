namespace AutomationExercise.UI.Tests.Tests
{
    [TestClass]
    [TestCategory("Smoke")]
    public class HomepageSmokeTests : BaseTest
    {
        [TestMethod]
        public async Task Homepage_Title_ContainsAutomationExercise()
        {
            Log.Step("Navigating to homepage");
            await Page.GotoAsync("/");

            Log.Step("Asserting page title");
            await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Automation Exercise"));
        }
    }
}