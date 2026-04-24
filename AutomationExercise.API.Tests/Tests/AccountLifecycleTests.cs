using AutomationExercise.API.Tests.Clients;
using AutomationExercise.API.Tests.Models;
using AutomationExercise.API.Tests.Utils;

namespace AutomationExercise.API.Tests.Tests
{
    /// <summary>
    /// Full account lifecycle test suite: create → update → read → delete.
    ///
    /// Data strategy: shared fixture via ClassInitialize/ClassCleanup.
    ///
    /// Why ClassInitialize/ClassCleanup over a single ordered test method:
    ///   - Account creation and deletion are infrastructure concerns, not test
    ///     assertions. ClassInitialize/ClassCleanup is the correct MSTest hook for
    ///     this — it mirrors how real teams manage test fixtures.
    ///   - Individual [TestMethod]s for update and read are independently named,
    ///     independently reported, and independently skippable during debugging.
    ///   - ClassCleanup guarantees deletion even if a test method fails mid-suite —
    ///     a single ordered method would leave a dangling account on the live site
    ///     if the update or read assertion threw.
    ///
    /// This class is fully self-contained — it owns its own account credentials
    /// and does not share state with any other test class.
    ///
    /// NOTE: ClassInitialize is static in MSTest — ApiClient is instantiated
    /// directly here rather than relying on the BaseApiTest instance property,
    /// which is not accessible from static context.
    /// </summary>
    [TestClass]
    [TestCategory("Regression")]
    [DoNotParallelize] // ClassInitialize/ClassCleanup manage shared static state — unsafe to run concurrently
    public class AccountLifecycleTests
    {
        private const string Email = "playwright.lifecycle.test@example.com";
        private const string Password = "LifecycleTest1234!";

        private static ApiClient _staticApi = null!;
        private ApiClient _api = null!;

        public TestContext TestContext { get; set; } = null!;

        // ── Fixture setup ─────────────────────────────────────────────────────────

        [ClassInitialize]
        public static async Task ClassInitialize(TestContext _)
        {
            var config = ApiTestConfig.Load();
            _staticApi = new ApiClient(config.BaseUrl, config.Timeout);

            // Clean up any leftover account from a previous interrupted run
            // before attempting creation — makes the suite idempotent.
            await _staticApi.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
            {
                { "email",    Email },
                { "password", Password }
            });

            var response = await _staticApi.PostFormAsync("/api/createAccount", new Dictionary<string, string>
            {
                { "name",          "Playwright Lifecycle" },
                { "email",         Email },
                { "password",      Password },
                { "title",         "Mr" },
                { "birth_date",    "15" },
                { "birth_month",   "6" },
                { "birth_year",    "1990" },
                { "firstname",     "Playwright" },
                { "lastname",      "Lifecycle" },
                { "company",       "Test Suite Ltd" },
                { "address1",      "123 Automation Street" },
                { "address2",      "Suite 1" },
                { "country",       "United Kingdom" },
                { "zipcode",       "SW1A 1AA" },
                { "state",         "England" },
                { "city",          "London" },
                { "mobile_number", "07700900000" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);
            if (body.ResponseCode != 201)
                throw new InvalidOperationException(
                    $"AccountLifecycleTests.ClassInitialize: account creation failed. " +
                    $"responseCode={body.ResponseCode}, message={body.Message}");
        }

        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            // Always attempt deletion — even if a test method fails mid-suite.
            // This is the core benefit of ClassCleanup over a single ordered test:
            // the live site is not left with a dangling account.
            if (_staticApi is not null)
            {
                await _staticApi.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
                {
                    { "email",    Email },
                    { "password", Password }
                });

                _staticApi.Dispose();
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var config = ApiTestConfig.Load();
            _api = new ApiClient(config.BaseUrl, config.Timeout);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _api?.Dispose();
        }

        // ── createAccount ─────────────────────────────────────────────────────────
        // Account creation is asserted inside ClassInitialize (it throws on failure),
        // so the test method below validates the outcome rather than repeating the call.

        [TestMethod]
        public async Task CreateAccount_AccountExistsAfterCreation()
        {
            var response = await _api.GetAsync($"/api/getUserDetailByEmail?email={Uri.EscapeDataString(Email)}");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithUser>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 confirming account exists after creation. " +
                $"Got {body.ResponseCode}: {body.Message}");
            Assert.IsNotNull(body.User,
                "Expected user detail to be returned after account creation");
            Assert.AreEqual(Email, body.User.Email,
                "Expected returned email to match created account email");
        }

        // ── updateAccount ─────────────────────────────────────────────────────────

