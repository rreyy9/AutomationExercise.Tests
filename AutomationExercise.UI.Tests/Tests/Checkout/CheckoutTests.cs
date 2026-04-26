namespace AutomationExercise.UI.Tests.Tests.Checkout
{
    [TestClass]
    public class CheckoutTests : BaseTest
    {
        // ── TC14: Place Order: Register while Checkout ────────────────────────────
        // ⚠️ Commented out — requires account creation against a live shared site.
        // Covered by Phase 5 API tests. Uncomment if a dedicated sandbox is available.
        //
        // [TestMethod]
        // [TestCategory("Smoke")]
        // [TestCategory("Critical")]
        // [Description("TC14: Place Order: Register while Checkout")]
        // public async Task TC14_PlaceOrder_RegisterWhileCheckout()
        // {
        //     // 1. Launch browser — handled by BaseTest
        //     // 2. Navigate to url 'http://automationexercise.com'
        //     Log.Step("Step 2 — Navigating to homepage");
        //     await Page.GotoAsync("/");
        //
        //     // 3. Verify that home page is visible successfully
        //     Log.Step("Step 3 — Verifying homepage is visible");
        //     var homePage = new HomePage(Page);
        //     Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync());
        //
        //     // 4. Add products to cart
        //     Log.Step("Step 4 — Adding products to cart");
        //     var productsPage = new ProductsPage(Page);
        //     await productsPage.GoToAsync();
        //     await productsPage.AddProductToCartAsync(0);
        //
        //     // 5. Click 'Cart' button
        //     Log.Step("Step 5 — Clicking Cart button");
        //     var cartPage = await homePage.Nav.GoToCartAsync();
        //
        //     // 6. Verify that cart page is displayed
        //     Log.Step("Step 6 — Verifying cart page is displayed");
        //     Assert.IsTrue(await cartPage.IsCartTableVisibleAsync());
        //
        //     // 7. Click Proceed To Checkout
        //     Log.Step("Step 7 — Clicking Proceed To Checkout");
        //
        //     // 8. Click 'Register / Login' button
        //     Log.Step("Step 8 — Clicking Register / Login from checkout prompt");
        //     var loginPage = await cartPage.GoToLoginFromCheckoutPromptAsync();
        //
        //     // 9. Fill all details in Signup and create account
        //     Log.Step("Step 9 — Filling signup details and creating account");
        //     // var signupPage = await loginPage.StartSignupAsync(name, email);
        //     // await signupPage.FillAccountDetailsAsync(details);
        //     // await signupPage.CreateAccountAsync();
        //
        //     // 10. Verify 'ACCOUNT CREATED!' and click 'Continue' button
        //     Log.Step("Step 10 — Verifying 'ACCOUNT CREATED!' and clicking Continue");
        //     // Assert.IsTrue(await signupPage.IsAccountCreatedHeadingVisibleAsync());
        //     // await signupPage.ClickContinueAsync();
        //
        //     // 11. Verify 'Logged in as username' at top
        //     Log.Step("Step 11 — Verifying 'Logged in as username'");
        //     // Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync());
        //
        //     // 12. Click 'Cart' button
        //     Log.Step("Step 12 — Clicking Cart button");
        //     // cartPage = await homePage.Nav.GoToCartAsync();
        //
        //     // 13. Click 'Proceed To Checkout' button
        //     Log.Step("Step 13 — Clicking Proceed To Checkout");
        //     // var checkoutPage = await cartPage.ProceedToCheckoutAsync();
        //
        //     // 14. Verify Address Details and Review Your Order
        //     Log.Step("Step 14 — Verifying address details and order review");
        //     // Assert.IsTrue(await checkoutPage.IsOrderTableVisibleAsync());
        //
        //     // 15. Enter description in comment text area and click 'Place Order'
        //     Log.Step("Step 15 — Entering comment and placing order");
        //     // await checkoutPage.AddCommentAsync("Automated test order");
        //     // var paymentPage = await checkoutPage.PlaceOrderAsync();
        //
        //     // 16. Enter payment details: Name on Card, Card Number, CVC, Expiration date
        //     Log.Step("Step 16 — Entering payment details");
        //     // await paymentPage.EnterCardDetailsAsync(cardDetails);
        //
        //     // 17. Click 'Pay and Confirm Order' button
        //     Log.Step("Step 17 — Clicking Pay and Confirm Order");
        //     // await paymentPage.ConfirmPaymentAsync();
        //
        //     // 18. Verify success message 'Your order has been placed successfully!'
        //     Log.Step("Step 18 — Verifying order success message");
        //     // Assert.IsTrue(await paymentPage.IsSuccessMessageVisibleAsync());
        //
        //     // 19. Click 'Delete Account' button
        //     Log.Step("Step 19 — Clicking Delete Account");
        //     // await homePage.Nav.DeleteAccountAsync();
        //
        //     // 20. Verify 'ACCOUNT DELETED!' and click 'Continue' button
        //     Log.Step("Step 20 — Verifying 'ACCOUNT DELETED!'");
        // }

        // ── TC15: Place Order: Register before Checkout ───────────────────────────
        // ⚠️ Commented out — requires account creation against a live shared site.
        //
        // [TestMethod]
        // [TestCategory("Smoke")]
        // [TestCategory("Critical")]
        // [Description("TC15: Place Order: Register before Checkout")]
        // public async Task TC15_PlaceOrder_RegisterBeforeCheckout()
        // {
        //     // 1. Launch browser — handled by BaseTest
        //     // 2. Navigate to url 'http://automationexercise.com'
        //     Log.Step("Step 2 — Navigating to homepage");
        //     await Page.GotoAsync("/");
        //
        //     // 3. Verify that home page is visible successfully
        //     Log.Step("Step 3 — Verifying homepage is visible");
        //     var homePage = new HomePage(Page);
        //     Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync());
        //
        //     // 4. Click 'Signup / Login' button
        //     Log.Step("Step 4 — Clicking Signup / Login button");
        //     var loginPage = await homePage.Nav.GoToLoginAsync();
        //
        //     // 5. Fill all details in Signup and create account
        //     Log.Step("Step 5 — Filling signup details and creating account");
        //     // var signupPage = await loginPage.StartSignupAsync(name, email);
        //     // await signupPage.FillAccountDetailsAsync(details);
        //     // await signupPage.CreateAccountAsync();
        //
        //     // 6. Verify 'ACCOUNT CREATED!' and click 'Continue' button
        //     Log.Step("Step 6 — Verifying 'ACCOUNT CREATED!' and clicking Continue");
        //
        //     // 7. Verify 'Logged in as username' at top
        //     Log.Step("Step 7 — Verifying 'Logged in as username'");
        //
        //     // 8. Add products to cart
        //     Log.Step("Step 8 — Adding products to cart");
        //
        //     // 9. Click 'Cart' button
        //     Log.Step("Step 9 — Clicking Cart button");
        //
        //     // 10. Verify that cart page is displayed
        //     Log.Step("Step 10 — Verifying cart page is displayed");
        //
        //     // 11. Click Proceed To Checkout
        //     Log.Step("Step 11 — Clicking Proceed To Checkout");
        //
        //     // 12. Verify Address Details and Review Your Order
        //     Log.Step("Step 12 — Verifying address details and order review");
        //
        //     // 13. Enter description in comment text area and click 'Place Order'
        //     Log.Step("Step 13 — Entering comment and placing order");
        //
        //     // 14. Enter payment details: Name on Card, Card Number, CVC, Expiration date
        //     Log.Step("Step 14 — Entering payment details");
        //
        //     // 15. Click 'Pay and Confirm Order' button
        //     Log.Step("Step 15 — Clicking Pay and Confirm Order");
        //
        //     // 16. Verify success message 'Your order has been placed successfully!'
        //     Log.Step("Step 16 — Verifying order success message");
        //
        //     // 17. Click 'Delete Account' button
        //     Log.Step("Step 17 — Clicking Delete Account");
        //
        //     // 18. Verify 'ACCOUNT DELETED!' and click 'Continue' button
        //     Log.Step("Step 18 — Verifying 'ACCOUNT DELETED!'");
        // }

        // ── TC16: Place Order: Login before Checkout ──────────────────────────────
        // ⚠️ Commented out — requires a pre-existing account on a live shared site.
        //
        // [TestMethod]
        // [TestCategory("Smoke")]
        // [TestCategory("Critical")]
        // [Description("TC16: Place Order: Login before Checkout")]
        // public async Task TC16_PlaceOrder_LoginBeforeCheckout()
        // {
        //     // 1. Launch browser — handled by BaseTest
        //     // 2. Navigate to url 'http://automationexercise.com'
        //     Log.Step("Step 2 — Navigating to homepage");
        //     await Page.GotoAsync("/");
        //
        //     // 3. Verify that home page is visible successfully
        //     Log.Step("Step 3 — Verifying homepage is visible");
        //     var homePage = new HomePage(Page);
        //     Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync());
        //
        //     // 4. Click 'Signup / Login' button
        //     Log.Step("Step 4 — Clicking Signup / Login button");
        //     var loginPage = await homePage.Nav.GoToLoginAsync();
        //
        //     // 5. Fill email, password and click 'Login' button
        //     Log.Step("Step 5 — Filling email and password and clicking Login");
        //     // await loginPage.LoginAsync(email, password);
        //
        //     // 6. Verify 'Logged in as username' at top
        //     Log.Step("Step 6 — Verifying 'Logged in as username'");
        //
        //     // 7. Add products to cart
        //     Log.Step("Step 7 — Adding products to cart");
        //
        //     // 8. Click 'Cart' button
        //     Log.Step("Step 8 — Clicking Cart button");
        //
        //     // 9. Verify that cart page is displayed
        //     Log.Step("Step 9 — Verifying cart page is displayed");
        //
        //     // 10. Click Proceed To Checkout
        //     Log.Step("Step 10 — Clicking Proceed To Checkout");
        //
        //     // 11. Verify Address Details and Review Your Order
        //     Log.Step("Step 11 — Verifying address details and order review");
        //
        //     // 12. Enter description in comment text area and click 'Place Order'
        //     Log.Step("Step 12 — Entering comment and placing order");
        //
        //     // 13. Enter payment details: Name on Card, Card Number, CVC, Expiration date
        //     Log.Step("Step 13 — Entering payment details");
        //
        //     // 14. Click 'Pay and Confirm Order' button
        //     Log.Step("Step 14 — Clicking Pay and Confirm Order");
        //
        //     // 15. Verify success message 'Your order has been placed successfully!'
        //     Log.Step("Step 15 — Verifying order success message");
        //
        //     // 16. Click 'Delete Account' button
        //     Log.Step("Step 16 — Clicking Delete Account");
        //
        //     // 17. Verify 'ACCOUNT DELETED!' and click 'Continue' button
        //     Log.Step("Step 17 — Verifying 'ACCOUNT DELETED!'");
        // }

        // ── TC23: Verify address details in checkout page ─────────────────────────
        // ⚠️ Commented out — requires account creation against a live shared site.
        //
        // [TestMethod]
        // [TestCategory("Regression")]
        // [Description("TC23: Verify address details in checkout page")]
        // public async Task TC23_VerifyAddressDetails_InCheckoutPage()
        // {
        //     // 1. Launch browser — handled by BaseTest
        //     // 2. Navigate to url 'http://automationexercise.com'
        //     Log.Step("Step 2 — Navigating to homepage");
        //     await Page.GotoAsync("/");
        //
        //     // 3. Verify that home page is visible successfully
        //     Log.Step("Step 3 — Verifying homepage is visible");
        //     var homePage = new HomePage(Page);
        //     Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync());
        //
        //     // 4. Click 'Signup / Login' button
        //     Log.Step("Step 4 — Clicking Signup / Login button");
        //     // var loginPage = await homePage.Nav.GoToLoginAsync();
        //
        //     // 5. Fill all details in Signup and create account
        //     Log.Step("Step 5 — Filling signup details and creating account");
        //
        //     // 6. Verify 'ACCOUNT CREATED!' and click 'Continue' button
        //     Log.Step("Step 6 — Verifying 'ACCOUNT CREATED!' and clicking Continue");
        //
        //     // 7. Verify 'Logged in as username' at top
        //     Log.Step("Step 7 — Verifying 'Logged in as username'");
        //
        //     // 8. Add products to cart
        //     Log.Step("Step 8 — Adding products to cart");
        //
        //     // 9. Click 'Cart' button
        //     Log.Step("Step 9 — Clicking Cart button");
        //
        //     // 10. Verify that cart page is displayed
        //     Log.Step("Step 10 — Verifying cart page is displayed");
        //
        //     // 11. Click Proceed To Checkout
        //     Log.Step("Step 11 — Clicking Proceed To Checkout");
        //
        //     // 12. Verify that the delivery address is same address filled at the time registration of account
        //     Log.Step("Step 12 — Verifying delivery address matches registration address");
        //
        //     // 13. Verify that the billing address is same address filled at the time registration of account
        //     Log.Step("Step 13 — Verifying billing address matches registration address");
        //
        //     // 14. Click 'Delete Account' button
        //     Log.Step("Step 14 — Clicking Delete Account");
        //
        //     // 15. Verify 'ACCOUNT DELETED!' and click 'Continue' button
        //     Log.Step("Step 15 — Verifying 'ACCOUNT DELETED!'");
        // }

        // ── TC24: Download Invoice after purchase order ───────────────────────────
        // ⚠️ Commented out — requires account creation against a live shared site.
        //
        // [TestMethod]
        // [TestCategory("Regression")]
        // [Description("TC24: Download Invoice after purchase order")]
        // public async Task TC24_DownloadInvoice_AfterPurchaseOrder()
        // {
        //     // 1. Launch browser — handled by BaseTest
        //     // 2. Navigate to url 'http://automationexercise.com'
        //     Log.Step("Step 2 — Navigating to homepage");
        //     await Page.GotoAsync("/");
        //
        //     // 3. Verify that home page is visible successfully
        //     Log.Step("Step 3 — Verifying homepage is visible");
        //     var homePage = new HomePage(Page);
        //     Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync());
        //
        //     // 4. Add products to cart
        //     Log.Step("Step 4 — Adding products to cart");
        //
        //     // 5. Click 'Cart' button
        //     Log.Step("Step 5 — Clicking Cart button");
        //
        //     // 6. Verify that cart page is displayed
        //     Log.Step("Step 6 — Verifying cart page is displayed");
        //
        //     // 7. Click Proceed To Checkout
        //     Log.Step("Step 7 — Clicking Proceed To Checkout");
        //
        //     // 8. Click 'Register / Login' button
        //     Log.Step("Step 8 — Clicking Register / Login from checkout prompt");
        //
        //     // 9. Fill all details in Signup and create account
        //     Log.Step("Step 9 — Filling signup details and creating account");
        //
        //     // 10. Verify 'ACCOUNT CREATED!' and click 'Continue' button
        //     Log.Step("Step 10 — Verifying 'ACCOUNT CREATED!' and clicking Continue");
        //
        //     // 11. Verify 'Logged in as username' at top
        //     Log.Step("Step 11 — Verifying 'Logged in as username'");
        //
        //     // 12. Click 'Cart' button
        //     Log.Step("Step 12 — Clicking Cart button");
        //
        //     // 13. Click 'Proceed To Checkout' button
        //     Log.Step("Step 13 — Clicking Proceed To Checkout");
        //
        //     // 14. Verify Address Details and Review Your Order
        //     Log.Step("Step 14 — Verifying address details and order review");
        //
        //     // 15. Enter description in comment text area and click 'Place Order'
        //     Log.Step("Step 15 — Entering comment and placing order");
        //
        //     // 16. Enter payment details: Name on Card, Card Number, CVC, Expiration date
        //     Log.Step("Step 16 — Entering payment details");
        //
        //     // 17. Click 'Pay and Confirm Order' button
        //     Log.Step("Step 17 — Clicking Pay and Confirm Order");
        //
        //     // 18. Verify success message 'Your order has been placed successfully!'
        //     Log.Step("Step 18 — Verifying order success message");
        //
        //     // 19. Click 'Download Invoice' button and verify invoice is downloaded successfully
        //     Log.Step("Step 19 — Downloading invoice and verifying");
        //     // Use context.WaitForDownloadAsync() to capture the file
        //
        //     // 20. Click 'Continue' button
        //     Log.Step("Step 20 — Clicking Continue");
        //
        //     // 21. Click 'Delete Account' button
        //     Log.Step("Step 21 — Clicking Delete Account");
        //
        //     // 22. Verify 'ACCOUNT DELETED!' and click 'Continue' button
        //     Log.Step("Step 22 — Verifying 'ACCOUNT DELETED!'");
        // }

        // ── TC14 partial — Guest checkout proceeds to Register/Login prompt ──────
        // This test covers the guest-accessible portion of TC14 steps 1-8:
        // adding a product, proceeding to checkout as a guest, and verifying the
        // Register/Login prompt appears. The full TC14 flow (account creation,
        // order placement) is commented out above.

        [TestMethod]
        [TestCategory("Regression")]
        [Description("TC14 (partial): Guest checkout proceeds to Register/Login prompt")]
        public async Task TC14_Partial_GuestCheckout_ProceedsToRegisterLoginPrompt()
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

            // 4. Add products to cart
            Log.Step("Step 4 — Adding a product to cart");
            var productsPage = new ProductsPage(Page);
            await productsPage.GoToAsync();
            await productsPage.AddProductToCartAsync(0);

            // 5. Click 'Cart' button
            Log.Step("Step 5 — Navigating to cart");
            await Page.GotoAsync("/view_cart");
            var cartPage = new CartPage(Page);

            // 6. Verify that cart page is displayed
            Log.Step("Step 6 — Verifying cart page is displayed");
            Assert.IsTrue(await cartPage.IsCartTableVisibleAsync(),
                "Expected cart table to be visible");

            // 7. Click Proceed To Checkout
            // 8. Click 'Register / Login' button
            Log.Step("Steps 7-8 — Proceeding to checkout as guest, expecting register/login prompt");
            var loginPage = await cartPage.GoToLoginFromCheckoutPromptAsync();

            // Verify login page is shown (guest users get redirected to register/login)
            Log.Step("Verifying login page is shown");
            Assert.IsTrue(await loginPage.IsLoginFormVisibleAsync(),
                "Expected login page to appear after guest clicks 'Register / Login' from checkout prompt");
        }
    }
}