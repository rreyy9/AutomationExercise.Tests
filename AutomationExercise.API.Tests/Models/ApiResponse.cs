using System.Text.Json.Serialization;

namespace AutomationExercise.API.Tests.Models
{
    /// <summary>
    /// Base envelope returned by every Automation Exercise API endpoint.
    /// NOTE: The API always returns HTTP 200, even for errors — responseCode
    /// in the body is the authoritative status indicator, not the HTTP status code.
    /// </summary>
    public class ApiResponse
    {
        [JsonPropertyName("responseCode")]
        public int ResponseCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response envelope for endpoints that return a product list.
    /// </summary>
    public class ApiResponseWithProducts : ApiResponse
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; } = new();
    }

    /// <summary>
    /// Response envelope for endpoints that return a brand list.
    /// </summary>
    public class ApiResponseWithBrands : ApiResponse
    {
        [JsonPropertyName("brands")]
        public List<Brand> Brands { get; set; } = new();
    }

    /// <summary>
    /// Response envelope for the getUserDetailByEmail endpoint.
    /// </summary>
    public class ApiResponseWithUser : ApiResponse
    {
        [JsonPropertyName("user")]
        public UserDetail? User { get; set; }
    }
}