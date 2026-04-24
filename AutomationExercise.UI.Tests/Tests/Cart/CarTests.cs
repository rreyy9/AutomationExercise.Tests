namespace AutomationExercise.UI.Tests.Tests.Cart
{
    [TestClass]
    public class CartTests : BaseTest
    {
        private HomePage _homePage = null!;
        private ProductsPage _productsPage = null!;

        [TestInitialize]
        public async Task Initialize()
        {
            _homePage = new HomePage(Page);
            _productsPage = new ProductsPage(Page);
        }

        // ── Add from products page ────────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Smoke")]
        [TestCategory("Critical")]
        public async Task Cart_AddProductFromProductsPage_ItemAppearsInCart()
        {
            Log.Step("Navigating to products page");
            await _productsPage.GoToAsync();

            Log.Step("Adding first product to cart");
            await _productsPage.AddProductToCartAsync(0);

            Log.Step("Navigating to cart");
            var cartPage = await _homePage.Nav.GoToCartAsync();

            Log.Step("Asserting cart contains at least one item");
            var count = await cartPage.GetItemCountAsync();
            Assert.IsTrue(count > 0, "Expected at least one item in cart after adding from products page");
        }

        // ── Add from product detail page ──────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        public async Task Cart_AddProductFromDetailPage_ItemAppearsInCart()
        {
            Log.Step("Navigating to products page and opening first product detail");
            await _productsPage.GoToAsync();
            var detailPage = await _productsPage.GoToProductDetailAsync(0);

            Log.Step("Adding product to cart from detail page");
            await detailPage.AddToCartAsync();
            var cartPage = await detailPage.ViewCartAsync();

            Log.Step("Asserting cart contains at least one item");
            var count = await cartPage.GetItemCountAsync();
            Assert.IsTrue(count > 0, "Expected at least one item in cart after adding from detail page");
        }

        // ── Remove item ───────────────────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        public async Task Cart_RemoveProduct_CartBecomesEmpty()
        {
            Log.Step("Adding a product to the cart");
            await _productsPage.GoToAsync();
            await _productsPage.AddProductToCartAsync(0);

            Log.Step("Navigating to cart");
            var cartPage = await _homePage.Nav.GoToCartAsync();

            Log.Step("Removing the item");
            await cartPage.RemoveItemAsync(0);

            Log.Step("Asserting cart is now empty");
            Assert.IsTrue(
                await cartPage.IsCartEmptyAsync(),
                "Expected cart to be empty after removing the only item");
        }

        // ── Quantity ──────────────────────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        public async Task Cart_AddProductWithQuantity_QuantityIsCorrectInCart()
        {
            const int expectedQuantity = 3;

            Log.Step("Navigating to first product detail page");
            await _productsPage.GoToAsync();
            var detailPage = await _productsPage.GoToProductDetailAsync(0);

            Log.Step($"Setting quantity to {expectedQuantity}");
            await detailPage.SetQuantityAsync(expectedQuantity);

            Log.Step("Adding to cart and navigating to cart");
            await detailPage.AddToCartAsync();
            var cartPage = await detailPage.ViewCartAsync();

            Log.Step("Asserting quantity in cart matches");
            var actualQuantity = await cartPage.GetQuantityAsync(0);
            Assert.AreEqual(expectedQuantity, actualQuantity,
                $"Expected quantity {expectedQuantity} but found {actualQuantity} in cart");
        }

        // ── Price ─────────────────────────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        public async Task Cart_ProductPrice_IsNotEmpty()
        {
            Log.Step("Adding a product to the cart");
            await _productsPage.GoToAsync();
            await _productsPage.AddProductToCartAsync(0);

            Log.Step("Navigating to cart");
            var cartPage = await _homePage.Nav.GoToCartAsync();

            Log.Step("Asserting unit price is present");
            var price = await cartPage.GetUnitPriceAsync(0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(price),
                "Expected unit price to be present for item in cart");

            Log.Info($"Unit price: {price}");
        }

        // ── Multiple products ─────────────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        public async Task Cart_AddMultipleProducts_AllAppearInCart()
        {
            Log.Step("Navigating to products page and adding first product");
            await _productsPage.GoToAsync();
            await _productsPage.AddProductToCartAsync(0);

            // Continue shopping and add a second product
            // The modal appears after the first add — dismiss it then add the second
            Log.Step("Dismissing modal and adding second product");
            await Page.Locator("div#cartModal button.close-modal").ClickAsync();
            await _productsPage.AddProductToCartAsync(1);

            Log.Step("Navigating to cart");
            var cartPage = await _homePage.Nav.GoToCartAsync();

            Log.Step("Asserting cart has two items");
            var count = await cartPage.GetItemCountAsync();
            Assert.AreEqual(2, count, $"Expected 2 items in cart but found {count}");

            Log.Step("Logging product names in cart");
            var names = await cartPage.GetAllProductNamesAsync();
            Log.Info($"Cart contents: {string.Join(", ", names)}");
        }
    }
}