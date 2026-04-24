using AutomationExercise.API.Tests.Clients;
using AutomationExercise.API.Tests.Models;
using AutomationExercise.API.Tests.Utils;

namespace AutomationExercise.API.Tests.Tests
{
    /// <summary>
    /// Tests for the /api/verifyLogin endpoint.
    ///
    /// This class manages its own throwaway account via ClassInitialize/ClassCleanup
    /// so it has zero dependency on AccountLifecycleTests or execution order.
    /// Previously it shared credentials with AccountLifecycleTests, which caused a
    /// race condition under parallel execution — the account might not exist yet
    /// when VerifyLogin_WithValidCredentials ran. Self-contained fixture eliminates
    /// that entirely.
    /// </summary>
    [TestClass]
    [TestCategory("Regression")]
    [DoNotParallelize] // ClassInitialize/ClassCleanup manage shared static state — unsafe to run concurrently
    public class AuthApiTests : BaseApiTest
    {
        private const string TestEmail = "playwright.auth.verify.test@example.com";
        private const string TestPassword = "AuthTest1234!";

        private static ApiClient _staticApi = null!;

        // ── Fixture setup ─────────────────────────────────────────────────────────

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext _)
        {
            var config = ApiTestConfig.Load();
            _staticApi = new ApiClient(config.BaseUrl, config.Timeout);

            // Clean up any leftover account from a previous interrupted run
            await _staticApi.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
            {
                { "email",    TestEmail },
                { "password", TestPassword }
            });

            // Create the account this class needs
            var response = await _staticApi.PostFormAsync("/api/createAccount", new Dictionary<string, string>
            {
                { "name",          "Auth Verify Test" },
                { "email",         TestEmail },
                { "password",      TestPassword },
                { "title",         "Mr" },
                { "birth_date",    "1" },
                { "birth_month",   "1" },
                { "birth_year",    "1990" },
                { "firstname",     "Auth" },
                { "lastname",      "Test" },
                { "company",       "" },
                { "address1",      "1 Test Street" },
                { "address2",      "" },
                { "country",       "United Kingdom" },
                { "zipcode",       "SW1A 1AA" },
                { "state",         "England" },
                { "city",          "London" },
                { "mobile_number", "07700900000" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);
            if (body.ResponseCode != 201)
                throw new InvalidOperationException(
                    $"AuthApiTests.ClassInitialize: account creation failed. " +
                    $"responseCode={body.ResponseCode}, message={body.Message}");
        }

        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            if (_staticApi is not null)
            {
                await _staticApi.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
                {
                    { "email",    TestEmail },
                    { "password", TestPassword }
                });

                _staticApi.Dispose();
            }
        }

        // ── POST /api/verifyLogin — valid credentials ─────────────────────────────

        [TestMethod]
        public async Task VerifyLogin_WithValidCredentials_ReturnsSuccess()
        {
            var response = await Api.PostFormAsync("/api/verifyLogin", new Dictionary<string, string>
            {
                { "email",    TestEmail },
                { "password", TestPassword }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 for valid credentials but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
        }

        // ── POST /api/verifyLogin — invalid credentials ───────────────────────────

        [TestMethod]
        public async Task VerifyLogin_WithInvalidCredentials_ReturnsNotFound()
        {
            var response = await Api.PostFormAsync("/api/verifyLogin", new Dictionary<string, string>
            {
                { "email",    "notareal@user.com" },
                { "password", "wrongpassword" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            // The API returns responseCode 404 in the body for unrecognised credentials.
            // The HTTP status is still 200 — see the notable observation in PORTFOLIO_PLAN.md.
            Assert.AreEqual(404, body.ResponseCode,
                $"Expected responseCode 404 for invalid credentials but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
        }

        [TestMethod]
        public async Task VerifyLogin_WithInvalidCredentials_ResponseMessageIsNotEmpty()
        {
            var response = await Api.PostFormAsync("/api/verifyLogin", new Dictionary<string, string>
            {
                { "email",    "notareal@user.com" },
                { "password", "wrongpassword" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.IsFalse(string.IsNullOrWhiteSpace(body.Message),
                "Expected a descriptive message in the error response body");
        }

        // ── DELETE /api/verifyLogin — method not supported ────────────────────────
        //
        // The API explicitly documents that DELETE is not a supported method on
        // this endpoint. The responseCode in the body should be 405.

        [TestMethod]
        public async Task VerifyLogin_WithDeleteMethod_ReturnsMethodNotAllowed()
        {
            var response = await Api.DeleteAsync("/api/verifyLogin");
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(405, body.ResponseCode,
                $"Expected responseCode 405 (Method Not Allowed) for DELETE on /api/verifyLogin " +
                $"but got {body.ResponseCode}. Message: {body.Message}");
        }
    }
}