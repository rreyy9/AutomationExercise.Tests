namespace AutomationExercise.UI.Tests.Pages
{
    public class CartPage(IPage page)
    {
        public NavBar Nav { get; } = new NavBar(page);

        // ── Locators ──────────────────────────────────────────────────────────────
        private readonly ILocator _cartTable = page.Locator("table#cart_info_table");
        private readonly ILocator _cartRows = page.Locator("table#cart_info_table tbody tr");
        private readonly ILocator _proceedToCheckout = page.Locator("a.btn:has-text('Proceed To Checkout')");
        private readonly ILocator _registerLoginLink = page.Locator("u:has-text('Register / Login')");

        // ── Navigation ────────────────────────────────────────────────────────────
        public async Task GoToAsync()
            => await page.GotoAsync("/view_cart");

        // ── Per-row helpers ───────────────────────────────────────────────────────
        private ILocator Row(int index) => _cartRows.Nth(index);

        public async Task<string> GetProductNameAsync(int rowIndex = 0)
            => await Row(rowIndex).Locator("td.cart_description h4 a").InnerTextAsync();

        public async Task<string> GetUnitPriceAsync(int rowIndex = 0)
            => await Row(rowIndex).Locator("td.cart_price p").InnerTextAsync();

        public async Task<int> GetQuantityAsync(int rowIndex = 0)
        {
            var text = await Row(rowIndex).Locator("td.cart_quantity button").InnerTextAsync();
            return int.TryParse(text.Trim(), out int qty) ? qty : 0;
        }

        public async Task<string> GetTotalPriceAsync(int rowIndex = 0)
            => await Row(rowIndex).Locator("td.cart_total p").InnerTextAsync();

        public async Task RemoveItemAsync(int rowIndex = 0)
        {
            var row = Row(rowIndex);
            await row.Locator("td.cart_delete a.cart_quantity_delete").ClickAsync();
            await row.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Detached });
        }

        // ── Bulk helpers ──────────────────────────────────────────────────────────
        public async Task<int> GetItemCountAsync()
            => await _cartRows.CountAsync();

        public async Task<IReadOnlyList<string>> GetAllProductNamesAsync()
        {
            var names = new List<string>();
            var count = await _cartRows.CountAsync();
            for (int i = 0; i < count; i++)
                names.Add(await GetProductNameAsync(i));
            return names;
        }

        // ── Checkout ──────────────────────────────────────────────────────────────
        public async Task<CheckoutPage> ProceedToCheckoutAsync()
        {
            await _proceedToCheckout.ClickAsync();
            return new CheckoutPage(page);
        }

        /// <summary>
        /// For guest users: clicking Proceed shows a modal with a Register/Login link.
        /// This clicks that link and returns the LoginPage.
        /// </summary>
        public async Task<LoginPage> GoToLoginFromCheckoutPromptAsync()
        {
            await _proceedToCheckout.ClickAsync();
            await _registerLoginLink.ClickAsync();
            return new LoginPage(page);
        }

        // ── State queries ─────────────────────────────────────────────────────────
        public async Task<bool> IsCartTableVisibleAsync()
            => await _cartTable.IsVisibleAsync();

        public async Task<bool> IsCartEmptyAsync()
            => await _cartRows.CountAsync() == 0;
    }
}