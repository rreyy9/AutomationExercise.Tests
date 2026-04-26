namespace AutomationExercise.UI.Tests.Tests.Cart
{
    [TestClass]
    public class CartTests : BaseTest
    {
        // ── TC12: Add Products in Cart ────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Smoke")]
        [TestCategory("Critical")]
        [Description("TC12: Add Products in Cart")]
        public async Task TC12_AddProductsInCart()
        {
            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Click 'Products' button
            Log.Step("Step 4 — Clicking Products button");
            var productsPage = await homePage.Nav.GoToProductsAsync();

            // 5. Hover over first product and click 'Add to cart'
            Log.Step("Step 5 — Hovering over first product and clicking 'Add to cart'");
            await productsPage.AddProductToCartAsync(0);

            // 6. Click 'Continue Shopping' button
            Log.Step("Step 6 — Clicking 'Continue Shopping'");
            // AddProductToCartAsync already clicks the close-modal/continue button

            // 7. Hover over second product and click 'Add to cart'
            Log.Step("Step 7 — Hovering over second product and clicking 'Add to cart'");
            await productsPage.AddProductToCartAsync(1);

            // 8. Click 'View Cart' button
            Log.Step("Step 8 — Navigating to cart");
            var cartPage = await homePage.Nav.GoToCartAsync();

            // 9. Verify both products are added to Cart
            Log.Step("Step 9 — Verifying both products are in cart");
            var count = await cartPage.GetItemCountAsync();
            Assert.AreEqual(2, count,
                $"Expected 2 items in cart but found {count}");

            // 10. Verify their prices, quantity and total price
            Log.Step("Step 10 — Verifying prices, quantity and total price");
            var names = await cartPage.GetAllProductNamesAsync();
            Log.Info($"Cart contents: {string.Join(", ", names)}");

            for (int i = 0; i < count; i++)
            {
                var price = await cartPage.GetUnitPriceAsync(i);
                Assert.IsFalse(string.IsNullOrWhiteSpace(price),
                    $"Expected unit price to be present for item at row {i}");

                var quantity = await cartPage.GetQuantityAsync(i);
                Assert.IsTrue(quantity > 0,
                    $"Expected quantity to be at least 1 for item at row {i}");

                var total = await cartPage.GetTotalPriceAsync(i);
                Assert.IsFalse(string.IsNullOrWhiteSpace(total),
                    $"Expected total price to be present for item at row {i}");

                Log.Info($"Row {i}: Name='{names[i]}' | Price='{price}' | Qty={quantity} | Total='{total}'");
            }
        }

        // ── TC13: Verify Product quantity in Cart ─────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        [Description("TC13: Verify Product quantity in Cart")]
        public async Task TC13_VerifyProductQuantityInCart()
        {
            const int expectedQuantity = 4;

            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Click 'View Product' for any product on home page
            Log.Step("Step 4 — Clicking 'View Product' on first product");
            var detailPage = await homePage.ClickViewProductAsync(0);

            // 5. Verify product detail is opened
            Log.Step("Step 5 — Verifying product detail page is opened");
            var productName = await detailPage.GetProductNameAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(productName),
                "Expected product name to be visible on detail page");
            Log.Info($"Product: '{productName}'");

            // 6. Increase quantity to 4
            Log.Step($"Step 6 — Setting quantity to {expectedQuantity}");
            await detailPage.SetQuantityAsync(expectedQuantity);

            // 7. Click 'Add to cart' button
            Log.Step("Step 7 — Clicking 'Add to cart'");
            await detailPage.AddToCartAsync();

            // 8. Click 'View Cart' button
            Log.Step("Step 8 — Clicking 'View Cart'");
            var cartPage = await detailPage.ViewCartAsync();

            // 9. Verify that product is displayed in cart page with exact quantity
            Log.Step("Step 9 — Verifying product quantity in cart");
            var actualQuantity = await cartPage.GetQuantityAsync(0);
            Assert.AreEqual(expectedQuantity, actualQuantity,
                $"Expected quantity {expectedQuantity} but found {actualQuantity} in cart");
            Log.Info($"Cart quantity verified: {actualQuantity}");
        }

        // ── TC17: Remove Products From Cart ───────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        [Description("TC17: Remove Products From Cart")]
        public async Task TC17_RemoveProductsFromCart()
        {
            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that home page is visible successfully
            Log.Step("Step 3 — Verifying homepage is visible");
            var homePage = new HomePage(Page);
            Assert.IsTrue(await homePage.IsHeroSliderVisibleAsync(),
                "Expected homepage to be visible");

            // 4. Add products to cart
            Log.Step("Step 4 — Adding a product to cart");
            var productsPage = new ProductsPage(Page);
            await productsPage.GoToAsync();
            await productsPage.AddProductToCartAsync(0);

            // 5. Click 'Cart' button
            Log.Step("Step 5 — Clicking Cart button");
            var cartPage = await homePage.Nav.GoToCartAsync();

            // 6. Verify that cart page is displayed
            Log.Step("Step 6 — Verifying cart page is displayed");
            Assert.IsTrue(await cartPage.IsCartTableVisibleAsync(),
                "Expected cart table to be visible");

            // 7. Click 'X' button corresponding to particular product
            Log.Step("Step 7 — Clicking 'X' button to remove the product");
            await cartPage.RemoveItemAsync(0);

            // 8. Verify that product is removed from the cart
            Log.Step("Step 8 — Verifying product is removed from cart");
            Assert.IsTrue(await cartPage.IsCartEmptyAsync(),
                "Expected cart to be empty after removing the only item");
        }
    }
}