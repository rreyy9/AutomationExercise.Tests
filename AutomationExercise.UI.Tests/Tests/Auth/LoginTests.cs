namespace AutomationExercise.UI.Tests.Tests.Auth
{
    [TestClass]
    [TestCategory("Regression")]
    public class LoginTests : BaseTest
    {
        private LoginPage _loginPage = null!;

        [TestInitialize]
        public async Task Initialize()
        {
            _loginPage = new LoginPage(Page);
            await _loginPage.GoToAsync();
        }

        // ── Active tests (no account required) ───────────────────────────────────

        [TestMethod]
        public async Task Login_WithInvalidCredentials_ShowsErrorMessage()
        {
            Log.Step("Submitting login with invalid credentials");
            await _loginPage.LoginAsync("notareal@email.com", "wrongpassword");

            Log.Step("Asserting error message is visible");
            Assert.IsTrue(
                await _loginPage.IsLoginErrorVisibleAsync(),
                "Expected login error message to be visible after invalid credentials");
        }

        [TestMethod]
        public async Task LoginPage_BothForms_AreVisible()
        {
            Log.Step("Asserting login form is visible");
            Assert.IsTrue(
                await _loginPage.IsLoginFormVisibleAsync(),
                "Expected 'Login to your account' heading to be visible");

            Log.Step("Asserting signup form is visible");
            Assert.IsTrue(
                await _loginPage.IsSignupFormVisibleAsync(),
                "Expected 'New User Signup!' heading to be visible");
        }

        // ── Commented out — require account creation against a live shared site ──
        // These flows are fully covered by the Phase 5 API tests (createAccount,
        // verifyLogin, deleteAccount). Uncomment if a dedicated sandbox is available.
        //
        // [TestMethod]
        // [TestCategory("Smoke")]
        // public async Task Login_WithValidCredentials_ShowsLoggedInState()
        // {
        //     // Requires a pre-existing account — covered by API tests instead.
        //     // If reinstating: register in [TestInitialize], delete in [TestCleanup].
        // }
        //
        // [TestMethod]
        // public async Task Logout_AfterLogin_RedirectsToLoginPage()
        // {
        //     // Requires a pre-existing account — covered by API tests instead.
        // }
        //
        // [TestMethod]
        // public async Task Register_WithAlreadyRegisteredEmail_ShowsErrorMessage()
        // {
        //     // Requires a pre-existing account — covered by API tests instead.
        // }
        //
        // [TestMethod]
        // [TestCategory("Smoke")]
        // public async Task Register_NewUser_Successfully()
        // {
        //     // Creates an account on a live shared site — covered by API tests instead.
        //     // If reinstating: use Bogus for unique email, delete account in [TestCleanup].
        // }
    }
}