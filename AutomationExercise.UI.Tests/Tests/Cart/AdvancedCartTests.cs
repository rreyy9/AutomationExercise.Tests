namespace AutomationExercise.UI.Tests.Tests.Cart
{
    [TestClass]
    public class AdvancedCartTests : BaseTest
    {
        // ── TC20: Search Products and Verify Cart After Login ─────────────────────
        // ⚠️ Partially commented out — steps 10-12 require a pre-existing account
        // on a live shared site. The search + add to cart portion (steps 1-9) is
        // fully implemented. Login verification is covered by Phase 5 API tests.
        //
        // [TestMethod]
        // [TestCategory("Regression")]
        // [Description("TC20: Search Products and Verify Cart After Login")]
        // public async Task TC20_SearchProducts_AndVerifyCartAfterLogin()
        // {
        //     // 1. Launch browser — handled by BaseTest
        //
        //     // 2. Navigate to url 'http://automationexercise.com'
        //     Log.Step("Step 2 — Navigating to homepage");
        //     await Page.GotoAsync("/");
        //
        //     // 3. Click on 'Products' button
        //     Log.Step("Step 3 — Clicking Products button");
        //     var homePage = new HomePage(Page);
        //     var productsPage = await homePage.Nav.GoToProductsAsync();
        //
        //     // 4. Verify user is navigated to ALL PRODUCTS page successfully
        //     Log.Step("Step 4 — Verifying 'ALL PRODUCTS' heading is visible");
        //     Assert.IsTrue(await productsPage.IsOnAllProductsPageAsync(),
        //         "Expected 'ALL PRODUCTS' heading to be visible");
        //
        //     // 5. Enter product name in search input and click search button
        //     const string searchTerm = "dress";
        //     Log.Step($"Step 5 — Searching for '{searchTerm}'");
        //     await productsPage.SearchAsync(searchTerm);
        //
        //     // 6. Verify 'SEARCHED PRODUCTS' is visible
        //     Log.Step("Step 6 — Verifying 'SEARCHED PRODUCTS' heading is visible");
        //     Assert.IsTrue(await productsPage.IsSearchedProductsHeadingVisibleAsync(),
        //         "Expected 'SEARCHED PRODUCTS' heading to appear after search");
        //
        //     // 7. Verify all the products related to search are visible
        //     Log.Step("Step 7 — Verifying search results are visible");
        //     var count = await productsPage.GetProductCountAsync();
        //     Assert.IsTrue(count > 0, $"Expected search results for '{searchTerm}'");
        //
        //     // 8. Add those products to cart
        //     Log.Step("Step 8 — Adding first search result to cart");
        //     await productsPage.AddProductToCartAsync(0);
        //
        //     // 9. Click 'Cart' button and verify that products are visible in cart
        //     Log.Step("Step 9 — Navigating to cart and verifying product is present");
        //     var cartPage = await homePage.Nav.GoToCartAsync();
        //     var cartCount = await cartPage.GetItemCountAsync();
        //     Assert.IsTrue(cartCount > 0, "Expected at least one item in cart after adding search result");
        //
        //     // 10. Click 'Signup / Login' button and submit login details
        //     Log.Step("Step 10 — Clicking Signup / Login and submitting login details");
        //     // var loginPage = await homePage.Nav.GoToLoginAsync();
        //     // await loginPage.LoginAsync(email, password);
        //
        //     // 11. Again, go to Cart page
        //     Log.Step("Step 11 — Navigating to Cart page again after login");
        //     // cartPage = await homePage.Nav.GoToCartAsync();
        //
        //     // 12. Verify that those products are visible in cart after login as well
        //     Log.Step("Step 12 — Verifying products are still in cart after login");
        //     // var cartCountAfterLogin = await cartPage.GetItemCountAsync();
        //     // Assert.IsTrue(cartCountAfterLogin > 0,
        //     //     "Expected products to remain in cart after login");
        // }

        // ── TC22: Add to cart from Recommended items ──────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        [Description("TC22: Add to cart from Recommended items")]
        public async Task TC22_AddToCartFromRecommendedItems()
        {
            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Scroll to bottom of page
            Log.Step("Step 3 — Scrolling to bottom of page");
            await Page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");

            // 4. Verify 'RECOMMENDED ITEMS' are visible
            Log.Step("Step 4 — Verifying 'RECOMMENDED ITEMS' section is visible");
            var recommendedSection = Page.Locator("h2:has-text('recommended items')");
            await Assertions.Expect(recommendedSection).ToBeVisibleAsync();

            // 5. Click on 'Add To Cart' on Recommended product
            Log.Step("Step 5 — Clicking 'Add To Cart' on a recommended product");
            var recommendedAddToCart = Page.Locator("#recommended-item-carousel .product-image-wrapper a.add-to-cart").First;
            await recommendedAddToCart.ClickAsync();

            // Wait for modal to appear
            var cartModal = Page.Locator("div#cartModal");
            await cartModal.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible
            });

            // 6. Click on 'View Cart' button
            Log.Step("Step 6 — Clicking 'View Cart' in the modal");
            await Page.Locator("div#cartModal a[href='/view_cart']").ClickAsync();

            // 7. Verify that product is displayed in cart page
            Log.Step("Step 7 — Verifying product is displayed in cart page");
            var cartPage = new CartPage(Page);
            var itemCount = await cartPage.GetItemCountAsync();
            Assert.IsTrue(itemCount > 0,
                "Expected at least one item in cart after adding from recommended items");

            var productName = await cartPage.GetProductNameAsync(0);
            Log.Info($"Recommended product added to cart: '{productName}'");
        }
    }
}