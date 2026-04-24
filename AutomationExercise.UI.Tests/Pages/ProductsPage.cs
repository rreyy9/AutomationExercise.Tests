namespace AutomationExercise.UI.Tests.Pages
{
    public class ProductsPage(IPage page)
    {
        public NavBar Nav { get; } = new NavBar(page);

        // ── Locators ─────────────────────────────────────────────────────────────
        private readonly ILocator _pageHeading = page.Locator("h2.title:has-text('ALL PRODUCTS')");
        private readonly ILocator _searchedHeading = page.Locator("h2.title:has-text('SEARCHED PRODUCTS')");

        // Search
        // ✅ Confirmed from real Playwright test code targeting this site
        private readonly ILocator _searchInput = page.Locator("#search_product");
        private readonly ILocator _searchButton = page.Locator("#submit_search");

        // Product grid
        private readonly ILocator _productCards = page.Locator("div.productinfo");
        private readonly ILocator _viewProductLinks = page.Locator("a[href*='/product_details/']");

        // Category sidebar (shared with HomePage)
        private readonly ILocator _womenCategory = page.Locator("a[href='#Women']");
        private readonly ILocator _menCategory = page.Locator("a[href='#Men']");
        private readonly ILocator _kidsCategory = page.Locator("a[href='#Kids']");

        // Category sub-links
        private readonly ILocator _womenDress = page.Locator("a[href='/category_products/1']");
        private readonly ILocator _womenTops = page.Locator("a[href='/category_products/2']");
        private readonly ILocator _womenSaree = page.Locator("a[href='/category_products/7']");
        private readonly ILocator _menTshirts = page.Locator("a[href='/category_products/3']");
        private readonly ILocator _menJeans = page.Locator("a[href='/category_products/6']");
        private readonly ILocator _kidsDress = page.Locator("a[href='/category_products/4']");
        private readonly ILocator _kidsTopsShirts = page.Locator("a[href='/category_products/5']");

        // Brand sidebar
        private readonly ILocator _brandLinks = page.Locator("div.brands-name a");

        // ── Navigation ────────────────────────────────────────────────────────────
        public async Task GoToAsync()
            => await page.GotoAsync("/products");

        // ── Search ────────────────────────────────────────────────────────────────
        public async Task SearchAsync(string term)
        {
            await _searchInput.FillAsync(term);
            await _searchButton.ClickAsync();
        }

        // ── Product grid ──────────────────────────────────────────────────────────
        public async Task<int> GetProductCountAsync()
            => await _productCards.CountAsync();

        public async Task<IReadOnlyList<string>> GetProductNamesAsync()
        {
            var names = new List<string>();
            var count = await _productCards.CountAsync();
            for (int i = 0; i < count; i++)
                names.Add(await _productCards.Nth(i).Locator("p").InnerTextAsync());
            return names;
        }

        public async Task<ProductDetailPage> GoToProductDetailAsync(int index = 0)
        {
            await _viewProductLinks.Nth(index).ClickAsync();
            return new ProductDetailPage(page);
        }

        public async Task AddProductToCartAsync(int index = 0)
        {
            var card = _productCards.Nth(index);
            await card.HoverAsync();
            await card.Locator("a.add-to-cart").First.ClickAsync();
        }

        // ── Category filters ──────────────────────────────────────────────────────
        public async Task FilterByCategoryAsync(string category, string subcategory)
        {
            ILocator catLink = category.ToLower() switch
            {
                "women" => _womenCategory,
                "men" => _menCategory,
                "kids" => _kidsCategory,
                _ => throw new ArgumentException($"Unknown category: {category}")
            };
            await catLink.ClickAsync();

            // Map subcategory label to its href locator
            string subKey = $"{category.ToLower()}_{subcategory.ToLower().Replace(" ", "_")}";
            ILocator subLink = subKey switch
            {
                "women_dress" => _womenDress,
                "women_tops" => _womenTops,
                "women_saree" => _womenSaree,
                "men_tshirts" => _menTshirts,
                "men_jeans" => _menJeans,
                "kids_dress" => _kidsDress,
                "kids_tops_&_shirts" => _kidsTopsShirts,
                "kids_tops_shirts" => _kidsTopsShirts,
                _ => throw new ArgumentException($"Unknown subcategory: {subcategory}")
            };
            await subLink.ClickAsync();
        }

        // ── Brand filters ──────────────────────────────────────────────────────────
        public async Task FilterByBrandAsync(string brand)
        {
            var link = page.Locator($"div.brands-name a[href='/brand_products/{brand}']");
            await link.ClickAsync();
        }

        public async Task<IReadOnlyList<string>> GetBrandNamesAsync()
        {
            var brands = new List<string>();
            var count = await _brandLinks.CountAsync();
            for (int i = 0; i < count; i++)
                brands.Add(await _brandLinks.Nth(i).InnerTextAsync());
            return brands;
        }

        // ── State queries ─────────────────────────────────────────────────────────
        public async Task<bool> IsOnAllProductsPageAsync()
            => await _pageHeading.IsVisibleAsync();

        public async Task<bool> IsSearchedProductsHeadingVisibleAsync()
            => await _searchedHeading.IsVisibleAsync();
    }
}