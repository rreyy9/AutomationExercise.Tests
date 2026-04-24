namespace AutomationExercise.UI.Tests.Pages
{
    public class HomePage(IPage page)
    {
        public NavBar Nav { get; } = new NavBar(page);

        // ── Locators ────────────────────────────────────────────────────────────
        // Hero / slider
        private readonly ILocator _heroSlider = page.Locator("div#slider");

        // Subscription (footer)
        private readonly ILocator _subscriptionHeading = page.Locator("h2:has-text('Subscription')");
        // ⚠️  Three s's in 'susbscibeemail' — this is the actual ID on the site
        private readonly ILocator _subscribeEmail = page.Locator("#susbscibeemail");
        private readonly ILocator _subscribeButton = page.Locator("#subscribe");
        private readonly ILocator _subscribeSuccess = page.Locator("div#success-subscribe");

        // Feature products grid
        private readonly ILocator _featureItems = page.Locator("div.features_items");
        private readonly ILocator _productCards = page.Locator("div.features_items .col-sm-4");
        private readonly ILocator _addToCartOverlay = page.Locator("div.features_items .product-overlay .add-to-cart");

        // "Add to cart" modal
        private readonly ILocator _cartModal = page.Locator("div#cartModal");
        private readonly ILocator _modalContinue = page.Locator("div#cartModal button.close-modal");
        private readonly ILocator _modalViewCart = page.Locator("div#cartModal a[href='/view_cart']");

        // Recommended items carousel
        private readonly ILocator _recommendedSection = page.Locator("div#recommended-item-carousel");

        // ── Navigation ──────────────────────────────────────────────────────────
        public async Task GoToAsync()
            => await page.GotoAsync("/");

        // ── Subscription ────────────────────────────────────────────────────────
        public async Task SubscribeAsync(string email)
        {
            await _subscribeEmail.ScrollIntoViewIfNeededAsync();
            await _subscribeEmail.FillAsync(email);
            await _subscribeButton.ClickAsync();
        }

        public async Task<string> GetSubscriptionSuccessMessageAsync()
            => await _subscribeSuccess.InnerTextAsync();

        public async Task<bool> IsSubscriptionSuccessVisibleAsync()
            => await _subscribeSuccess.IsVisibleAsync();

        // ── Products grid ───────────────────────────────────────────────────────
        public async Task<int> GetFeatureProductCountAsync()
            => await _productCards.CountAsync();

        /// <summary>Adds a product to cart by 0-based index in the featured grid.</summary>
        public async Task AddProductToCartAsync(int index = 0)
        {
            var card = _productCards.Nth(index);
            await card.HoverAsync();
            await card.Locator("a.add-to-cart").ClickAsync();
        }

        public async Task<ProductDetailPage> ClickViewProductAsync(int index = 0)
        {
            await page.Locator("div.features_items a[href*='/product_details/']")
                      .Nth(index)
                      .ClickAsync();
            return new ProductDetailPage(page);
        }

        // ── Modal helpers ────────────────────────────────────────────────────────
        public async Task ContinueShoppingAsync()
            => await _modalContinue.ClickAsync();

        public async Task<CartPage> ViewCartFromModalAsync()
        {
            await _modalViewCart.ClickAsync();
            return new CartPage(page);
        }

        // ── State queries ────────────────────────────────────────────────────────
        public async Task<bool> IsHeroSliderVisibleAsync()
            => await _heroSlider.IsVisibleAsync();

        public async Task<bool> IsCartModalVisibleAsync()
            => await _cartModal.IsVisibleAsync();
    }
}