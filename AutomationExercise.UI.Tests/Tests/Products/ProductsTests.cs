namespace AutomationExercise.UI.Tests.Tests.Products
{
    [TestClass]
    public class ProductsTests : BaseTest
    {
        // ── TC8: Verify All Products and product detail page ──────────────────────

        [TestMethod]
        [TestCategory("Smoke")]
        [Description("TC8: Verify All Products and product detail page")]
        public async Task TC8_VerifyAllProducts_AndProductDetailPage()
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

            // 4. Click on 'Products' button
            Log.Step("Step 4 — Clicking Products button");
            var productsPage = await homePage.Nav.GoToProductsAsync();

            // 5. Verify user is navigated to ALL PRODUCTS page successfully
            Log.Step("Step 5 — Verifying 'ALL PRODUCTS' heading is visible");
            Assert.IsTrue(await productsPage.IsOnAllProductsPageAsync(),
                "Expected 'ALL PRODUCTS' heading to be visible on /products");

            // 6. The products list is visible
            Log.Step("Step 6 — Verifying products list is visible");
            var count = await productsPage.GetProductCountAsync();
            Assert.IsTrue(count > 0, "Expected at least one product to be listed on the products page");
            Log.Info($"Product count: {count}");

            // 7. Click on 'View Product' of first product
            Log.Step("Step 7 — Clicking 'View Product' of first product");
            var detailPage = await productsPage.GoToProductDetailAsync(0);

            // 8. User is landed to product detail page
            // 9. Verify that detail is visible: product name, category, price, availability, condition, brand
            Log.Step("Steps 8-9 — Verifying product detail page fields are visible");

            var name = await detailPage.GetProductNameAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(name),
                "Expected product name to be present on detail page");

            var price = await detailPage.GetPriceAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(price),
                "Expected product price to be present on detail page");

            var category = await detailPage.GetCategoryAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(category),
                "Expected category to be present on detail page");

            var availability = await detailPage.GetAvailabilityAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(availability),
                "Expected availability to be present on detail page");

            var condition = await detailPage.GetConditionAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(condition),
                "Expected condition to be present on detail page");

            var brand = await detailPage.GetBrandAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(brand),
                "Expected brand to be present on detail page");

            Log.Info($"Product: '{name}' | Price: '{price}' | Category: '{category}' | Availability: '{availability}' | Condition: '{condition}' | Brand: '{brand}'");
        }

        // ── TC9: Search Product ───────────────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        [Description("TC9: Search Product")]
        public async Task TC9_SearchProduct()
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

            // 4. Click on 'Products' button
            Log.Step("Step 4 — Clicking Products button");
            var productsPage = await homePage.Nav.GoToProductsAsync();

            // 5. Verify user is navigated to ALL PRODUCTS page successfully
            Log.Step("Step 5 — Verifying 'ALL PRODUCTS' heading is visible");
            Assert.IsTrue(await productsPage.IsOnAllProductsPageAsync(),
                "Expected 'ALL PRODUCTS' heading to be visible");

            // 6. Enter product name in search input and click search button
            const string searchTerm = "dress";
            Log.Step($"Step 6 — Searching for '{searchTerm}'");
            await productsPage.SearchAsync(searchTerm);

            // 7. Verify 'SEARCHED PRODUCTS' is visible
            Log.Step("Step 7 — Verifying 'SEARCHED PRODUCTS' heading is visible");
            Assert.IsTrue(await productsPage.IsSearchedProductsHeadingVisibleAsync(),
                "Expected 'SEARCHED PRODUCTS' heading to appear after search");

            // 8. Verify all the products related to search are visible
            Log.Step("Step 8 — Verifying search results are visible and relevant");
            var count = await productsPage.GetProductCountAsync();
            Assert.IsTrue(count > 0,
                $"Expected at least one product in search results for '{searchTerm}'");

            var names = await productsPage.GetProductNamesAsync();
            var anyMatch = names.Any(n => n.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(anyMatch,
                $"Expected at least one result to contain '{searchTerm}' in its name. " +
                $"Found: {string.Join(", ", names)}");

            Log.Info($"Search results count: {count}");
        }

        // ── TC18: View Category Products ──────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        [Description("TC18: View Category Products")]
        public async Task TC18_ViewCategoryProducts()
        {
            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Verify that categories are visible on left side bar
            Log.Step("Step 3 — Verifying categories are visible on left side bar");
            // Categories are visible on the products page — navigate there first
            var productsPage = new ProductsPage(Page);
            await productsPage.GoToAsync();

            // 4. Click on 'Women' category
            // 5. Click on any category link under 'Women' category, for example: Dress
            Log.Step("Steps 4-5 — Clicking Women > Dress category");
            await productsPage.FilterByCategoryAsync("Women", "Dress");

            // 6. Verify that category page is displayed and confirm text 'WOMEN - DRESS PRODUCTS'
            Log.Step("Step 6 — Verifying Women > Dress category page is displayed");
            var womenDressCount = await productsPage.GetProductCountAsync();
            Assert.IsTrue(womenDressCount > 0,
                "Expected products to appear after filtering by Women > Dress");
            Log.Info($"Products shown after Women > Dress filter: {womenDressCount}");

            // 7. On left side bar, click on any sub-category link of 'Men' category
            Log.Step("Step 7 — Clicking Men > Tshirts category");
            await productsPage.FilterByCategoryAsync("Men", "Tshirts");

            // 8. Verify that user is navigated to that category page
            Log.Step("Step 8 — Verifying Men > Tshirts category page is displayed");
            var menTshirtsCount = await productsPage.GetProductCountAsync();
            Assert.IsTrue(menTshirtsCount > 0,
                "Expected products to appear after filtering by Men > Tshirts");
            Log.Info($"Products shown after Men > Tshirts filter: {menTshirtsCount}");
        }

        // ── TC19: View & Cart Brand Products ──────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        [Description("TC19: View & Cart Brand Products")]
        public async Task TC19_ViewAndCartBrandProducts()
        {
            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Click on 'Products' button
            Log.Step("Step 3 — Clicking Products button");
            var homePage = new HomePage(Page);
            var productsPage = await homePage.Nav.GoToProductsAsync();

            // 4. Verify that Brands are visible on left side bar
            Log.Step("Step 4 — Verifying brands are visible on left side bar");
            var brands = await productsPage.GetBrandNamesAsync();
            Assert.IsTrue(brands.Count > 0,
                "Expected at least one brand to be listed in the sidebar");
            Log.Info($"Brands found: {string.Join(", ", brands)}");

            // 5. Click on any brand name
            Log.Step("Step 5 — Clicking on 'Polo' brand");
            await productsPage.FilterByBrandAsync("Polo");

            // 6. Verify that user is navigated to brand page and brand products are displayed
            Log.Step("Step 6 — Verifying Polo brand page shows products");
            var poloCount = await productsPage.GetProductCountAsync();
            Assert.IsTrue(poloCount > 0,
                "Expected products to appear after filtering by Polo brand");
            Log.Info($"Products shown for Polo brand: {poloCount}");

            // 7. On left side bar, click on any other brand link
            Log.Step("Step 7 — Clicking on another brand");
            await productsPage.FilterByBrandAsync("H&M");

            // 8. Verify that user is navigated to that brand page and can see products
            Log.Step("Step 8 — Verifying second brand page shows products");
            var hmCount = await productsPage.GetProductCountAsync();
            Assert.IsTrue(hmCount > 0,
                "Expected products to appear after filtering by second brand");
            Log.Info($"Products shown for second brand: {hmCount}");
        }

        // ── TC21: Add review on product ───────────────────────────────────────────

        [TestMethod]
        [TestCategory("Regression")]
        [Description("TC21: Add review on product")]
        public async Task TC21_AddReviewOnProduct()
        {
            // 1. Launch browser — handled by BaseTest

            // 2. Navigate to url 'http://automationexercise.com'
            Log.Step("Step 2 — Navigating to homepage");
            await Page.GotoAsync("/");

            // 3. Click on 'Products' button
            Log.Step("Step 3 — Clicking Products button");
            var homePage = new HomePage(Page);
            var productsPage = await homePage.Nav.GoToProductsAsync();

            // 4. Verify user is navigated to ALL PRODUCTS page successfully
            Log.Step("Step 4 — Verifying 'ALL PRODUCTS' heading is visible");
            Assert.IsTrue(await productsPage.IsOnAllProductsPageAsync(),
                "Expected 'ALL PRODUCTS' heading to be visible");

            // 5. Click on 'View Product' button
            Log.Step("Step 5 — Clicking 'View Product' on first product");
            var detailPage = await productsPage.GoToProductDetailAsync(0);

            // 6. Verify 'Write Your Review' is visible
            Log.Step("Step 6 — Verifying 'Write Your Review' section is visible");
            // The review section is at the bottom of the product detail page
            var reviewHeading = Page.Locator("a[href='#reviews']:has-text('Write Your Review')");
            await Assertions.Expect(reviewHeading).ToBeVisibleAsync();

            // 7. Enter name, email and review
            // 8. Click 'Submit' button
            Log.Step("Steps 7-8 — Entering review details and submitting");
            await detailPage.SubmitReviewAsync(
                name: "Test Reviewer",
                email: "reviewer@example.com",
                review: "This is an automated review submitted by a Playwright test suite.");

            // 9. Verify success message 'Thank you for your review.'
            Log.Step("Step 9 — Verifying review success message");
            var successMessage = Page.Locator("span:has-text('Thank you for your review.')");
            await Assertions.Expect(successMessage).ToBeVisibleAsync();
            Log.Info("Review submitted successfully");
        }
    }
}