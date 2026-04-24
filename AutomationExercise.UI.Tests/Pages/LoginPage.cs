namespace AutomationExercise.UI.Tests.Pages
{
    public class LoginPage(IPage page)
    {
        public NavBar Nav { get; } = new NavBar(page);

        // ── Locators — Login form ────────────────────────────────────────────────
        // ✅ This page uses data-qa attributes — always prefer these over IDs/classes
        private readonly ILocator _loginHeading = page.Locator("h2:has-text('Login to your account')");
        private readonly ILocator _loginEmail = page.Locator("input[data-qa='login-email']");
        private readonly ILocator _loginPassword = page.Locator("input[data-qa='login-password']");
        private readonly ILocator _loginButton = page.Locator("button[data-qa='login-button']");
        private readonly ILocator _loginError = page.Locator("p:has-text('Your email or password is incorrect!')");

        // ── Locators — Signup form (same URL, second panel) ─────────────────────
        private readonly ILocator _signupHeading = page.Locator("h2:has-text('New User Signup!')");
        private readonly ILocator _signupName = page.Locator("input[data-qa='signup-name']");
        private readonly ILocator _signupEmail = page.Locator("input[data-qa='signup-email']");
        private readonly ILocator _signupButton = page.Locator("button[data-qa='signup-button']");
        private readonly ILocator _signupError = page.Locator("p:has-text('Email Address already exist!')");

        // ── Navigation ──────────────────────────────────────────────────────────
        public async Task GoToAsync()
            => await page.GotoAsync("/login");

        // ── Login actions ────────────────────────────────────────────────────────
        public async Task LoginAsync(string email, string password)
        {
            await _loginEmail.FillAsync(email);
            await _loginPassword.FillAsync(password);
            await _loginButton.ClickAsync();
        }

        public async Task<string> GetLoginErrorMessageAsync()
            => await _loginError.InnerTextAsync();

        public async Task<bool> IsLoginErrorVisibleAsync()
            => await _loginError.IsVisibleAsync();

        // ── Signup actions ───────────────────────────────────────────────────────
        /// <summary>Fills the signup panel and clicks Signup → returns the SignupPage.</summary>
        public async Task<SignupPage> StartSignupAsync(string name, string email)
        {
            await _signupName.FillAsync(name);
            await _signupEmail.FillAsync(email);
            await _signupButton.ClickAsync();
            return new SignupPage(page);
        }

        public async Task<string> GetSignupErrorMessageAsync()
            => await _signupError.InnerTextAsync();

        public async Task<bool> IsSignupErrorVisibleAsync()
            => await _signupError.IsVisibleAsync();

        // ── State queries ────────────────────────────────────────────────────────
        public async Task<bool> IsLoginFormVisibleAsync()
            => await _loginHeading.IsVisibleAsync();

        public async Task<bool> IsSignupFormVisibleAsync()
            => await _signupHeading.IsVisibleAsync();
    }
}