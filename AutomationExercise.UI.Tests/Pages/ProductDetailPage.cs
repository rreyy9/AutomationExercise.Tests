namespace AutomationExercise.UI.Tests.Pages
{
    public class ProductDetailPage(IPage page)
    {
        public NavBar Nav { get; } = new NavBar(page);

        // ── Locators — Product info ───────────────────────────────────────────────
        private readonly ILocator _productName = page.Locator("div.product-information h2");
        private readonly ILocator _category = page.Locator("div.product-information p:has-text('Category')");
        private readonly ILocator _price = page.Locator("div.product-information span span");
        private readonly ILocator _availability = page.Locator("div.product-information p:has-text('Availability')");
        private readonly ILocator _condition = page.Locator("div.product-information p:has-text('Condition')");
        private readonly ILocator _brand = page.Locator("div.product-information p:has-text('Brand')");

        // ── Locators — Cart interaction ───────────────────────────────────────────
        private readonly ILocator _quantityInput = page.Locator("input#quantity");
        private readonly ILocator _addToCartBtn = page.Locator("button.cart");

        // ── Locators — Modal (shared pattern) ────────────────────────────────────
        private readonly ILocator _cartModal = page.Locator("div#cartModal");
        private readonly ILocator _modalContinue = page.Locator("div#cartModal button.close-modal");
        private readonly ILocator _modalViewCart = page.Locator("div#cartModal a[href='/view_cart']");

        // ── Locators — Review section ─────────────────────────────────────────────
        private readonly ILocator _reviewName = page.Locator("input#name");
        private readonly ILocator _reviewEmail = page.Locator("input#email");
        private readonly ILocator _reviewText = page.Locator("textarea#review");
        private readonly ILocator _reviewSubmit = page.Locator("button#button-review");

        // ── Product info getters ──────────────────────────────────────────────────
        public async Task<string> GetProductNameAsync()
            => await _productName.InnerTextAsync();

        public async Task<string> GetPriceAsync()
            => await _price.InnerTextAsync();

        public async Task<string> GetCategoryAsync()
            => await _category.InnerTextAsync();

        public async Task<string> GetAvailabilityAsync()
            => await _availability.InnerTextAsync();

        public async Task<string> GetConditionAsync()
            => await _condition.InnerTextAsync();

        public async Task<string> GetBrandAsync()
            => await _brand.InnerTextAsync();

        // ── Cart actions ──────────────────────────────────────────────────────────
        public async Task SetQuantityAsync(int quantity)
        {
            await _quantityInput.ClearAsync();
            await _quantityInput.FillAsync(quantity.ToString());
        }

        public async Task AddToCartAsync()
        {
            await _addToCartBtn.ClickAsync();
            await _cartModal.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible
            });
        }

        public async Task ContinueShoppingAsync()
            => await _modalContinue.ClickAsync();

        public async Task<CartPage> ViewCartAsync()
        {
            await _modalViewCart.ClickAsync();
            return new CartPage(page);
        }

        // ── Review actions ────────────────────────────────────────────────────────
        public async Task SubmitReviewAsync(string name, string email, string review)
        {
            await _reviewName.FillAsync(name);
            await _reviewEmail.FillAsync(email);
            await _reviewText.FillAsync(review);
            await _reviewSubmit.ClickAsync();
        }

        // ── State queries ─────────────────────────────────────────────────────────
        public async Task<bool> IsCartModalVisibleAsync()
            => await _cartModal.IsVisibleAsync();
    }
}