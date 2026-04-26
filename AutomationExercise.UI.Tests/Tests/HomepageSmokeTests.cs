namespace AutomationExercise.UI.Tests.Tests
{
    [TestClass]
    [TestCategory("Smoke")]
    public class HomepageSmokeTests : BaseTest
    {
        // ── Homepage smoke test (existing) ────────────────────────────────────────

        [TestMethod]
        [Description("Smoke: Homepage title contains 'Automation Exercise'")]
        public async Task Homepage_Title_ContainsAutomationExercise()
        {
            Log.Step("Navigating to homepage");
            await Page.GotoAsync("/");

            Log.Step("Asserting page title");
            await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Automation Exercise"));
        }

        // ── TC7: Verify Test Cases Page ───────────────────────────────────────────

        [TestMethod]
        [Description("TC7: Verify Test Cases Page")]
        public async Task TC7_VerifyTestCasesPage()
        {
            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Click on 'Test Cases' button
            Log.Step("Step 4 — Clicking 'Test Cases' button");
            await Page.Locator("a[href='/test_cases']").First.ClickAsync();

            // 5. Verify user is navigated to test cases page successfully
            Log.Step("Step 5 — Verifying user is on test cases page");
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex("/test_cases"));
            var testCasesHeading = Page.Locator("h2.title b:has-text('Test Cases')");
            await Assertions.Expect(testCasesHeading).ToBeVisibleAsync();
            Log.Info("Successfully navigated to Test Cases page");
        }

        // ── TC25: Verify Scroll Up using 'Arrow' button and Scroll Down ──────────

        [TestMethod]
        [TestCategory("Regression")]
        [Description("TC25: Verify Scroll Up using 'Arrow' button and Scroll Down functionality")]
        public async Task TC25_VerifyScrollUp_UsingArrowButton_AndScrollDown()
        {
            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Scroll down page to bottom
            Log.Step("Step 4 — Scrolling down to bottom of page");
            await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");

            // 5. Verify 'SUBSCRIPTION' is visible
            Log.Step("Step 5 — Verifying 'SUBSCRIPTION' is visible");
            var subscriptionHeading = Page.Locator("h2:has-text('Subscription')");
            await Assertions.Expect(subscriptionHeading).ToBeVisibleAsync();

            // 6. Click on arrow at bottom right side to move upward
            Log.Step("Step 6 — Clicking scroll-up arrow button");
            var scrollUpArrow = Page.Locator("#scrollUp");
            await scrollUpArrow.ClickAsync();

            // 7. Verify that page is scrolled up and 'Full-Fledged practice website for Automation Engineers' text is visible on screen
            Log.Step("Step 7 — Verifying page scrolled up and hero text is visible");
            var heroText = Page.Locator("h2:has-text('Full-Fledged practice website for Automation Engineers')");
            await Assertions.Expect(heroText).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
            {
                Timeout = 5000
            });
            Log.Info("Successfully verified scroll up using arrow button");
        }

        // ── TC26: Verify Scroll Up without 'Arrow' button and Scroll Down ────────

        [TestMethod]
        [TestCategory("Regression")]
        [Description("TC26: Verify Scroll Up without 'Arrow' button and Scroll Down functionality")]
        public async Task TC26_VerifyScrollUp_WithoutArrowButton_AndScrollDown()
        {
            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Scroll down page to bottom
            Log.Step("Step 4 — Scrolling down to bottom of page");
            await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");

            // 5. Verify 'SUBSCRIPTION' is visible
            Log.Step("Step 5 — Verifying 'SUBSCRIPTION' is visible");
            var subscriptionHeading = Page.Locator("h2:has-text('Subscription')");
            await Assertions.Expect(subscriptionHeading).ToBeVisibleAsync();

            // 6. Scroll up page to top
            Log.Step("Step 6 — Scrolling up to top of page (without arrow button)");
            await Page.EvaluateAsync("window.scrollTo(0, 0)");

            // 7. Verify that page is scrolled up and 'Full-Fledged practice website for Automation Engineers' text is visible on screen
            Log.Step("Step 7 — Verifying page scrolled up and hero text is visible");
            var heroText = Page.Locator("h2:has-text('Full-Fledged practice website for Automation Engineers')");
            await Assertions.Expect(heroText).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
            {
                Timeout = 5000
            });
            Log.Info("Successfully verified scroll up without arrow button");
        }
    }
}