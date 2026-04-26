namespace AutomationExercise.UI.Tests.Tests.Auth
{
    [TestClass]
    [TestCategory("Regression")]
    public class LoginTests : BaseTest
    {
        // ── TC1: Register User ────────────────────────────────────────────────────
        // ⚠️ Commented out — requires account creation against a live shared site.
        // Covered by Phase 5 API tests (AccountLifecycleTests). Uncomment if a
        // dedicated sandbox or test credentials become available.
        //
        // [TestMethod]
        // [TestCategory("Smoke")]
        // [TestCategory("Critical")]
        // [Description("TC1: Register User")]
        // public async Task TC1_RegisterUser()
        // {
        //     // 1. Launch browser — handled by BaseTest
        //     // 2. Navigate to url 'http://automationexercise.com'
        //     Log.Step("Step 2 — Navigating to homepage");
        //     await Page.GotoAsync("/");
        //
        //     // 3. Verify that home page is visible successfully
        //     Log.Step("Step 3 — Verifying homepage is visible");
        //     var homePage = new HomePage(Page);
        //     Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
        //         "Expected homepage to be visible");
        //
        //     // 4. Click on 'Signup / Login' button
        //     Log.Step("Step 4 — Clicking Signup / Login button");
        //     var loginPage = await homePage.Nav.GoToLoginAsync();
        //
        //     // 5. Verify 'New User Signup!' is visible
        //     Log.Step("Step 5 — Verifying 'New User Signup!' is visible");
        //     Assert.IsTrue(await loginPage.IsSignupFormVisibleAsync(),
        //         "Expected 'New User Signup!' heading to be visible");
        //
        //     // 6. Enter name and email address
        //     Log.Step("Step 6 — Entering name and email address");
        //     // Use Bogus for unique email
        //     var signupPage = await loginPage.StartSignupAsync("Test User", "unique@email.com");
        //
        //     // 7. Click 'Signup' button — handled by StartSignupAsync
        //
        //     // 8. Verify that 'ENTER ACCOUNT INFORMATION' is visible
        //     Log.Step("Step 8 — Verifying 'ENTER ACCOUNT INFORMATION' is visible");
        //     var heading = await signupPage.GetHeadingTextAsync();
        //     Assert.AreEqual("ENTER ACCOUNT INFORMATION", heading);
        //
        //     // 9-12. Fill details and create account
        //     Log.Step("Steps 9-12 — Filling account details");
        //     // await signupPage.FillAccountDetailsAsync(details);
        //
        //     // 13. Click 'Create Account' button
        //     Log.Step("Step 13 — Clicking Create Account");
        //     // await signupPage.CreateAccountAsync();
        //
        //     // 14. Verify that 'ACCOUNT CREATED!' is visible
        //     Log.Step("Step 14 — Verifying 'ACCOUNT CREATED!' is visible");
        //     // Assert.IsTrue(await signupPage.IsAccountCreatedHeadingVisibleAsync());
        //
        //     // 15. Click 'Continue' button
        //     Log.Step("Step 15 — Clicking Continue");
        //     // await signupPage.ClickContinueAsync();
        //
        //     // 16. Verify that 'Logged in as username' is visible
        //     Log.Step("Step 16 — Verifying 'Logged in as username' is visible");
        //     // Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync());
        //
        //     // 17. Click 'Delete Account' button
        //     Log.Step("Step 17 — Clicking Delete Account");
        //     // await homePage.Nav.DeleteAccountAsync();
        //
        //     // 18. Verify that 'ACCOUNT DELETED!' is visible and click 'Continue' button
        //     Log.Step("Step 18 — Verifying 'ACCOUNT DELETED!' is visible");
        //     // Assert and continue
        // }

        // ── TC2: Login User with correct email and password ───────────────────────
        // ⚠️ Commented out — requires a pre-existing account on a live shared site.
        // Covered by Phase 5 API tests (AuthApiTests.VerifyLogin_WithValidCredentials).
        //
        // [TestMethod]
        // [TestCategory("Smoke")]
        // [TestCategory("Critical")]
        // [Description("TC2: Login User with correct email and password")]
        // public async Task TC2_LoginUser_WithCorrectCredentials()
        // {
        //     // 1. Launch browser — handled by BaseTest
        //     // 2. Navigate to url 'http://automationexercise.com'
        //     Log.Step("Step 2 — Navigating to homepage");
        //     await Page.GotoAsync("/");
        //
        //     // 3. Verify that home page is visible successfully
        //     Log.Step("Step 3 — Verifying homepage is visible");
        //     var homePage = new HomePage(Page);
        //     Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
        //         "Expected homepage to be visible");
        //
        //     // 4. Click on 'Signup / Login' button
        //     Log.Step("Step 4 — Clicking Signup / Login button");
        //     var loginPage = await homePage.Nav.GoToLoginAsync();
        //
        //     // 5. Verify 'Login to your account' is visible
        //     Log.Step("Step 5 — Verifying 'Login to your account' is visible");
        //     Assert.IsTrue(await loginPage.IsLoginFormVisibleAsync(),
        //         "Expected 'Login to your account' heading to be visible");
        //
        //     // 6. Enter correct email address and password
        //     Log.Step("Step 6 — Entering correct email and password");
        //     // 7. Click 'login' button
        //     Log.Step("Step 7 — Clicking login button");
        //     // await loginPage.LoginAsync(email, password);
        //
        //     // 8. Verify that 'Logged in as username' is visible
        //     Log.Step("Step 8 — Verifying 'Logged in as username' is visible");
        //     // Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync());
        //
        //     // 9. Click 'Delete Account' button
        //     Log.Step("Step 9 — Clicking Delete Account");
        //     // await homePage.Nav.DeleteAccountAsync();
        //
        //     // 10. Verify that 'ACCOUNT DELETED!' is visible
        //     Log.Step("Step 10 — Verifying 'ACCOUNT DELETED!' is visible");
        // }

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
        // ⚠️ Commented out — requires a pre-existing account on a live shared site.
        // Covered by Phase 5 API tests.
        //
        // [TestMethod]
        // [Description("TC4: Logout User")]
        // public async Task TC4_LogoutUser()
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
        //     // 4. Click on 'Signup / Login' button
        //     Log.Step("Step 4 — Clicking Signup / Login button");
        //     var loginPage = await homePage.Nav.GoToLoginAsync();
        //
        //     // 5. Verify 'Login to your account' is visible
        //     Log.Step("Step 5 — Verifying 'Login to your account' is visible");
        //     Assert.IsTrue(await loginPage.IsLoginFormVisibleAsync());
        //
        //     // 6. Enter correct email address and password
        //     Log.Step("Step 6 — Entering correct email and password");
        //     // 7. Click 'login' button
        //     Log.Step("Step 7 — Clicking login button");
        //     // await loginPage.LoginAsync(email, password);
        //
        //     // 8. Verify that 'Logged in as username' is visible
        //     Log.Step("Step 8 — Verifying 'Logged in as username' is visible");
        //     // Assert.IsTrue(await homePage.Nav.IsUserLoggedInAsync());
        //
        //     // 9. Click 'Logout' button
        //     Log.Step("Step 9 — Clicking Logout");
        //     // await homePage.Nav.LogoutAsync();
        //
        //     // 10. Verify that user is navigated to login page
        //     Log.Step("Step 10 — Verifying user is on login page");
        //     // Assert.IsTrue(await loginPage.IsLoginFormVisibleAsync());
        // }

        // ── TC5: Register User with existing email ────────────────────────────────
        // ⚠️ Commented out — requires a pre-existing account on a live shared site.
        // Covered by Phase 5 API tests.
        //
        // [TestMethod]
        // [Description("TC5: Register User with existing email")]
        // public async Task TC5_RegisterUser_WithExistingEmail()
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
        //     // 4. Click on 'Signup / Login' button
        //     Log.Step("Step 4 — Clicking Signup / Login button");
        //     var loginPage = await homePage.Nav.GoToLoginAsync();
        //
        //     // 5. Verify 'New User Signup!' is visible
        //     Log.Step("Step 5 — Verifying 'New User Signup!' is visible");
        //     Assert.IsTrue(await loginPage.IsSignupFormVisibleAsync());
        //
        //     // 6. Enter name and already registered email address
        //     Log.Step("Step 6 — Entering name and already registered email");
        //     // 7. Click 'Signup' button
        //     Log.Step("Step 7 — Clicking Signup button");
        //     // await loginPage.StartSignupAsync("Test", "existing@email.com");
        //
        //     // 8. Verify error 'Email Address already exist!' is visible
        //     Log.Step("Step 8 — Verifying error message is visible");
        //     // Assert.IsTrue(await loginPage.IsSignupErrorVisibleAsync());
        // }
    }
}