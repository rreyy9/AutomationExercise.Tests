namespace AutomationExercise.API.Tests.Tests
{
    [TestClass]
    [TestCategory("Regression")]
    public class SearchProductApiTests : BaseApiTest
    {
        // ── API 5: POST To Search Product ─────────────────────────────────────────
        //
        // API URL: https://automationexercise.com/api/searchProduct
        // Request Method: POST
        // Request Parameter: search_product (For example: top, tshirt, jean)
        // Response Code: 200
        // Response JSON: Searched products list

        [TestMethod]
        [Description("API 5: POST To Search Product — verify responseCode 200")]
        public async Task API5_PostToSearchProduct_ReturnsResponseCode200()
        {
            var response = await Api.PostFormAsync("/api/searchProduct", new Dictionary<string, string>
            {
                { "search_product", "dress" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponseWithProducts>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
        }

        [TestMethod]
        [Description("API 5: POST To Search Product — verify searched products list is not empty")]
        public async Task API5_PostToSearchProduct_ReturnsMatchingResults()
        {
            const string searchTerm = "dress";

            var response = await Api.PostFormAsync("/api/searchProduct", new Dictionary<string, string>
            {
                { "search_product", searchTerm }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponseWithProducts>(response);

            Assert.IsTrue(body.Products.Count > 0,
                $"Expected at least one product result for search term '{searchTerm}'");
        }

        [TestMethod]
        [Description("API 5: POST To Search Product — verify result names contain search term")]
        public async Task API5_PostToSearchProduct_ResultNamesContainSearchTerm()
        {
            const string searchTerm = "top";

            var response = await Api.PostFormAsync("/api/searchProduct", new Dictionary<string, string>
            {
                { "search_product", searchTerm }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponseWithProducts>(response);

            Assert.IsTrue(body.Products.Count > 0,
                $"Expected at least one result for search term '{searchTerm}'");

            var anyMatch = body.Products.Any(p =>
                p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            Assert.IsTrue(anyMatch,
                $"Expected at least one product name to contain '{searchTerm}'. " +
                $"Found: {string.Join(", ", body.Products.Select(p => p.Name))}");
        }

        // ── API 6: POST To Search Product without search_product parameter ────────
        //
        // API URL: https://automationexercise.com/api/searchProduct
        // Request Method: POST
        // Response Code: 400
        // Response Message: Bad request, search_product parameter is missing in POST request.
        //
        // NOTE: This is a documented real-world quirk of this API.
        // The HTTP status code returned is 200, but the responseCode in the body
        // is 400, signalling the missing parameter error. Tests must inspect the
        // body — relying on HTTP status alone would give a false pass here.

        [TestMethod]
        [Description("API 6: POST To Search Product without search_product parameter — verify responseCode 400")]
        public async Task API6_PostToSearchProductWithoutParam_ReturnsResponseCode400()
        {
            // POST with an empty body — no search_product parameter provided
            var response = await Api.PostFormAsync("/api/searchProduct", new Dictionary<string, string>());
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(400, body.ResponseCode,
                $"Expected responseCode 400 in body for missing parameter but got {body.ResponseCode}. " +
                "This verifies the API signals errors via responseCode, not HTTP status.");
        }

        [TestMethod]
        [Description("API 6: POST To Search Product without search_product parameter — verify response message")]
        public async Task API6_PostToSearchProductWithoutParam_ReturnsCorrectErrorMessage()
        {
            var response = await Api.PostFormAsync("/api/searchProduct", new Dictionary<string, string>());
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(
                "Bad request, search_product parameter is missing in POST request.",
                body.Message,
                $"Expected specific error message but got '{body.Message}'");
        }
    }
}