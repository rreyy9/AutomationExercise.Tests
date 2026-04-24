namespace AutomationExercise.API.Tests.Clients
{
    /// <summary>
    /// Thin typed wrapper around HttpClient for the Automation Exercise API.
    ///
    /// Design notes:
    /// - All POST/PUT/DELETE bodies use application/x-www-form-urlencoded (not JSON).
    ///   The API does not accept JSON request bodies.
    /// - Returns HttpResponseMessage so tests can inspect status code and body
    ///   independently — keeping the client free of assertion logic.
    /// - The API always returns HTTP 200 regardless of error state.
    ///   Tests must inspect responseCode in the body, not the HTTP status code.
    /// </summary>
    public class ApiClient : IDisposable
    {
        private readonly HttpClient _http;

        public ApiClient(string baseUrl, int timeoutMs = 30000)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromMilliseconds(timeoutMs)
            };
        }

        // ── GET ───────────────────────────────────────────────────────────────────

        public Task<HttpResponseMessage> GetAsync(string path)
            => _http.GetAsync(path);

        // ── POST (form-encoded) ───────────────────────────────────────────────────

        public Task<HttpResponseMessage> PostFormAsync(string path, Dictionary<string, string> fields)
            => _http.PostAsync(path, new FormUrlEncodedContent(fields));

        // ── PUT (form-encoded) ────────────────────────────────────────────────────

        public Task<HttpResponseMessage> PutFormAsync(string path, Dictionary<string, string> fields)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, path)
            {
                Content = new FormUrlEncodedContent(fields)
            };
            return _http.SendAsync(request);
        }

        // ── DELETE (form-encoded) ─────────────────────────────────────────────────

        public Task<HttpResponseMessage> DeleteFormAsync(string path, Dictionary<string, string> fields)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, path)
            {
                Content = new FormUrlEncodedContent(fields)
            };
            return _http.SendAsync(request);
        }

        // ── DELETE (no body) ──────────────────────────────────────────────────────

        public Task<HttpResponseMessage> DeleteAsync(string path)
            => _http.DeleteAsync(path);

        // ── Response helpers ──────────────────────────────────────────────────────

        /// <summary>
        /// Reads the response body and deserialises it to T.
        /// Caller is responsible for disposing the HttpResponseMessage.
        /// </summary>
        public static async Task<T> DeserialiseAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException($"Failed to deserialise response to {typeof(T).Name}. Body: {json}");
        }

        public void Dispose() => _http.Dispose();
    }
}