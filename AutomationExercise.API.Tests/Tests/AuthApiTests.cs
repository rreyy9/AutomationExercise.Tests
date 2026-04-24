using AutomationExercise.API.Tests.Clients;
using AutomationExercise.API.Tests.Models;

namespace AutomationExercise.API.Tests.Tests
{
    /// <summary>
    /// Tests for the /api/verifyLogin endpoint.
    ///
    /// These tests use the credentials created by AccountLifecycleTests via
    /// ClassInitialize. Because ClassInitialize runs before any test in that class,
    /// and both test classes are independent, AuthApiTests uses its own fixed
    /// test account credentials.
    ///
    /// The valid-credentials tests depend on an account existing with these
    /// credentials on the live site. If the site resets, these will fail with
    /// responseCode 404 — the invalid-credentials and DELETE tests will still pass.
    ///
    /// NOTE: The valid-login test shares credentials with AccountLifecycleTests
    /// to avoid creating a second throwaway account. AccountLifecycleTests
    /// guarantees cleanup via ClassCleanup, so these credentials are only valid
    /// during a test run that includes AccountLifecycleTests. Run both classes
    /// together for a complete suite run.
    /// </summary>
    [TestClass]
    [TestCategory("Regression")]
    [DoNotParallelize] // depends on AccountLifecycleTests.ClassInitialize having run first
    public class AuthApiTests : BaseApiTest
    {
        // These credentials are created by AccountLifecycleTests.ClassInitialize
        // and deleted by AccountLifecycleTests.ClassCleanup.
        // Run the full suite to exercise the valid-login path end-to-end.
        internal const string TestEmail = "playwright.api.test@example.com";
        internal const string TestPassword = "ApiTest1234!";

        // ── POST /api/verifyLogin — valid credentials ─────────────────────────────

        [TestMethod]
        public async Task VerifyLogin_WithValidCredentials_ReturnsSuccess()
        {
            var response = await Api.PostFormAsync("/api/verifyLogin", new Dictionary<string, string>
            {
                { "email", TestEmail },
                { "password", TestPassword }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 for valid credentials but got {body.ResponseCode}. " +
                $"Message: {body.Message}. " +
                "Note: This test requires AccountLifecycleTests to have run first to create the account.");
        }

        // ── POST /api/verifyLogin — invalid credentials ───────────────────────────

        [TestMethod]
        public async Task VerifyLogin_WithInvalidCredentials_ReturnsNotFound()
        {
            var response = await Api.PostFormAsync("/api/verifyLogin", new Dictionary<string, string>
            {
                { "email", "notareal@user.com" },
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
                { "email", "notareal@user.com" },
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
        // This test confirms the API correctly rejects unsupported HTTP methods.

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