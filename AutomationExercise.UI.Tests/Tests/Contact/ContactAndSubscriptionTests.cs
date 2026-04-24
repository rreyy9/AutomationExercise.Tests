namespace AutomationExercise.UI.Tests.Tests.Contact
{
    [TestClass]
    [TestCategory("Regression")]
    public class ContactAndSubscriptionTests : BaseTest
    {
        // ── Contact form ──────────────────────────────────────────────────────────

        [TestMethod]
        public async Task ContactForm_Submit_ShowsSuccessMessage()
        {
            Log.Step("Navigating to Contact Us page");
            var contactPage = new ContactUsPage(Page);
            await contactPage.GoToAsync();

            Log.Step("Asserting 'GET IN TOUCH' heading is visible");
            Assert.IsTrue(
                await contactPage.IsGetInTouchHeadingVisibleAsync(),
                "Expected 'GET IN TOUCH' heading to be visible");

            Log.Step("Filling contact form");
            await contactPage.FillContactFormAsync(
                name: "Test User",
                email: "testuser@example.com",
                subject: "Portfolio automation test enquiry",
                message: "This is an automated test message submitted by a Playwright test suite.");

            Log.Step("Submitting form (auto-accepting browser confirm dialog)");
            await contactPage.SubmitAsync();

            Log.Step("Asserting success message is visible");
            Assert.IsTrue(
                await contactPage.IsSuccessMessageVisibleAsync(),
                "Expected success message to appear after contact form submission");

            var message = await contactPage.GetSuccessMessageAsync();
            Log.Info($"Success message: {message}");
        }

        [TestMethod]
        public async Task ContactForm_Submit_HomeButtonNavigatesHome()
        {
            Log.Step("Navigating to Contact Us page and submitting form");
            var contactPage = new ContactUsPage(Page);
            await contactPage.GoToAsync();

            await contactPage.FillContactFormAsync(
                name: "Test User",
                email: "testuser@example.com",
                subject: "Navigation test",
                message: "Testing the Home button after submission.");

            await contactPage.SubmitAsync();

            Log.Step("Clicking Home button after submission");
            var homePage = await contactPage.ClickHomeAsync();

            Log.Step("Asserting hero slider is visible on homepage");
            Assert.IsTrue(
                await homePage.IsHeroSliderVisibleAsync(),
                "Expected to land on homepage with hero slider visible after clicking Home");
        }

        // ── Newsletter subscription from homepage ─────────────────────────────────

        [TestMethod]
        [Retry(1)] // Retry once — the subscription confirmation widget can be slow on first response from the live site
        public async Task Newsletter_SubscribeFromHomepage_ShowsSuccessMessage()
        {
            Log.Step("Navigating to homepage");
            var homePage = new HomePage(Page);
            await homePage.GoToAsync();

            Log.Step("Subscribing with a test email");
            await homePage.SubscribeAsync("playwright.test@example.com");

            Log.Step("Asserting subscription success message is visible");
            Assert.IsTrue(
                await homePage.IsSubscriptionSuccessVisibleAsync(),
                "Expected subscription success message to appear after subscribing from homepage");

            var message = await homePage.GetSubscriptionSuccessMessageAsync();
            Log.Info($"Subscription message: {message}");
        }

        // ── Newsletter subscription from cart page ────────────────────────────────

        [TestMethod]
        [Retry(1)] // Retry once — same footer subscription widget as homepage; same external latency risk
        public async Task Newsletter_SubscribeFromCartPage_ShowsSuccessMessage()
        {
            Log.Step("Navigating to cart page");
            var cartPage = new CartPage(Page);
            await cartPage.GoToAsync();

            // The cart page footer has the same subscription widget as the homepage.
            // CartPage doesn't expose a SubscribeAsync method — scroll to footer and
            // interact with the shared subscription locators directly via Page.
            Log.Step("Scrolling to subscription section in footer");
            var subscribeEmail = Page.Locator("#susbscibeemail"); // ⚠️ typo is the actual site ID
            var subscribeButton = Page.Locator("#subscribe");
            var successMessage = Page.Locator("div#success-subscribe");

            await subscribeEmail.ScrollIntoViewIfNeededAsync();
            await Page.Keyboard.PressAsync("Escape");  // add this line
            await subscribeEmail.FillAsync("playwright.cart.test@example.com", new LocatorFillOptions { Force = true });
            await subscribeButton.ClickAsync(new LocatorClickOptions { Force = true });

            Log.Step("Asserting subscription success message is visible");
            await Microsoft.Playwright.Assertions.Expect(successMessage).ToBeVisibleAsync();
            Log.Info("Subscription from cart page successful");
        }
    }
}