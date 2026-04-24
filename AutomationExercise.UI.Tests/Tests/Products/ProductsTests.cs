namespace AutomationExercise.UI.Tests.Tests.Products
{
    [TestClass]
    public class ProductsTests : BaseTest
    {
        private ProductsPage _productsPage = null!;

        [TestInitialize]
        public async Task Initialize()
        {
            _productsPage = new ProductsPage(Page);
            await _productsPage.GoToAsync();
        }

        // ── Navigate to all products page ─────────────────────────────────────────

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task ProductsPage_AllProductsHeading_IsVisible()
        {
            Log.Step("Asserting 'ALL PRODUCTS' heading is visible");
            Assert.IsTrue(
                await _productsPage.IsOnAllProductsPageAsync(),
                "Expected 'ALL PRODUCTS' heading to be visible on /products");
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task ProductsPage_ProductGrid_IsNotEmpty()
        {
            Log.Step("Counting products in the grid");
            var count = await _productsPage.GetProductCountAsync();

            Log.Info($"Product count: {count}");
            Assert.IsTrue(count > 0, "Expected at least one product to be listed on the products page");
        }

        // ── View product detail page ──────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        public async Task ProductDetail_FirstProduct_HasNameAndPrice()
        {
            Log.Step("Navigating to first product detail page");
            var detailPage = await _productsPage.GoToProductDetailAsync(0);

            Log.Step("Asserting product name is not empty");
            var name = await detailPage.GetProductNameAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(name), "Expected product name to be present on detail page");

            Log.Step("Asserting product price is not empty");
            var price = await detailPage.GetPriceAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(price), "Expected product price to be present on detail page");

            Log.Info($"Product: '{name}' | Price: '{price}'");
        }

        [TestMethod]
        [TestCategory("Regression")]
        public async Task ProductDetail_FirstProduct_HasCategoryAndBrand()
        {
            Log.Step("Navigating to first product detail page");
            var detailPage = await _productsPage.GoToProductDetailAsync(0);

            Log.Step("Asserting category is present");
            var category = await detailPage.GetCategoryAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(category), "Expected category to be present on detail page");

            Log.Step("Asserting brand is present");
            var brand = await detailPage.GetBrandAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(brand), "Expected brand to be present on detail page");

            Log.Info($"Category: '{category}' | Brand: '{brand}'");
        }

        // ── Search ────────────────────────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        public async Task Search_WithKnownTerm_ReturnsResults()
        {
            Log.Step("Searching for 'dress'");
            await _productsPage.SearchAsync("dress");

            Log.Step("Asserting 'SEARCHED PRODUCTS' heading is visible");
            Assert.IsTrue(
                await _productsPage.IsSearchedProductsHeadingVisibleAsync(),
                "Expected 'SEARCHED PRODUCTS' heading to appear after search");

            Log.Step("Asserting search returned at least one result");
            var count = await _productsPage.GetProductCountAsync();
            Assert.IsTrue(count > 0, "Expected at least one product in search results for 'dress'");

            Log.Info($"Search results count: {count}");
        }

        [TestMethod]
        [TestCategory("Regression")]
        public async Task Search_ResultNames_ContainSearchTerm()
        {
            const string searchTerm = "top";

            Log.Step($"Searching for '{searchTerm}'");
            await _productsPage.SearchAsync(searchTerm);

            Log.Step("Retrieving result product names");
            var names = await _productsPage.GetProductNamesAsync();

            Assert.IsTrue(names.Count > 0, $"Expected search results for '{searchTerm}'");

            Log.Step("Asserting at least one result name contains the search term");
            var anyMatch = names.Any(n => n.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(anyMatch,
                $"Expected at least one result to contain '{searchTerm}' in its name. Found: {string.Join(", ", names)}");
        }

        // ── Category filter ───────────────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        public async Task CategoryFilter_Women_Dress_ShowsProducts()
        {
            Log.Step("Filtering by Women > Dress");
            await _productsPage.FilterByCategoryAsync("Women", "Dress");

            Log.Step("Asserting at least one product is shown");
            var count = await _productsPage.GetProductCountAsync();
            Assert.IsTrue(count > 0, "Expected products to appear after filtering by Women > Dress");

            Log.Info($"Products shown after Women > Dress filter: {count}");
        }

        [TestMethod]
        [TestCategory("Regression")]
        public async Task CategoryFilter_Men_Tshirts_ShowsProducts()
        {
            Log.Step("Filtering by Men > Tshirts");
            await _productsPage.FilterByCategoryAsync("Men", "Tshirts");

            Log.Step("Asserting at least one product is shown");
            var count = await _productsPage.GetProductCountAsync();
            Assert.IsTrue(count > 0, "Expected products to appear after filtering by Men > Tshirts");

            Log.Info($"Products shown after Men > Tshirts filter: {count}");
        }

        // ── Brand filter ──────────────────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        public async Task BrandFilter_Sidebar_IsNotEmpty()
        {
            Log.Step("Retrieving brand names from the sidebar");
            var brands = await _productsPage.GetBrandNamesAsync();

            Assert.IsTrue(brands.Count > 0, "Expected at least one brand to be listed in the sidebar");
            Log.Info($"Brands found: {string.Join(", ", brands)}");
        }

        [TestMethod]
        [TestCategory("Regression")]
        public async Task BrandFilter_Polo_ShowsProducts()
        {
            Log.Step("Filtering by brand: Polo");
            await _productsPage.FilterByBrandAsync("Polo");

            Log.Step("Asserting at least one product is shown");
            var count = await _productsPage.GetProductCountAsync();
            Assert.IsTrue(count > 0, "Expected products to appear after filtering by Polo brand");

            Log.Info($"Products shown for Polo brand: {count}");
        }
    }
}