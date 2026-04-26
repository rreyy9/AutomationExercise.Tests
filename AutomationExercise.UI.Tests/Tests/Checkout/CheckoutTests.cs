using Bogus;

namespace AutomationExercise.UI.Tests.Tests.Checkout
{
    [TestClass]
    public class CheckoutTests : BaseTest
    {
        // ── Account cleanup safety net ────────────────────────────────────────────
        // Account-tagged tests store credentials here so TestCleanup can guarantee
        // API-level deletion even if the UI flow fails mid-test.

        private string? _accountEmail;
        private string? _accountPassword;

        [TestCleanup]
        public async Task AccountTestCleanup()
        {
            if (_accountEmail != null && _accountPassword != null)
            {
                try
                {
                    using var http = new HttpClient { BaseAddress = new Uri("https://automationexercise.com") };
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "email", _accountEmail },
                        { "password", _accountPassword }
                    });
                    var request = new HttpRequestMessage(HttpMethod.Delete, "/api/deleteAccount") { Content = content };
                    await http.SendAsync(request);
                    Log.Info($"Account cleanup: sent DELETE for {_accountEmail}");
                }
                catch (Exception ex)
                {
                    Log.Warning($"Account cleanup failed: {ex.Message}");
                }
            }
        }

        // ── Helper: create account via API ────────────────────────────────────────
        // Used by Account-tagged checkout tests to set up a user without a full UI
        // registration flow — the test focus is checkout, not registration.

        private async Task CreateAccountViaApiAsync(string name, string firstName, string lastName)
        {
            using var http = new HttpClient { BaseAddress = new Uri("https://automationexercise.com") };
            await http.PostAsync("/api/createAccount", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "name", name }, { "email", _accountEmail! }, { "password", _accountPassword! },
                { "title", "Mr" }, { "birth_date", "1" }, { "birth_month", "1" }, { "birth_year", "1990" },
                { "firstname", firstName }, { "lastname", lastName }, { "company", "Test Co" },
                { "address1", "123 Test Street" }, { "address2", "Apt 1" }, { "country", "United States" },
                { "zipcode", "10001" }, { "state", "New York" }, { "city", "New York" },
                { "mobile_number", "1234567890" }
            }));
            Log.Info($"API account created: {_accountEmail}");
        }

        // ── TC14: Place Order: Register while Checkout ────────────────────────────
        //
        // Tagged [TestCategory("Account")] — only runs via manual workflow dispatch.
        // Full end-to-end: add to cart → checkout as guest → register → complete order → delete account.

        [TestMethod]
        [TestCategory("Account")]
        [Description("TC14: Place Order: Register while Checkout")]
        public async Task TC14_PlaceOrder_RegisterWhileCheckout()
        {
            var faker = new Faker();
            _accountEmail = $"pw.tc14.{Guid.NewGuid():N}@example.com";
            _accountPassword = "TestPass1234!";

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
            Log.Step("Step 4 — Adding products to cart");
            var productsPage = new ProductsPage(Page);
            await productsPage.GoToAsync();
            await productsPage.AddProductToCartAsync(0);

            // 5. Click 'Cart' button
            Log.Step("Step 5 — Clicking Cart button");
            var cartPage = await homePage.Nav.GoToCartAsync();

            // 6. Verify that cart page is displayed
            Log.Step("Step 6 — Verifying cart page is displayed");
            Assert.IsTrue(await cartPage.IsCartTableVisibleAsync(),
                "Expected cart table to be visible");

            // 7. Click Proceed To Checkout
            // 8. Click 'Register / Login' button
            Log.Step("Steps 7-8 — Clicking Proceed To Checkout, then Register / Login");
            var loginPage = await cartPage.GoToLoginFromCheckoutPromptAsync();

            // 9. Fill all details in Signup and create account
            Log.Step("Step 9 — Filling signup details and creating account");
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();
            var signupPage = await loginPage.StartSignupAsync($"{firstName} {lastName}", _accountEmail);

            var details = new AccountDetails
            {
                Title = "Mr",
                Password = _accountPassword,
                DateOfBirthDay = "15",
                DateOfBirthMonth = "June",
                DateOfBirthYear = "1990",
                SignUpForNewsletter = false,
                ReceiveSpecialOffers = false,
                FirstName = firstName,
                LastName = lastName,
                Company = faker.Company.CompanyName(),
                Address1 = faker.Address.StreetAddress(),
                Address2 = faker.Address.SecondaryAddress(),
                Country = "United States",
                State = faker.Address.State(),
                City = faker.Address.City(),
                Zipcode = faker.Address.ZipCode(),
                MobileNumber = faker.Phone.PhoneNumber("##########")
            };
            await signupPage.FillAccountDetailsAsync(details);

            // 10. Click 'Create Account' and verify 'ACCOUNT CREATED!'
            Log.Step("Step 10 — Creating account and verifying confirmation");
            await signupPage.CreateAccountAsync();
            Assert.IsTrue(await signupPage.IsAccountCreatedHeadingVisibleAsync(),
                "Expected 'ACCOUNT CREATED!' heading");

            // Click Continue
            await signupPage.ClickContinueAsync();

            // 11. Verify 'Logged in as username' at top
            Log.Step("Step 11 — Verifying 'Logged in as username'");
            Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync(),
                "Expected user to be logged in after registration");

            // 12. Click 'Cart' button
            Log.Step("Step 12 — Clicking Cart button");
            cartPage = await homePage.Nav.GoToCartAsync();

            // 13. Click 'Proceed To Checkout' button
            Log.Step("Step 13 — Clicking Proceed To Checkout");
            var checkoutPage = await cartPage.ProceedToCheckoutAsync();

            // 14. Verify Address Details and Review Your Order
            Log.Step("Step 14 — Verifying address details and order review");
            Assert.IsTrue(await checkoutPage.IsOrderTableVisibleAsync(),
                "Expected order table to be visible on checkout page");

            // 15. Enter description in comment text area and click 'Place Order'
            Log.Step("Step 15 — Entering comment and placing order");
            await checkoutPage.AddCommentAsync("Automated test order — TC14");
            var paymentPage = await checkoutPage.PlaceOrderAsync();

            // 16. Enter payment details
            Log.Step("Step 16 — Entering payment details");
            var card = new CardDetails
            {
                NameOnCard = $"{firstName} {lastName}",
                CardNumber = "4100000000000",
                Cvc = "123",
                ExpiryMonth = "12",
                ExpiryYear = "2029"
            };
            await paymentPage.EnterCardDetailsAndConfirmAsync(card);

            // 18. Verify success message
            Log.Step("Step 18 — Verifying order success message");
            Assert.IsTrue(await paymentPage.IsSuccessMessageVisibleAsync(),
                "Expected order confirmation success message");
            Log.Info("Order placed successfully");

            // 19. Click 'Delete Account' button
            Log.Step("Step 19 — Clicking Delete Account");
            await homePage.Nav.DeleteAccountAsync();

            // 20. Verify 'ACCOUNT DELETED!'
            Log.Step("Step 20 — Verifying 'ACCOUNT DELETED!'");
            var deletedHeading = Page.Locator("h2[data-qa='account-deleted']");
            await Assertions.Expect(deletedHeading).ToBeVisibleAsync();
            await Page.Locator("a[data-qa='continue-button']").ClickAsync();
            Log.Info("Account deleted via UI");

            _accountEmail = null;
            _accountPassword = null;
        }

        // ── TC15: Place Order: Register before Checkout ───────────────────────────

        [TestMethod]
        [TestCategory("Account")]
        [Description("TC15: Place Order: Register before Checkout")]
        public async Task TC15_PlaceOrder_RegisterBeforeCheckout()
        {
            var faker = new Faker();
            _accountEmail = $"pw.tc15.{Guid.NewGuid():N}@example.com";
            _accountPassword = "TestPass1234!";
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();

            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Click 'Signup / Login' button
            Log.Step("Step 4 — Clicking Signup / Login button");
            var loginPage = await homePage.Nav.GoToLoginAsync();

            // 5. Fill all details in Signup and create account
            Log.Step("Step 5 — Filling signup details and creating account");
            var signupPage = await loginPage.StartSignupAsync($"{firstName} {lastName}", _accountEmail);
            var details = new AccountDetails
            {
                Title = "Mrs",
                Password = _accountPassword,
                DateOfBirthDay = "20",
                DateOfBirthMonth = "March",
                DateOfBirthYear = "1985",
                SignUpForNewsletter = true,
                ReceiveSpecialOffers = true,
                FirstName = firstName,
                LastName = lastName,
                Company = faker.Company.CompanyName(),
                Address1 = faker.Address.StreetAddress(),
                Address2 = faker.Address.SecondaryAddress(),
                Country = "United States",
                State = faker.Address.State(),
                City = faker.Address.City(),
                Zipcode = faker.Address.ZipCode(),
                MobileNumber = faker.Phone.PhoneNumber("##########")
            };
            await signupPage.FillAccountDetailsAsync(details);

            // 6. Verify 'ACCOUNT CREATED!' and click 'Continue' button
            Log.Step("Step 6 — Creating account and verifying confirmation");
            await signupPage.CreateAccountAsync();
            Assert.IsTrue(await signupPage.IsAccountCreatedHeadingVisibleAsync(),
                "Expected 'ACCOUNT CREATED!' heading");
            await signupPage.ClickContinueAsync();

            // 7. Verify 'Logged in as username' at top
            Log.Step("Step 7 — Verifying 'Logged in as username'");
            Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync(),
                "Expected user to be logged in");

            // 8. Add products to cart
            Log.Step("Step 8 — Adding products to cart");
            var productsPage = new ProductsPage(Page);
            await productsPage.GoToAsync();
            await productsPage.AddProductToCartAsync(0);

            // 9. Click 'Cart' button
            Log.Step("Step 9 — Clicking Cart button");
            var cartPage = await homePage.Nav.GoToCartAsync();

            // 10. Verify that cart page is displayed
            Log.Step("Step 10 — Verifying cart page is displayed");
            Assert.IsTrue(await cartPage.IsCartTableVisibleAsync(),
                "Expected cart table to be visible");

            // 11. Click Proceed To Checkout
            Log.Step("Step 11 — Clicking Proceed To Checkout");
            var checkoutPage = await cartPage.ProceedToCheckoutAsync();

            // 12. Verify Address Details and Review Your Order
            Log.Step("Step 12 — Verifying address details and order review");
            Assert.IsTrue(await checkoutPage.IsOrderTableVisibleAsync(),
                "Expected order table on checkout page");

            // 13. Enter description in comment text area and click 'Place Order'
            Log.Step("Step 13 — Entering comment and placing order");
            await checkoutPage.AddCommentAsync("Automated test order — TC15");
            var paymentPage = await checkoutPage.PlaceOrderAsync();

            // 14. Enter payment details
            Log.Step("Step 14 — Entering payment details");
            var card = new CardDetails
            {
                NameOnCard = $"{firstName} {lastName}",
                CardNumber = "4100000000000",
                Cvc = "456",
                ExpiryMonth = "06",
                ExpiryYear = "2028"
            };
            await paymentPage.EnterCardDetailsAndConfirmAsync(card);

            // 16. Verify success message
            Log.Step("Step 16 — Verifying order success message");
            Assert.IsTrue(await paymentPage.IsSuccessMessageVisibleAsync(),
                "Expected order confirmation success message");

            // 17. Click 'Delete Account' button
            Log.Step("Step 17 — Clicking Delete Account");
            await homePage.Nav.DeleteAccountAsync();

            // 18. Verify 'ACCOUNT DELETED!'
            Log.Step("Step 18 — Verifying 'ACCOUNT DELETED!'");
            var deletedHeading = Page.Locator("h2[data-qa='account-deleted']");
            await Assertions.Expect(deletedHeading).ToBeVisibleAsync();
            await Page.Locator("a[data-qa='continue-button']").ClickAsync();

            _accountEmail = null;
            _accountPassword = null;
        }

        // ── TC16: Place Order: Login before Checkout ──────────────────────────────

        [TestMethod]
        [TestCategory("Account")]
        [Description("TC16: Place Order: Login before Checkout")]
        public async Task TC16_PlaceOrder_LoginBeforeCheckout()
        {
            var faker = new Faker();
            _accountEmail = $"pw.tc16.{Guid.NewGuid():N}@example.com";
            _accountPassword = "TestPass1234!";
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();

            // Pre-create account via API — test focus is checkout, not registration
            await CreateAccountViaApiAsync($"{firstName} {lastName}", firstName, lastName);

            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Click 'Signup / Login' button
            Log.Step("Step 4 — Clicking Signup / Login button");
            var loginPage = await homePage.Nav.GoToLoginAsync();

            // 5. Fill email, password and click 'Login' button
            Log.Step("Step 5 — Logging in with valid credentials");
            await loginPage.LoginAsync(_accountEmail, _accountPassword);

            // 6. Verify 'Logged in as username' at top
            Log.Step("Step 6 — Verifying 'Logged in as username'");
            Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync(),
                "Expected user to be logged in");

            // 7. Add products to cart
            Log.Step("Step 7 — Adding products to cart");
            var productsPage = new ProductsPage(Page);
            await productsPage.GoToAsync();
            await productsPage.AddProductToCartAsync(0);

            // 8. Click 'Cart' button
            Log.Step("Step 8 — Clicking Cart button");
            var cartPage = await homePage.Nav.GoToCartAsync();

            // 9. Verify that cart page is displayed
            Log.Step("Step 9 — Verifying cart page is displayed");
            Assert.IsTrue(await cartPage.IsCartTableVisibleAsync(),
                "Expected cart table to be visible");

            // 10. Click Proceed To Checkout
            Log.Step("Step 10 — Clicking Proceed To Checkout");
            var checkoutPage = await cartPage.ProceedToCheckoutAsync();

            // 11. Verify Address Details and Review Your Order
            Log.Step("Step 11 — Verifying address details and order review");
            Assert.IsTrue(await checkoutPage.IsOrderTableVisibleAsync(),
                "Expected order table on checkout page");

            // 12. Enter description in comment text area and click 'Place Order'
            Log.Step("Step 12 — Entering comment and placing order");
            await checkoutPage.AddCommentAsync("Automated test order — TC16");
            var paymentPage = await checkoutPage.PlaceOrderAsync();

            // 13. Enter payment details
            Log.Step("Step 13 — Entering payment details");
            var card = new CardDetails
            {
                NameOnCard = $"{firstName} {lastName}",
                CardNumber = "4100000000000",
                Cvc = "789",
                ExpiryMonth = "09",
                ExpiryYear = "2030"
            };
            await paymentPage.EnterCardDetailsAndConfirmAsync(card);

            // 15. Verify success message
            Log.Step("Step 15 — Verifying order success message");
            Assert.IsTrue(await paymentPage.IsSuccessMessageVisibleAsync(),
                "Expected order confirmation success message");
            Log.Info("Order placed successfully via login-before-checkout flow");

            // Cleanup via API in TestCleanup
        }

        // ── TC23: Verify address details in checkout page ─────────────────────────

        [TestMethod]
        [TestCategory("Account")]
        [Description("TC23: Verify address details in checkout page")]
        public async Task TC23_VerifyAddressDetails_InCheckoutPage()
        {
            var faker = new Faker();
            _accountEmail = $"pw.tc23.{Guid.NewGuid():N}@example.com";
            _accountPassword = "TestPass1234!";
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();

            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Click 'Signup / Login' button
            Log.Step("Step 4 — Clicking Signup / Login button");
            var loginPage = await homePage.Nav.GoToLoginAsync();

            // 5. Fill all details in Signup and create account
            Log.Step("Step 5 — Filling signup details and creating account");
            var signupPage = await loginPage.StartSignupAsync($"{firstName} {lastName}", _accountEmail);
            var address1 = faker.Address.StreetAddress();
            var state = faker.Address.State();
            var city = faker.Address.City();
            var zipcode = faker.Address.ZipCode();

            var details = new AccountDetails
            {
                Title = "Mr",
                Password = _accountPassword,
                DateOfBirthDay = "10",
                DateOfBirthMonth = "October",
                DateOfBirthYear = "1991",
                SignUpForNewsletter = false,
                ReceiveSpecialOffers = false,
                FirstName = firstName,
                LastName = lastName,
                Company = "Address Verify Co",
                Address1 = address1,
                Address2 = "",
                Country = "United States",
                State = state,
                City = city,
                Zipcode = zipcode,
                MobileNumber = faker.Phone.PhoneNumber("##########")
            };
            await signupPage.FillAccountDetailsAsync(details);

            // 6. Verify 'ACCOUNT CREATED!' and click 'Continue' button
            Log.Step("Step 6 — Creating account and verifying confirmation");
            await signupPage.CreateAccountAsync();
            Assert.IsTrue(await signupPage.IsAccountCreatedHeadingVisibleAsync(),
                "Expected 'ACCOUNT CREATED!' heading");
            await signupPage.ClickContinueAsync();

            // 7. Verify 'Logged in as username' at top
            Log.Step("Step 7 — Verifying 'Logged in as username'");
            Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync(),
                "Expected user to be logged in");

            // 8. Add products to cart
            Log.Step("Step 8 — Adding products to cart");
            var productsPage = new ProductsPage(Page);
            await productsPage.GoToAsync();
            await productsPage.AddProductToCartAsync(0);

            // 9. Click 'Cart' button
            Log.Step("Step 9 — Clicking Cart button");
            var cartPage = await homePage.Nav.GoToCartAsync();

            // 10. Verify that cart page is displayed
            Log.Step("Step 10 — Verifying cart page is displayed");
            Assert.IsTrue(await cartPage.IsCartTableVisibleAsync(),
                "Expected cart table to be visible");

            // 11. Click Proceed To Checkout
            Log.Step("Step 11 — Clicking Proceed To Checkout");
            var checkoutPage = await cartPage.ProceedToCheckoutAsync();

            // 12. Verify that the delivery address matches registration
            Log.Step("Step 12 — Verifying delivery address matches registration address");
            var deliveryAddress = await checkoutPage.GetDeliveryAddressAsync();
            Assert.IsTrue(deliveryAddress.Contains(firstName),
                $"Expected delivery address to contain first name '{firstName}'");
            Assert.IsTrue(deliveryAddress.Contains(address1),
                $"Expected delivery address to contain address1 '{address1}'");
            Assert.IsTrue(deliveryAddress.Contains(city),
                $"Expected delivery address to contain city '{city}'");
            Log.Info($"Delivery address verified: contains '{firstName}', '{address1}', '{city}'");

            // 13. Verify that the billing address matches registration
            Log.Step("Step 13 — Verifying billing address matches registration address");
            var billingAddress = await checkoutPage.GetBillingAddressAsync();
            Assert.IsTrue(billingAddress.Contains(firstName),
                $"Expected billing address to contain first name '{firstName}'");
            Assert.IsTrue(billingAddress.Contains(address1),
                $"Expected billing address to contain address1 '{address1}'");
            Log.Info("Billing address verified");

            // 14. Click 'Delete Account' button
            Log.Step("Step 14 — Clicking Delete Account");
            await homePage.Nav.DeleteAccountAsync();

            // 15. Verify 'ACCOUNT DELETED!'
            Log.Step("Step 15 — Verifying 'ACCOUNT DELETED!'");
            var deletedHeading = Page.Locator("h2[data-qa='account-deleted']");
            await Assertions.Expect(deletedHeading).ToBeVisibleAsync();
            await Page.Locator("a[data-qa='continue-button']").ClickAsync();

            _accountEmail = null;
            _accountPassword = null;
        }

        // ── TC24: Download Invoice after purchase order ───────────────────────────

        [TestMethod]
        [TestCategory("Account")]
        [Description("TC24: Download Invoice after purchase order")]
        public async Task TC24_DownloadInvoice_AfterPurchaseOrder()
        {
            var faker = new Faker();
            _accountEmail = $"pw.tc24.{Guid.NewGuid():N}@example.com";
            _accountPassword = "TestPass1234!";
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();

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
            Log.Step("Step 4 — Adding products to cart");
            var productsPage = new ProductsPage(Page);
            await productsPage.GoToAsync();
            await productsPage.AddProductToCartAsync(0);

            // 5. Click 'Cart' button
            Log.Step("Step 5 — Clicking Cart button");
            var cartPage = await homePage.Nav.GoToCartAsync();

            // 6. Verify that cart page is displayed
            Log.Step("Step 6 — Verifying cart page is displayed");
            Assert.IsTrue(await cartPage.IsCartTableVisibleAsync(),
                "Expected cart table to be visible");

            // 7. Click Proceed To Checkout
            // 8. Click 'Register / Login' button
            Log.Step("Steps 7-8 — Clicking Proceed To Checkout, then Register / Login");
            var loginPage = await cartPage.GoToLoginFromCheckoutPromptAsync();

            // 9. Fill all details in Signup and create account
            Log.Step("Step 9 — Filling signup details and creating account");
            var signupPage = await loginPage.StartSignupAsync($"{firstName} {lastName}", _accountEmail);
            var details = new AccountDetails
            {
                Title = "Mr",
                Password = _accountPassword,
                DateOfBirthDay = "5",
                DateOfBirthMonth = "May",
                DateOfBirthYear = "1993",
                SignUpForNewsletter = false,
                ReceiveSpecialOffers = false,
                FirstName = firstName,
                LastName = lastName,
                Company = "",
                Address1 = faker.Address.StreetAddress(),
                Country = "United States",
                State = faker.Address.State(),
                City = faker.Address.City(),
                Zipcode = faker.Address.ZipCode(),
                MobileNumber = faker.Phone.PhoneNumber("##########")
            };
            await signupPage.FillAccountDetailsAsync(details);

            // 10. Verify 'ACCOUNT CREATED!' and click 'Continue' button
            Log.Step("Step 10 — Creating account and verifying confirmation");
            await signupPage.CreateAccountAsync();
            Assert.IsTrue(await signupPage.IsAccountCreatedHeadingVisibleAsync(),
                "Expected 'ACCOUNT CREATED!' heading");
            await signupPage.ClickContinueAsync();

            // 11. Verify 'Logged in as username' at top
            Log.Step("Step 11 — Verifying 'Logged in as username'");
            Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync(),
                "Expected user to be logged in");

            // 12. Click 'Cart' button
            Log.Step("Step 12 — Clicking Cart button");
            cartPage = await homePage.Nav.GoToCartAsync();

            // 13. Click 'Proceed To Checkout' button
            Log.Step("Step 13 — Clicking Proceed To Checkout");
            var checkoutPage = await cartPage.ProceedToCheckoutAsync();

            // 14. Verify Address Details and Review Your Order
            Log.Step("Step 14 — Verifying order review");
            Assert.IsTrue(await checkoutPage.IsOrderTableVisibleAsync(),
                "Expected order table on checkout page");

            // 15. Enter description in comment text area and click 'Place Order'
            Log.Step("Step 15 — Entering comment and placing order");
            await checkoutPage.AddCommentAsync("Automated test order — TC24 invoice download");
            var paymentPage = await checkoutPage.PlaceOrderAsync();

            // 16. Enter payment details
            Log.Step("Step 16 — Entering payment details");
            var card = new CardDetails
            {
                NameOnCard = $"{firstName} {lastName}",
                CardNumber = "4100000000000",
                Cvc = "321",
                ExpiryMonth = "03",
                ExpiryYear = "2031"
            };
            await paymentPage.EnterCardDetailsAndConfirmAsync(card);

            // 18. Verify success message
            Log.Step("Step 18 — Verifying order success message");
            Assert.IsTrue(await paymentPage.IsSuccessMessageVisibleAsync(),
                "Expected order confirmation success message");

            // 19. Click 'Download Invoice' button and verify invoice is downloaded
            Log.Step("Step 19 — Downloading invoice");
            Assert.IsTrue(await paymentPage.IsDownloadInvoiceButtonVisibleAsync(),
                "Expected Download Invoice button to be visible");

            var downloadTask = Page.WaitForDownloadAsync();
            await paymentPage.DownloadInvoiceAsync();
            var download = await downloadTask;

            Assert.IsFalse(string.IsNullOrWhiteSpace(download.SuggestedFilename),
                "Expected downloaded invoice to have a filename");
            Log.Info($"Invoice downloaded: {download.SuggestedFilename}");

            // 20. Click 'Continue' button
            Log.Step("Step 20 — Clicking Continue");
            await paymentPage.ClickContinueAsync();

            // 21. Click 'Delete Account' button
            Log.Step("Step 21 — Clicking Delete Account");
            await homePage.Nav.DeleteAccountAsync();

            // 22. Verify 'ACCOUNT DELETED!'
            Log.Step("Step 22 — Verifying 'ACCOUNT DELETED!'");
            var deletedHeading = Page.Locator("h2[data-qa='account-deleted']");
            await Assertions.Expect(deletedHeading).ToBeVisibleAsync();
            await Page.Locator("a[data-qa='continue-button']").ClickAsync();

            _accountEmail = null;
            _accountPassword = null;
        }

        // ── TC14 partial — Guest checkout proceeds to Register/Login prompt ──────
        // This covers the guest-accessible portion of the checkout flow.
        // No account required — always safe to run in CI.

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