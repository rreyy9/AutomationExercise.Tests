using AutomationExercise.UI.Tests.Utils;
using Bogus;

namespace AutomationExercise.UI.Tests.Tests.Auth
{
    [TestClass]
    [TestCategory("Regression")]
    public class LoginTests : BaseTest
    {
        // ── TC1: Register User ────────────────────────────────────────────────────
        //
        // Tagged [TestCategory("Account")] — only runs via manual workflow dispatch.
        // This test creates a user account on the live shared site and deletes it
        // afterward. The API delete endpoint is used in TestCleanup as a safety net
        // to guarantee cleanup even if the UI flow fails mid-test.

        private string? _accountEmail;
        private string? _accountPassword;

        [TestCleanup]
        public async Task AccountTestCleanup()
        {
            // Safety net: if an Account test created credentials, always attempt
            // API-level deletion regardless of test outcome. This prevents dangling
            // accounts on the live site even if the UI delete step never executes.
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

        [TestMethod]
        [TestCategory("Account")]
        [Description("TC1: Register User")]
        public async Task TC1_RegisterUser()
        {
            var faker = new Faker();
            var name = faker.Name.FullName();
            _accountEmail = $"pw.tc1.{Guid.NewGuid():N}@example.com";
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

            // 4. Click on 'Signup / Login' button
            Log.Step("Step 4 — Clicking Signup / Login button");
            var loginPage = await homePage.Nav.GoToLoginAsync();

            // 5. Verify 'New User Signup!' is visible
            Log.Step("Step 5 — Verifying 'New User Signup!' is visible");
            Assert.IsTrue(await loginPage.IsSignupFormVisibleAsync(),
                "Expected 'New User Signup!' heading to be visible");

            // 6. Enter name and email address
            // 7. Click 'Signup' button
            Log.Step("Steps 6-7 — Entering name and email, clicking Signup");
            var signupPage = await loginPage.StartSignupAsync(name, _accountEmail);

            // 8. Verify that 'ENTER ACCOUNT INFORMATION' is visible
            Log.Step("Step 8 — Verifying 'ENTER ACCOUNT INFORMATION' is visible");
            var heading = await signupPage.GetHeadingTextAsync();
            Assert.AreEqual("ENTER ACCOUNT INFORMATION", heading,
                $"Expected 'ENTER ACCOUNT INFORMATION' but got '{heading}'");

            // 9-12. Fill details and create account
            Log.Step("Steps 9-12 — Filling account details");
            var details = new AccountDetails
            {
                Title = "Mr",
                Password = _accountPassword,
                DateOfBirthDay = "15",
                DateOfBirthMonth = "June",
                DateOfBirthYear = "1990",
                SignUpForNewsletter = true,
                ReceiveSpecialOffers = false,
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
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

            // 13. Click 'Create Account' button
            Log.Step("Step 13 — Clicking Create Account");
            await signupPage.CreateAccountAsync();

            // 14. Verify that 'ACCOUNT CREATED!' is visible
            Log.Step("Step 14 — Verifying 'ACCOUNT CREATED!' is visible");
            Assert.IsTrue(await signupPage.IsAccountCreatedHeadingVisibleAsync(),
                "Expected 'ACCOUNT CREATED!' heading to be visible");

            // 15. Click 'Continue' button
            Log.Step("Step 15 — Clicking Continue");
            await signupPage.ClickContinueAsync();

            // 16. Verify that 'Logged in as username' is visible
            Log.Step("Step 16 — Verifying 'Logged in as username' is visible");
            Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync(),
                "Expected user to be logged in after account creation");
            var loggedInName = await homePage.Nav.GetLoggedInUsernameAsync();
            Log.Info($"Logged in as: '{loggedInName}'");

            // 17. Click 'Delete Account' button
            Log.Step("Step 17 — Clicking Delete Account");
            await homePage.Nav.DeleteAccountAsync();

            // 18. Verify that 'ACCOUNT DELETED!' is visible
            Log.Step("Step 18 — Verifying 'ACCOUNT DELETED!' is visible");
            var deletedHeading = Page.Locator("h2[data-qa='account-deleted']");
            await Assertions.Expect(deletedHeading).ToBeVisibleAsync();
            Log.Info("Account deleted successfully via UI");

            // Click Continue after deletion
            await Page.Locator("a[data-qa='continue-button']").ClickAsync();

            // Clear credentials so TestCleanup doesn't attempt redundant API delete
            _accountEmail = null;
            _accountPassword = null;
        }

        // ── TC2: Login User with correct email and password ───────────────────────
        //
        // Tagged [TestCategory("Account")] — only runs via manual workflow dispatch.
        // Creates an account via the API before the UI login flow, then deletes via API in cleanup.

        [TestMethod]
        [TestCategory("Account")]
        [Description("TC2: Login User with correct email and password")]
        public async Task TC2_LoginUser_WithCorrectCredentials()
        {
            var faker = new Faker();
            _accountEmail = $"pw.tc2.{Guid.NewGuid():N}@example.com";
            _accountPassword = "TestPass1234!";
            var firstName = faker.Name.FirstName();

            // Pre-create account via API so the UI test only exercises the login flow
            using var http = new HttpClient { BaseAddress = new Uri("https://automationexercise.com") };
            var createResponse = await http.PostAsync("/api/createAccount", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "name", $"{firstName} Test" }, { "email", _accountEmail }, { "password", _accountPassword },
                { "title", "Mr" }, { "birth_date", "1" }, { "birth_month", "1" }, { "birth_year", "1990" },
                { "firstname", firstName }, { "lastname", "Test" }, { "company", "" },
                { "address1", "1 Test Lane" }, { "address2", "" }, { "country", "United States" },
                { "zipcode", "10001" }, { "state", "New York" }, { "city", "New York" },
                { "mobile_number", "1234567890" }
            }));
            Log.Info($"API account creation status: {createResponse.StatusCode}");

            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Click on 'Signup / Login' button
            Log.Step("Step 4 — Clicking Signup / Login button");
            var loginPage = await homePage.Nav.GoToLoginAsync();

