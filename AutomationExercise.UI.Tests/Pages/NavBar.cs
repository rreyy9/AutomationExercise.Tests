namespace AutomationExercise.UI.Tests.Pages
{
    public class NavBar(IPage page)
    {
        // ── Locators ────────────────────────────────────────────────────────────
        private readonly ILocator _logo = page.Locator("img[src*='logo']");
        private readonly ILocator _homeLink = page.Locator("a[href='/']").First;
        private readonly ILocator _productsLink = page.Locator("a[href='/products']");
        private readonly ILocator _cartLink = page.Locator("#header a[href='/view_cart']");
        private readonly ILocator _loginLink = page.Locator("a[href='/login']");
        private readonly ILocator _logoutLink = page.Locator("a[href='/logout']");
        private readonly ILocator _deleteAcctLink = page.Locator("a[href='/delete_account']");
        private readonly ILocator _contactUsLink = page.Locator("a[href='/contact_us']");
        private readonly ILocator _testCasesLink = page.Locator("a[href='/test_cases']");
        private readonly ILocator _loggedInAs = page.Locator("a:has-text('Logged in as') b");

        // ── Navigation ──────────────────────────────────────────────────────────
        public async Task<HomePage> GoToHomeAsync()
        {
            await _homeLink.ClickAsync();
            return new HomePage(page);
        }

        public async Task<ProductsPage> GoToProductsAsync()
        {
            await _productsLink.ClickAsync();
            return new ProductsPage(page);
        }

        public async Task<CartPage> GoToCartAsync()
        {
            await _cartLink.ClickAsync();
            return new CartPage(page);
        }

        public async Task<LoginPage> GoToLoginAsync()
        {
            await _loginLink.ClickAsync();
            return new LoginPage(page);
        }

        public async Task<ContactUsPage> GoToContactUsAsync()
        {
            await _contactUsLink.ClickAsync();
            return new ContactUsPage(page);
        }

        public async Task<HomePage> LogoutAsync()
        {
            await _logoutLink.ClickAsync();
            return new HomePage(page);
        }

        public async Task DeleteAccountAsync()
        {
            await _deleteAcctLink.ClickAsync();
        }

        // ── State queries ───────────────────────────────────────────────────────
        public async Task<string> GetLoggedInUsernameAsync()
            => await _loggedInAs.InnerTextAsync();

        public async Task<bool> IsUserLoggedInAsync()
            => await _logoutLink.IsVisibleAsync();

        public async Task<bool> IsLogoVisibleAsync()
            => await _logo.IsVisibleAsync();
    }
}