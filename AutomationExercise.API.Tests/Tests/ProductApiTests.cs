using AutomationExercise.API.Tests.Clients;
using AutomationExercise.API.Tests.Models;

namespace AutomationExercise.API.Tests.Tests
{
    [TestClass]
    [TestCategory("Regression")]
    public class ProductsApiTests : BaseApiTest
    {
        // ── GET /api/productsList ─────────────────────────────────────────────────

        [TestMethod]
        public async Task GetProductsList_ReturnsSuccessResponseCode()
        {
            var response = await Api.GetAsync("/api/productsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithProducts>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 but got {body.ResponseCode}. Message: {body.Message}");
        }

        [TestMethod]
        public async Task GetProductsList_ProductsListIsNotEmpty()
        {
            var response = await Api.GetAsync("/api/productsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithProducts>(response);

            Assert.IsTrue(body.Products.Count > 0,
                "Expected at least one product in the products list response");
        }

        [TestMethod]
        public async Task GetProductsList_FirstProduct_HasRequiredFields()
        {
            var response = await Api.GetAsync("/api/productsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithProducts>(response);

            Assert.IsTrue(body.Products.Count > 0, "Cannot validate schema — product list is empty");

            var first = body.Products[0];

            Assert.IsTrue(first.Id > 0,
                $"Expected product Id to be a positive integer but got {first.Id}");
            Assert.IsFalse(string.IsNullOrWhiteSpace(first.Name),
                "Expected product Name to be present");
            Assert.IsFalse(string.IsNullOrWhiteSpace(first.Price),
                "Expected product Price to be present");
            Assert.IsFalse(string.IsNullOrWhiteSpace(first.Brand),
                "Expected product Brand to be present");
            Assert.IsNotNull(first.Category,
                "Expected product Category to be present");
        }

        // ── GET /api/brandsList ───────────────────────────────────────────────────

        [TestMethod]
        public async Task GetBrandsList_ReturnsSuccessResponseCode()
        {
            var response = await Api.GetAsync("/api/brandsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithBrands>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 but got {body.ResponseCode}. Message: {body.Message}");
        }

        [TestMethod]
        public async Task GetBrandsList_BrandsListIsNotEmpty()
        {
            var response = await Api.GetAsync("/api/brandsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithBrands>(response);

            Assert.IsTrue(body.Brands.Count > 0,
                "Expected at least one brand in the brands list response");
        }

        [TestMethod]
        public async Task GetBrandsList_EachBrand_HasNameAndId()
        {
            var response = await Api.GetAsync("/api/brandsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithBrands>(response);

            Assert.IsTrue(body.Brands.Count > 0, "Cannot validate schema — brands list is empty");

            foreach (var brand in body.Brands)
            {
                Assert.IsTrue(brand.Id > 0,
                    $"Expected brand Id to be a positive integer but got {brand.Id}");
                Assert.IsFalse(string.IsNullOrWhiteSpace(brand.Name),
                    $"Expected brand Name to be present for brand with Id {brand.Id}");
            }
        }
    }
}