            // 5. Verify 'Login to your account' is visible
            Log.Step("Step 5 — Verifying 'Login to your account' is visible");
            Assert.IsTrue(await loginPage.IsLoginFormVisibleAsync(),
                "Expected 'Login to your account' heading to be visible");

            // 6. Enter correct email address and password
            // 7. Click 'login' button
            Log.Step("Steps 6-7 — Entering correct email and password, clicking login");
            await loginPage.LoginAsync(_accountEmail, _accountPassword);

            // 8. Verify that 'Logged in as username' is visible
            Log.Step("Step 8 — Verifying 'Logged in as username' is visible");
            Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync(),
                "Expected user to be logged in after entering valid credentials");
            var loggedInName = await homePage.Nav.GetLoggedInUsernameAsync();
            Log.Info($"Logged in as: '{loggedInName}'");

            // Cleanup happens via API in TestCleanup — no UI delete needed
        }

        // ── TC3: Login User with incorrect email and password ─────────────────────

        [TestMethod]
        [Description("TC3: Login User with incorrect email and password")]
        public async Task TC3_LoginUser_WithIncorrectCredentials()
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

            // 4. Click on 'Signup / Login' button
            Log.Step("Step 4 — Clicking Signup / Login button");
            var loginPage = await homePage.Nav.GoToLoginAsync();

            // 5. Verify 'Login to your account' is visible
            Log.Step("Step 5 — Verifying 'Login to your account' is visible");
            Assert.IsTrue(await loginPage.IsLoginFormVisibleAsync(),
                "Expected 'Login to your account' heading to be visible");

            // 6. Enter incorrect email address and password
            Log.Step("Step 6 — Entering incorrect email and password");
            // 7. Click 'login' button
            Log.Step("Step 7 — Clicking login button");
            await loginPage.LoginAsync("notareal@email.com", "wrongpassword");

            // 8. Verify error 'Your email or password is incorrect!' is visible
            Log.Step("Step 8 — Verifying error message is visible");
            Assert.IsTrue(
                await loginPage.IsLoginErrorVisibleAsync(),
                "Expected error 'Your email or password is incorrect!' to be visible");
        }

        // ── TC4: Logout User ──────────────────────────────────────────────────────
        //
        // Tagged [TestCategory("Account")] — only runs via manual workflow dispatch.
        // Creates an account via API, logs in via UI, then logs out and verifies redirect.

        [TestMethod]
        [TestCategory("Account")]
        [Description("TC4: Logout User")]
        public async Task TC4_LogoutUser()
        {
            var faker = new Faker();
            _accountEmail = $"pw.tc4.{Guid.NewGuid():N}@example.com";
            _accountPassword = "TestPass1234!";
            var firstName = faker.Name.FirstName();

            // Pre-create account via API
            using var http = new HttpClient { BaseAddress = new Uri("https://automationexercise.com") };
            await http.PostAsync("/api/createAccount", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "name", $"{firstName} Test" }, { "email", _accountEmail }, { "password", _accountPassword },
                { "title", "Mrs" }, { "birth_date", "10" }, { "birth_month", "3" }, { "birth_year", "1992" },
                { "firstname", firstName }, { "lastname", "Test" }, { "company", "" },
                { "address1", "2 Test Lane" }, { "address2", "" }, { "country", "United States" },
                { "zipcode", "10002" }, { "state", "New York" }, { "city", "New York" },
                { "mobile_number", "1234567890" }
            }));

            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Click on 'Signup / Login' button
            Log.Step("Step 4 — Clicking Signup / Login button");
            var loginPage = await homePage.Nav.GoToLoginAsync();

            // 5. Verify 'Login to your account' is visible
            Log.Step("Step 5 — Verifying 'Login to your account' is visible");
            Assert.IsTrue(await loginPage.IsLoginFormVisibleAsync(),
                "Expected 'Login to your account' heading to be visible");

            // 6. Enter correct email address and password
            // 7. Click 'login' button
            Log.Step("Steps 6-7 — Entering correct credentials and clicking login");
            await loginPage.LoginAsync(_accountEmail, _accountPassword);

            // 8. Verify that 'Logged in as username' is visible
            Log.Step("Step 8 — Verifying 'Logged in as username' is visible");
            Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync(),
                "Expected user to be logged in");

            // 9. Click 'Logout' button
            Log.Step("Step 9 — Clicking Logout");
            await homePage.Nav.LogoutAsync();

            // 10. Verify that user is navigated to login page
            Log.Step("Step 10 — Verifying user is on login page");
            loginPage = new LoginPage(Page);
            Assert.IsTrue(await loginPage.IsLoginFormVisibleAsync(),
                "Expected login page to be visible after logout");
            Log.Info("Logout successful — user redirected to login page");
        }

        // ── TC5: Register User with existing email ────────────────────────────────
        //
        // Tagged [TestCategory("Account")] — only runs via manual workflow dispatch.
        // Creates an account via API, then attempts to register again with the same email via UI.

        [TestMethod]
        [TestCategory("Account")]
        [Description("TC5: Register User with existing email")]
        public async Task TC5_RegisterUser_WithExistingEmail()
        {
            var faker = new Faker();
            _accountEmail = $"pw.tc5.{Guid.NewGuid():N}@example.com";
            _accountPassword = "TestPass1234!";
            var firstName = faker.Name.FirstName();

            // Pre-create account via API so we can test duplicate registration
            using var http = new HttpClient { BaseAddress = new Uri("https://automationexercise.com") };
            await http.PostAsync("/api/createAccount", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "name", $"{firstName} Test" }, { "email", _accountEmail }, { "password", _accountPassword },
                { "title", "Mr" }, { "birth_date", "5" }, { "birth_month", "5" }, { "birth_year", "1988" },
                { "firstname", firstName }, { "lastname", "Test" }, { "company", "" },
                { "address1", "3 Test Lane" }, { "address2", "" }, { "country", "United States" },
                { "zipcode", "10003" }, { "state", "New York" }, { "city", "New York" },
                { "mobile_number", "1234567890" }
            }));

            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Click on 'Signup / Login' button
            Log.Step("Step 4 — Clicking Signup / Login button");
            var loginPage = await homePage.Nav.GoToLoginAsync();

            // 5. Verify 'New User Signup!' is visible
            Log.Step("Step 5 — Verifying 'New User Signup!' is visible");
            Assert.IsTrue(await loginPage.IsSignupFormVisibleAsync(),
                "Expected 'New User Signup!' heading to be visible");

            // 6. Enter name and already registered email address
            // 7. Click 'Signup' button
            Log.Step("Steps 6-7 — Entering name and already-registered email, clicking Signup");
            await loginPage.StartSignupAsync("Duplicate User", _accountEmail);

            // 8. Verify error 'Email Address already exist!' is visible
            Log.Step("Step 8 — Verifying error message is visible");
            Assert.IsTrue(await loginPage.IsSignupErrorVisibleAsync(),
                "Expected 'Email Address already exist!' error to be visible");
            Log.Info("Duplicate registration correctly rejected");
        }
    }
}