        [TestMethod]
        public async Task UpdateAccount_WithNewCity_ReturnsSuccess()
        {
            var response = await _api.PutFormAsync("/api/updateAccount", new Dictionary<string, string>
            {
                { "name",          "Playwright Lifecycle" },
                { "email",         Email },
                { "password",      Password },
                { "title",         "Mr" },
                { "birth_date",    "15" },
                { "birth_month",   "6" },
                { "birth_year",    "1990" },
                { "firstname",     "Playwright" },
                { "lastname",      "Lifecycle" },
                { "company",       "Updated Suite Ltd" },
                { "address1",      "456 Updated Street" },
                { "address2",      "" },
                { "country",       "United Kingdom" },
                { "zipcode",       "EC1A 1BB" },
                { "state",         "England" },
                { "city",          "Manchester" },
                { "mobile_number", "07700900001" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 after account update but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
            Assert.IsFalse(string.IsNullOrWhiteSpace(body.Message),
                "Expected a confirmation message in the update response");
        }

        [TestMethod]
        public async Task UpdateAccount_UpdatedFieldsAreReflectedOnRead()
        {
            // Update city to a known value
            await _api.PutFormAsync("/api/updateAccount", new Dictionary<string, string>
            {
                { "name",          "Playwright Lifecycle" },
                { "email",         Email },
                { "password",      Password },
                { "title",         "Mr" },
                { "birth_date",    "15" },
                { "birth_month",   "6" },
                { "birth_year",    "1990" },
                { "firstname",     "Playwright" },
                { "lastname",      "Lifecycle" },
                { "company",       "Updated Suite Ltd" },
                { "address1",      "456 Updated Street" },
                { "address2",      "" },
                { "country",       "United Kingdom" },
                { "zipcode",       "EC1A 1BB" },
                { "state",         "England" },
                { "city",          "Manchester" },
                { "mobile_number", "07700900001" }
            });

            // Read back and confirm the city was persisted
            var readResponse = await _api.GetAsync($"/api/getUserDetailByEmail?email={Uri.EscapeDataString(Email)}");
            var readBody = await ApiClient.DeserialiseAsync<ApiResponseWithUser>(readResponse);

            Assert.AreEqual(200, readBody.ResponseCode,
                $"Expected responseCode 200 on read after update but got {readBody.ResponseCode}");
            Assert.IsNotNull(readBody.User,
                "Expected user detail to be present after update");
            Assert.AreEqual("Manchester", readBody.User.City,
                $"Expected city to be 'Manchester' after update but got '{readBody.User.City}'");
        }

        // ── getUserDetailByEmail ──────────────────────────────────────────────────

        [TestMethod]
        public async Task GetUserDetailByEmail_ReturnsCorrectUserData()
        {
            var response = await _api.GetAsync($"/api/getUserDetailByEmail?email={Uri.EscapeDataString(Email)}");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithUser>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 but got {body.ResponseCode}. Message: {body.Message}");
            Assert.IsNotNull(body.User,
                "Expected user object to be present in response");
            Assert.AreEqual(Email, body.User.Email,
                $"Expected email '{Email}' but got '{body.User.Email}'");
            Assert.IsFalse(string.IsNullOrWhiteSpace(body.User.FirstName),
                "Expected FirstName to be present on the user detail");
        }

        [TestMethod]
        public async Task GetUserDetailByEmail_WithUnknownEmail_ReturnsNotFound()
        {
            var response = await _api.GetAsync("/api/getUserDetailByEmail?email=nobody%40nowhere.com");
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            // The API returns responseCode 404 in the body when no user matches.
            // HTTP status is still 200 — consistent with the documented API quirk.
            Assert.AreEqual(404, body.ResponseCode,
                $"Expected responseCode 404 for unknown email but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
        }

        // ── deleteAccount ─────────────────────────────────────────────────────────
        // Actual deletion of the fixture account is performed by ClassCleanup to
        // guarantee it always runs. This test exercises the delete endpoint in
        // isolation using a separate throwaway account so it doesn't interfere
        // with the other test methods that depend on the fixture account existing.

        [TestMethod]
        public async Task DeleteAccount_ReturnsSuccess()
        {
            const string throwawayEmail = "playwright.lifecycle.delete.test@example.com";
            const string throwawayPassword = "DeleteTest1234!";

            // Clean up any previous leftover first
            await _api.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
            {
                { "email",    throwawayEmail },
                { "password", throwawayPassword }
            });

            // Create a fresh throwaway account
            await _api.PostFormAsync("/api/createAccount", new Dictionary<string, string>
            {
                { "name",          "Delete Test" },
                { "email",         throwawayEmail },
                { "password",      throwawayPassword },
                { "title",         "Mrs" },
                { "birth_date",    "1" },
                { "birth_month",   "1" },
                { "birth_year",    "2000" },
                { "firstname",     "Delete" },
                { "lastname",      "Test" },
                { "company",       "" },
                { "address1",      "1 Test Lane" },
                { "address2",      "" },
                { "country",       "United Kingdom" },
                { "zipcode",       "W1A 0AX" },
                { "state",         "England" },
                { "city",          "London" },
                { "mobile_number", "07700900002" }
            });

            // Delete and assert
            var deleteResponse = await _api.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
            {
                { "email",    throwawayEmail },
                { "password", throwawayPassword }
            });

            var deleteBody = await ApiClient.DeserialiseAsync<ApiResponse>(deleteResponse);

            Assert.AreEqual(200, deleteBody.ResponseCode,
                $"Expected responseCode 200 after account deletion but got {deleteBody.ResponseCode}. " +
                $"Message: {deleteBody.Message}");
            Assert.IsFalse(string.IsNullOrWhiteSpace(deleteBody.Message),
                "Expected a confirmation message in the delete response");
        }
    }
}