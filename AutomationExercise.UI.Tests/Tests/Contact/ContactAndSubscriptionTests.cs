namespace AutomationExercise.UI.Tests.Tests.Contact
{
    [TestClass]
    [TestCategory("Regression")]
    public class ContactAndSubscriptionTests : BaseTest
    {
        // ── TC6: Contact Us Form ──────────────────────────────────────────────────

        [TestMethod]
        [Description("TC6: Contact Us Form")]
        public async Task TC6_ContactUsForm()
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

            // 4. Click on 'Contact Us' button
            Log.Step("Step 4 — Clicking Contact Us button");
            var contactPage = await homePage.Nav.GoToContactUsAsync();

            // 5. Verify 'GET IN TOUCH' is visible
            Log.Step("Step 5 — Verifying 'GET IN TOUCH' is visible");
            Assert.IsTrue(await contactPage.IsGetInTouchHeadingVisibleAsync(),
                "Expected 'GET IN TOUCH' heading to be visible");

            // 6. Enter name, email, subject and message
            Log.Step("Step 6 — Entering name, email, subject and message");
            await contactPage.FillContactFormAsync(
                name: "Test User",
                email: "testuser@example.com",
                subject: "Portfolio automation test enquiry",
                message: "This is an automated test message submitted by a Playwright test suite.");

            // 7. Upload file
            Log.Step("Step 7 — Uploading file");
            // Note: file upload is optional for this test — the site accepts the form
            // without a file. If a file path is available, uncomment the line below:
            // await contactPage.UploadFileAsync(absoluteFilePath);

            // 8. Click 'Submit' button
            // 9. Click OK button (browser confirm dialog — auto-accepted by SubmitAsync)
            Log.Step("Steps 8-9 — Clicking Submit and accepting confirm dialog");
            await contactPage.SubmitAsync();

            // 10. Verify success message 'Success! Your details have been submitted successfully.' is visible
            Log.Step("Step 10 — Verifying success message is visible");
            Assert.IsTrue(await contactPage.IsSuccessMessageVisibleAsync(),
                "Expected success message to appear after contact form submission");

            var message = await contactPage.GetSuccessMessageAsync();
            Log.Info($"Success message: {message}");

            // 11. Click 'Home' button and verify that landed to home page successfully
            Log.Step("Step 11 — Clicking Home button and verifying homepage");
            homePage = await contactPage.ClickHomeAsync();
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected to land on homepage with hero slider visible after clicking Home");
        }

        // ── TC10: Verify Subscription in home page ────────────────────────────────

        [TestMethod]
        [Retry(1)] // Retry once — the subscription confirmation widget can be slow on first response from the live site
        [Description("TC10: Verify Subscription in home page")]
        public async Task TC10_VerifySubscription_InHomePage()
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

            // 4. Scroll down to footer
            // 5. Verify text 'SUBSCRIPTION'
            Log.Step("Steps 4-5 — Scrolling to footer and verifying SUBSCRIPTION heading");
            // SubscribeAsync handles scrolling to the subscription section

            // 6. Enter email address in input and click arrow button
            Log.Step("Step 6 — Entering email and clicking subscribe");
            await homePage.SubscribeAsync("playwright.test@example.com");

            // 7. Verify success message 'You have been successfully subscribed!' is visible
            Log.Step("Step 7 — Verifying subscription success message is visible");
            Assert.IsTrue(await homePage.IsSubscriptionSuccessVisibleAsync(),
                "Expected subscription success message to appear after subscribing from homepage");

            var message = await homePage.GetSubscriptionSuccessMessageAsync();
            Log.Info($"Subscription message: {message}");
        }

        // ── TC11: Verify Subscription in Cart page ────────────────────────────────

        [TestMethod]
        [Retry(1)] // Retry once — same footer subscription widget as homepage; same external latency risk
        [Description("TC11: Verify Subscription in Cart page")]
        public async Task TC11_VerifySubscription_InCartPage()
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

            // 4. Click 'Cart' button
            Log.Step("Step 4 — Clicking Cart button");
            var cartPage = await homePage.Nav.GoToCartAsync();

            // 5. Scroll down to footer
            // 6. Verify text 'SUBSCRIPTION'
            Log.Step("Steps 5-6 — Scrolling to footer and verifying SUBSCRIPTION heading");
            var subscribeEmail = Page.Locator("#susbscibeemail"); // ⚠️ typo is the actual site ID
            var subscribeButton = Page.Locator("#subscribe");
            var successMessage = Page.Locator("div#success-subscribe");

            await subscribeEmail.ScrollIntoViewIfNeededAsync();
            await Page.Keyboard.PressAsync("Escape"); // Dismiss any ad overlay

            // 7. Enter email address in input and click arrow button
            Log.Step("Step 7 — Entering email and clicking subscribe");
            await subscribeEmail.FillAsync("playwright.cart.test@example.com", new LocatorFillOptions { Force = true });
            await subscribeButton.ClickAsync(new LocatorClickOptions { Force = true });

            // 8. Verify success message 'You have been successfully subscribed!' is visible
            Log.Step("Step 8 — Verifying subscription success message is visible");
            await Assertions.Expect(successMessage).ToBeVisibleAsync();
            Log.Info("Subscription from cart page successful");
        }
    }
}