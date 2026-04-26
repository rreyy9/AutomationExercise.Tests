using AutomationExercise.API.Tests.Utils;

namespace AutomationExercise.API.Tests.Tests
{
    /// <summary>
    /// Full account lifecycle test suite covering API 11, 12, 13, 14.
    ///
    /// Data strategy — shared fixture via ClassInitialize/ClassCleanup.
    /// ClassInitialize creates the account before any test method runs.
    /// ClassCleanup deletes it after all complete — guarantees cleanup even on failure.
    /// Individual [TestMethod]s for each endpoint are independently named and reported.
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
            await _staticApi.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
            {
                { "email",    Email },
                { "password", Password }
            });
            _staticApi.Dispose();
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

        // ── API 11: POST To Create/Register User Account ─────────────────────────
        //
        // API URL: https://automationexercise.com/api/createAccount
        // Request Method: POST
        // Request Parameters: name, email, password, title, birth_date, birth_month,
        //   birth_year, firstname, lastname, company, address1, address2, country,
        //   zipcode, state, city, mobile_number
        // Response Code: 201
        // Response Message: User created!
        //
        // NOTE: The actual account creation is performed in ClassInitialize.
        // This test verifies the account exists by reading it back via getUserDetailByEmail.

        [TestMethod]
        [Description("API 11: POST To Create/Register User Account — verify account exists after creation")]
        public async Task API11_CreateAccount_AccountExistsAfterCreation()
        {
            var response = await _api.GetAsync($"/api/getUserDetailByEmail?email={Uri.EscapeDataString(Email)}");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithUser>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 confirming account exists but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
            Assert.IsNotNull(body.User,
                "Expected user object to be present in response after account creation");
            Assert.AreEqual(Email, body.User.Email,
                $"Expected email '{Email}' but got '{body.User.Email}'");
        }

        // ── API 13: PUT METHOD To Update User Account ─────────────────────────────
        //
        // API URL: https://automationexercise.com/api/updateAccount
        // Request Method: PUT
        // Request Parameters: name, email, password, title, birth_date, birth_month,
        //   birth_year, firstname, lastname, company, address1, address2, country,
        //   zipcode, state, city, mobile_number
        // Response Code: 200
        // Response Message: User updated!

        [TestMethod]
        [Description("API 13: PUT METHOD To Update User Account — verify responseCode 200")]
        public async Task API13_UpdateAccount_ReturnsResponseCode200()
        {
            var response = await _api.PutFormAsync("/api/updateAccount", new Dictionary<string, string>
            {
                { "name",          "Updated Lifecycle" },
                { "email",         Email },
                { "password",      Password },
                { "title",         "Mr" },
                { "birth_date",    "15" },
                { "birth_month",   "6" },
                { "birth_year",    "1990" },
                { "firstname",     "Updated" },
                { "lastname",      "Lifecycle" },
                { "company",       "Updated Suite Ltd" },
                { "address1",      "456 Updated Street" },
                { "address2",      "Suite 2" },
                { "country",       "United Kingdom" },
                { "zipcode",       "SW1A 1AA" },
                { "state",         "England" },
                { "city",          "London" },
                { "mobile_number", "07700900000" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 after update but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
        }

        [TestMethod]
        [Description("API 13: PUT METHOD To Update User Account — verify response message 'User updated!'")]
        public async Task API13_UpdateAccount_ReturnsUserUpdatedMessage()
        {
            var response = await _api.PutFormAsync("/api/updateAccount", new Dictionary<string, string>
            {
                { "name",          "Updated Lifecycle" },
                { "email",         Email },
                { "password",      Password },
                { "title",         "Mr" },
                { "birth_date",    "15" },
                { "birth_month",   "6" },
                { "birth_year",    "1990" },
                { "firstname",     "Updated" },
                { "lastname",      "Lifecycle" },
                { "company",       "Updated Suite Ltd" },
                { "address1",      "456 Updated Street" },
                { "address2",      "Suite 2" },
                { "country",       "United Kingdom" },
                { "zipcode",       "SW1A 1AA" },
                { "state",         "England" },
                { "city",          "London" },
                { "mobile_number", "07700900000" }
            });

            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            Assert.AreEqual("User updated!", body.Message,
                $"Expected 'User updated!' but got '{body.Message}'");
        }

        [TestMethod]
        [Description("API 13: PUT METHOD To Update User Account — verify updated field is reflected on subsequent read")]
        public async Task API13_UpdateAccount_UpdatedFieldReflectedOnRead()
        {
            // Update the company field
            await _api.PutFormAsync("/api/updateAccount", new Dictionary<string, string>
            {
                { "name",          "Updated Lifecycle" },
                { "email",         Email },
                { "password",      Password },
                { "title",         "Mr" },
                { "birth_date",    "15" },
                { "birth_month",   "6" },
                { "birth_year",    "1990" },
                { "firstname",     "Updated" },
                { "lastname",      "Lifecycle" },
                { "company",       "Verified Update Ltd" },
                { "address1",      "456 Updated Street" },
                { "address2",      "Suite 2" },
                { "country",       "United Kingdom" },
                { "zipcode",       "SW1A 1AA" },
                { "state",         "England" },
                { "city",          "London" },
                { "mobile_number", "07700900000" }
            });

            // Read back and verify
            var readResponse = await _api.GetAsync($"/api/getUserDetailByEmail?email={Uri.EscapeDataString(Email)}");
            var readBody = await ApiClient.DeserialiseAsync<ApiResponseWithUser>(readResponse);

            Assert.IsNotNull(readBody.User,
                "Expected user object to be present after update");
            Assert.AreEqual("Updated", readBody.User.FirstName,
                $"Expected updated FirstName 'Updated' but got '{readBody.User.FirstName}'");
        }

        // ── API 14: GET user account detail by email ──────────────────────────────
        //
        // API URL: https://automationexercise.com/api/getUserDetailByEmail
        // Request Method: GET
        // Request Parameters: email
        // Response Code: 200
        // Response JSON: User Detail

        [TestMethod]
        [Description("API 14: GET user account detail by email — verify responseCode 200 and user data")]
        public async Task API14_GetUserDetailByEmail_ReturnsCorrectUserData()
        {
            var response = await _api.GetAsync($"/api/getUserDetailByEmail?email={Uri.EscapeDataString(Email)}");
            var body = await ApiClient.DeserialiseAsync<ApiResponseWithUser>(response);

            Assert.AreEqual(200, body.ResponseCode,
                $"Expected responseCode 200 but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
            Assert.IsNotNull(body.User,
                "Expected user object to be present in response");
            Assert.AreEqual(Email, body.User.Email,
                $"Expected email '{Email}' but got '{body.User.Email}'");
            Assert.IsFalse(string.IsNullOrWhiteSpace(body.User.FirstName),
                "Expected FirstName to be present on the user detail");
        }

        [TestMethod]
        [Description("API 14: GET user account detail by email — verify responseCode 404 for unknown email")]
        public async Task API14_GetUserDetailByEmail_WithUnknownEmail_ReturnsNotFound()
        {
            var response = await _api.GetAsync("/api/getUserDetailByEmail?email=nobody%40nowhere.com");
            var body = await ApiClient.DeserialiseAsync<ApiResponse>(response);

            // The API returns responseCode 404 in the body when no user matches.
            // HTTP status is still 200 — consistent with the documented API quirk.
            Assert.AreEqual(404, body.ResponseCode,
                $"Expected responseCode 404 for unknown email but got {body.ResponseCode}. " +
                $"Message: {body.Message}");
        }

        // ── API 12: DELETE METHOD To Delete User Account ──────────────────────────
        //
        // API URL: https://automationexercise.com/api/deleteAccount
        // Request Method: DELETE
        // Request Parameters: email, password
        // Response Code: 200
        // Response Message: Account deleted!
        //
        // NOTE: Actual deletion of the fixture account is performed by ClassCleanup
        // to guarantee it always runs. This test exercises the delete endpoint in
        // isolation using a separate throwaway account so it doesn't interfere
        // with the other test methods that depend on the fixture account existing.

        [TestMethod]
        [Description("API 12: DELETE METHOD To Delete User Account — verify responseCode 200")]
        public async Task API12_DeleteAccount_ReturnsResponseCode200()
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
        }

        [TestMethod]
        [Description("API 12: DELETE METHOD To Delete User Account — verify response message 'Account deleted!'")]
        public async Task API12_DeleteAccount_ReturnsAccountDeletedMessage()
        {
            const string throwawayEmail = "playwright.lifecycle.delete.msg.test@example.com";
            const string throwawayPassword = "DeleteMsgTest1234!";

            // Clean up any previous leftover first
            await _api.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
            {
                { "email",    throwawayEmail },
                { "password", throwawayPassword }
            });

            // Create a fresh throwaway account
            await _api.PostFormAsync("/api/createAccount", new Dictionary<string, string>
            {
                { "name",          "Delete Msg Test" },
                { "email",         throwawayEmail },
                { "password",      throwawayPassword },
                { "title",         "Miss" },
                { "birth_date",    "2" },
                { "birth_month",   "2" },
                { "birth_year",    "1999" },
                { "firstname",     "DeleteMsg" },
                { "lastname",      "Test" },
                { "company",       "" },
                { "address1",      "2 Test Lane" },
                { "address2",      "" },
                { "country",       "United Kingdom" },
                { "zipcode",       "W1A 0AX" },
                { "state",         "England" },
                { "city",          "London" },
                { "mobile_number", "07700900003" }
            });

            // Delete and assert the message
            var deleteResponse = await _api.DeleteFormAsync("/api/deleteAccount", new Dictionary<string, string>
            {
                { "email",    throwawayEmail },
                { "password", throwawayPassword }
            });

            var deleteBody = await ApiClient.DeserialiseAsync<ApiResponse>(deleteResponse);

            Assert.AreEqual("Account deleted!", deleteBody.Message,
                $"Expected 'Account deleted!' but got '{deleteBody.Message}'");
        }
    }
}