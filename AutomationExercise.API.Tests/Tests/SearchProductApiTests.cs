using AutomationExercise.API.Tests.Clients;
using AutomationExercise.API.Tests.Models;

namespace AutomationExercise.API.Tests.Tests
{
    [TestClass]
    [TestCategory("Regression")]
    public class SearchProductApiTests : BaseApiTest
    {
        // ── POST /api/searchProduct — valid search ─────────────────────────────────

        [TestMethod]
        public async Task SearchProduct_WithValidTerm_ReturnsSuccessResponseCode()
        {
            var response = await Api.PostFormAsync("/api/searchProduct", new Dictionary<string, string>
            {
                { "search_product", "dress" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponseWithProducts>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 but got {body.ResponseCode}. Message: {body.Message}");
        }

        [TestMethod]
        public async Task SearchProduct_WithValidTerm_ReturnsMatchingResults()
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
        public async Task SearchProduct_WithValidTerm_ResultNamesContainSearchTerm()
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

        // ── POST /api/searchProduct — missing parameter ────────────────────────────
        //
        // NOTE: This is a documented real-world quirk of this API.
        // The HTTP status code returned is 200, but the responseCode in the body
        // is 400, signalling the missing parameter error. Tests must inspect the
        // body — relying on HTTP status alone would give a false pass here.

        [TestMethod]
        public async Task SearchProduct_WithMissingParameter_ReturnsBadRequestInBody()
        {
            // POST with an empty body — no search_product parameter provided
            var response = await Api.PostFormAsync("/api/searchProduct", new Dictionary<string, string>());
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(400, body.ResponseCode,
                $"Expected responseCode 400 in body for missing parameter but got {body.ResponseCode}. " +
                "This verifies the API signals errors via responseCode, not HTTP status.");
        }

        [TestMethod]
        public async Task SearchProduct_WithMissingParameter_ResponseMessageIsNotEmpty()
        {
            var response = await Api.PostFormAsync("/api/searchProduct", new Dictionary<string, string>());
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.IsFalse(string.IsNullOrWhiteSpace(body.Message),
                "Expected a descriptive error message when required parameter is missing");
        }
    }
}