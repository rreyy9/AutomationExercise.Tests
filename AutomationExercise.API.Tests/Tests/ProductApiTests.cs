namespace AutomationExercise.API.Tests.Tests
{
    [TestClass]
    [TestCategory("Regression")]
    public class ProductsApiTests : BaseApiTest
    {
        // ── API 1: Get All Products List ──────────────────────────────────────────
        //
        // API URL: https://automationexercise.com/api/productsList
        // Request Method: GET
        // Response Code: 200
        // Response JSON: All products list

        [TestMethod]
        [Description("API 1: Get All Products List — verify responseCode 200")]
        public async Task API1_GetAllProductsList_ReturnsResponseCode200()
        {
            var response = await Api.GetAsync("/api/productsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithProducts>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
        }

        [TestMethod]
        [Description("API 1: Get All Products List — verify products list is not empty")]
        public async Task API1_GetAllProductsList_ProductsListIsNotEmpty()
        {
            var response = await Api.GetAsync("/api/productsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithProducts>(response);

            Assert.IsTrue(body.Products.Count > 0,
                "Expected at least one product in the products list response");
        }

        [TestMethod]
        [Description("API 1: Get All Products List — verify first product has required schema fields")]
        public async Task API1_GetAllProductsList_FirstProductHasRequiredFields()
        {
            var response = await Api.GetAsync("/api/productsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithProducts>(response);

            Assert.IsTrue(body.Products.Count > 0, "Cannot validate schema — products list is empty");

            var first = body.Products[0];

            Assert.IsTrue(first.Id > 0,
                $"Expected product Id to be a positive integer but got {first.Id}");
            Assert.IsFalse(string.IsNullOrWhiteSpace(first.Name),
                "Expected product Name to be present");
            Assert.IsFalse(string.IsNullOrWhiteSpace(first.Price),
                "Expected product Price to be present");
            Assert.IsFalse(string.IsNullOrWhiteSpace(first.Brand),
                "Expected product Brand to be present");
        }

        // ── API 2: POST To All Products List ─────────────────────────────────────
        //
        // API URL: https://automationexercise.com/api/productsList
        // Request Method: POST
        // Response Code: 405
        // Response Message: This request method is not supported.

        [TestMethod]
        [Description("API 2: POST To All Products List — verify responseCode 405")]
        public async Task API2_PostToAllProductsList_ReturnsResponseCode405()
        {
            var response = await Api.PostFormAsync("/api/productsList", new Dictionary<string, string>());
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(405, body.ResponseCode,
                $"Expected responseCode 405 (Method Not Supported) for POST on /api/productsList " +
                $"but got {body.ResponseCode}. Message: {body.Message}");
        }

        [TestMethod]
        [Description("API 2: POST To All Products List — verify response message")]
        public async Task API2_PostToAllProductsList_ReturnsMethodNotSupportedMessage()
        {
            var response = await Api.PostFormAsync("/api/productsList", new Dictionary<string, string>());
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual("This request method is not supported.", body.Message,
                $"Expected 'This request method is not supported.' but got '{body.Message}'");
        }

        // ── API 3: Get All Brands List ────────────────────────────────────────────
        //
        // API URL: https://automationexercise.com/api/brandsList
        // Request Method: GET
        // Response Code: 200
        // Response JSON: All brands list

        [TestMethod]
        [Description("API 3: Get All Brands List — verify responseCode 200")]
        public async Task API3_GetAllBrandsList_ReturnsResponseCode200()
        {
            var response = await Api.GetAsync("/api/brandsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithBrands>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
        }

        [TestMethod]
        [Description("API 3: Get All Brands List — verify brands list is not empty")]
        public async Task API3_GetAllBrandsList_BrandsListIsNotEmpty()
        {
            var response = await Api.GetAsync("/api/brandsList");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithBrands>(response);

            Assert.IsTrue(body.Brands.Count > 0,
                "Expected at least one brand in the brands list response");
        }

        [TestMethod]
        [Description("API 3: Get All Brands List — verify each brand has id and name")]
        public async Task API3_GetAllBrandsList_EachBrandHasNameAndId()
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

        // ── API 4: PUT To All Brands List ─────────────────────────────────────────
        //
        // API URL: https://automationexercise.com/api/brandsList
        // Request Method: PUT
        // Response Code: 405
        // Response Message: This request method is not supported.

        [TestMethod]
        [Description("API 4: PUT To All Brands List — verify responseCode 405")]
        public async Task API4_PutToAllBrandsList_ReturnsResponseCode405()
        {
            var response = await Api.PutFormAsync("/api/brandsList", new Dictionary<string, string>());
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(405, body.ResponseCode,
                $"Expected responseCode 405 (Method Not Supported) for PUT on /api/brandsList " +
                $"but got {body.ResponseCode}. Message: {body.Message}");
        }

        [TestMethod]
        [Description("API 4: PUT To All Brands List — verify response message")]
        public async Task API4_PutToAllBrandsList_ReturnsMethodNotSupportedMessage()
        {
            var response = await Api.PutFormAsync("/api/brandsList", new Dictionary<string, string>());
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual("This request method is not supported.", body.Message,
                $"Expected 'This request method is not supported.' but got '{body.Message}'");
        }
    }
}