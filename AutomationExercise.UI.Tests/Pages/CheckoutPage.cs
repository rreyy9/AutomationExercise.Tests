namespace AutomationExercise.UI.Tests.Pages
{
    public class CheckoutPage(IPage page)
    {
        public NavBar Nav { get; } = new NavBar(page);

        // ── Locators ──────────────────────────────────────────────────────────────
        private readonly ILocator _deliveryAddress = page.Locator("#address_delivery");
        private readonly ILocator _billingAddress = page.Locator("#address_invoice");
        private readonly ILocator _orderItemsTable = page.Locator("table#cart_info_table");
        private readonly ILocator _commentBox = page.Locator("textarea.form-control");
        private readonly ILocator _placeOrderBtn = page.Locator("a.btn:has-text('Place Order')");

        // Per-row helpers (reused from CartPage pattern)
        private ILocator CartRow(int index) =>
            page.Locator("table#cart_info_table tbody tr").Nth(index);

        // ── Address getters ───────────────────────────────────────────────────────
        public async Task<string> GetDeliveryAddressAsync()
            => await _deliveryAddress.InnerTextAsync();

        public async Task<string> GetBillingAddressAsync()
            => await _billingAddress.InnerTextAsync();

        // ── Order review ──────────────────────────────────────────────────────────
        public async Task<int> GetOrderItemCountAsync()
            => await page.Locator("table#cart_info_table tbody tr").CountAsync();

        public async Task<string> GetOrderItemNameAsync(int rowIndex = 0)
            => await CartRow(rowIndex).Locator("td.cart_description h4 a").InnerTextAsync();

        public async Task<string> GetOrderItemPriceAsync(int rowIndex = 0)
            => await CartRow(rowIndex).Locator("td.cart_price p").InnerTextAsync();

        // ── Order placement ───────────────────────────────────────────────────────
        public async Task AddCommentAsync(string comment)
            => await _commentBox.FillAsync(comment);

        public async Task<PaymentPage> PlaceOrderAsync()
        {
            await _placeOrderBtn.ClickAsync();
            return new PaymentPage(page);
        }

        // ── State queries ─────────────────────────────────────────────────────────
        public async Task<bool> IsOrderTableVisibleAsync()
            => await _orderItemsTable.IsVisibleAsync();
    }
}