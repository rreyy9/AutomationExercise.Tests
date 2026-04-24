namespace AutomationExercise.UI.Tests.Tests.Checkout
{
    [TestClass]
    public class CheckoutTests : BaseTest
    {
        private ProductsPage _productsPage = null!;

        [TestInitialize]
        public async Task Initialize()
        {
            _productsPage = new ProductsPage(Page);
        }

        // ── Guest checkout flow ───────────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        public async Task Checkout_AsGuest_ProceedShowsRegisterLoginPrompt()
        {
            Log.Step("Adding a product to cart");
            await _productsPage.GoToAsync();
            await _productsPage.AddProductToCartAsync(0);

            Log.Step("Navigating to cart");
            var cartPage = await Page.GotoAsync("/view_cart")
                .ContinueWith(_ => new CartPage(Page));

            Log.Step("Asserting cart has an item");
            Assert.IsTrue(await cartPage.IsCartTableVisibleAsync(),
                "Expected cart table to be visible");

            Log.Step("Proceeding to checkout as guest — expecting register/login prompt");
            // Guest users get a modal with a Register/Login link before proceeding.
            // Clicking that link navigates to /login — we assert we arrive there.
            var loginPage = await cartPage.GoToLoginFromCheckoutPromptAsync();

            Log.Step("Asserting login page is shown");
            Assert.IsTrue(
                await loginPage.IsLoginFormVisibleAsync(),
                "Expected login page to appear after guest clicks 'Register / Login' from checkout prompt");
        }

        // ── Commented out — require account creation against a live shared site ──
        // These flows are fully covered by the Phase 5 API tests.
        // Uncomment if a dedicated sandbox or test credentials are available.
        //
        // [TestMethod]
        // [TestCategory("Smoke")]
        // [TestCategory("Critical")]
        // public async Task Checkout_AsRegisteredUser_OrderPlacedSuccessfully()
        // {
        //     // Requires account creation — covered by API tests instead.
        //     // If reinstating:
        //     // 1. Register via LoginPage.StartSignupAsync + SignupPage.FillAccountDetailsAsync
        //     // 2. Add product, proceed through checkout, enter card details
        //     // 3. Assert PaymentPage.IsSuccessMessageVisibleAsync()
        //     // 4. Delete account in [TestCleanup] via Nav.DeleteAccountAsync()
        // }
        //
        // [TestMethod]
        // [TestCategory("Regression")]
        // public async Task Checkout_AddressDetails_MatchRegisteredUserAddress()
        // {
        //     // Requires account with known address — covered by API tests instead.
        // }
        //
        // [TestMethod]
        // [TestCategory("Regression")]
        // public async Task Checkout_DownloadInvoice_AfterOrderPlacement()
        // {
        //     // Requires completed order as registered user — covered by API tests instead.
        //     // If reinstating: use context.WaitForDownloadAsync() to capture the file,
        //     // then assert the download path is not null/empty.
        // }
    }
}