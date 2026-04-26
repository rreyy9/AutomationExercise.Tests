using AutomationExercise.API.Tests.Utils;

namespace AutomationExercise.API.Tests.Tests
{
    /// <summary>
    /// Tests for the /api/verifyLogin endpoint (API 7, 8, 9, 10).
    ///
    /// This class manages its own dedicated test account via ClassInitialize/ClassCleanup.
    /// The account is created before any test method runs and deleted after all complete,
    /// making this class fully self-contained — it can run in any order or in isolation.
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

            // Pre-emptive delete — handles leftover from interrupted previous runs
            await _staticApi.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
            {
                { "email",    TestEmail },
                { "password", TestPassword }
            });

            var response = await _staticApi.PostFormAsync("/api/createAccount", new Dictionary<string, string>
            {
                { "name",          "Auth Verify Test" },
                { "email",         TestEmail },
                { "password",      TestPassword },
                { "title",         "Mr" },
                { "birth_date",    "1" },
                { "birth_month",   "1" },
                { "birth_year",    "1995" },
                { "firstname",     "Auth" },
                { "lastname",      "Verify" },
                { "company",       "Test Suite Ltd" },
                { "address1",      "1 Auth Street" },
                { "address2",      "" },
                { "country",       "United Kingdom" },
                { "zipcode",       "SW1A 1AA" },
                { "state",         "England" },
                { "city",          "London" },
                { "mobile_number", "07700900001" }
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
            await _staticApi.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
            {
                { "email",    TestEmail },
                { "password", TestPassword }
            });
            _staticApi.Dispose();
        }

        // ── API 7: POST To Verify Login with valid details ────────────────────────
        //
        // API URL: https://automationexercise.com/api/verifyLogin
        // Request Method: POST
        // Request Parameters: email, password
        // Response Code: 200
        // Response Message: User exists!

        [TestMethod]
        [Description("API 7: POST To Verify Login with valid details — verify responseCode 200")]
        public async Task API7_VerifyLoginWithValidDetails_ReturnsResponseCode200()
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

        [TestMethod]
        [Description("API 7: POST To Verify Login with valid details — verify response message 'User exists!'")]
        public async Task API7_VerifyLoginWithValidDetails_ReturnsUserExistsMessage()
        {
            var response = await Api.PostFormAsync("/api/verifyLogin", new Dictionary<string, string>
            {
                { "email",    TestEmail },
                { "password", TestPassword }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual("User exists!", body.Message,
                $"Expected 'User exists!' but got '{body.Message}'");
        }

        // ── API 8: POST To Verify Login without email parameter ───────────────────
        //
        // API URL: https://automationexercise.com/api/verifyLogin
        // Request Method: POST
        // Request Parameter: password
        // Response Code: 400
        // Response Message: Bad request, email or password parameter is missing in POST request.

        [TestMethod]
        [Description("API 8: POST To Verify Login without email parameter — verify responseCode 400")]
        public async Task API8_VerifyLoginWithoutEmail_ReturnsResponseCode400()
        {
            var response = await Api.PostFormAsync("/api/verifyLogin", new Dictionary<string, string>
            {
                { "password", TestPassword }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(400, body.ResponseCode,
                $"Expected responseCode 400 for missing email parameter but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
        }

        [TestMethod]
        [Description("API 8: POST To Verify Login without email parameter — verify response message")]
        public async Task API8_VerifyLoginWithoutEmail_ReturnsCorrectErrorMessage()
        {
            var response = await Api.PostFormAsync("/api/verifyLogin", new Dictionary<string, string>
            {
                { "password", TestPassword }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(
                "Bad request, email or password parameter is missing in POST request.",
                body.Message,
                $"Expected specific error message but got '{body.Message}'");
        }

        // ── API 9: DELETE To Verify Login ─────────────────────────────────────────
        //
        // API URL: https://automationexercise.com/api/verifyLogin
        // Request Method: DELETE
        // Response Code: 405
        // Response Message: This request method is not supported.

        [TestMethod]
        [Description("API 9: DELETE To Verify Login — verify responseCode 405")]
        public async Task API9_DeleteToVerifyLogin_ReturnsResponseCode405()
        {
            var response = await Api.DeleteAsync("/api/verifyLogin");
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(405, body.ResponseCode,
                $"Expected responseCode 405 (Method Not Allowed) for DELETE on /api/verifyLogin " +
                $"but got {body.ResponseCode}. Message: {body.Message}");
        }

        [TestMethod]
        [Description("API 9: DELETE To Verify Login — verify response message")]
        public async Task API9_DeleteToVerifyLogin_ReturnsMethodNotSupportedMessage()
        {
            var response = await Api.DeleteAsync("/api/verifyLogin");
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual("This request method is not supported.", body.Message,
                $"Expected 'This request method is not supported.' but got '{body.Message}'");
        }

        // ── API 10: POST To Verify Login with invalid details ─────────────────────
        //
        // API URL: https://automationexercise.com/api/verifyLogin
        // Request Method: POST
        // Request Parameters: email, password (invalid values)
        // Response Code: 404
        // Response Message: User not found!

        [TestMethod]
        [Description("API 10: POST To Verify Login with invalid details — verify responseCode 404")]
        public async Task API10_VerifyLoginWithInvalidDetails_ReturnsResponseCode404()
        {
            var response = await Api.PostFormAsync("/api/verifyLogin", new Dictionary<string, string>
            {
                { "email",    "notareal@user.com" },
                { "password", "wrongpassword" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            // The HTTP status is still 200 — see the notable observation in PORTFOLIO_PLAN.md.
            Assert.AreEqual(404, body.ResponseCode,
                $"Expected responseCode 404 for invalid credentials but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
        }

        [TestMethod]
        [Description("API 10: POST To Verify Login with invalid details — verify response message 'User not found!'")]
        public async Task API10_VerifyLoginWithInvalidDetails_ReturnsUserNotFoundMessage()
        {
            var response = await Api.PostFormAsync("/api/verifyLogin", new Dictionary<string, string>
            {
                { "email",    "notareal@user.com" },
                { "password", "wrongpassword" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual("User not found!", body.Message,
                $"Expected 'User not found!' but got '{body.Message}'");
        }
    }
}