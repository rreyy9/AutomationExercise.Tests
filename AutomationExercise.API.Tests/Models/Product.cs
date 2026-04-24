using System.Text.Json.Serialization;

namespace AutomationExercise.API.Tests.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public string Price { get; set; } = string.Empty;

        [JsonPropertyName("brand")]
        public string Brand { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public ProductCategory? Category { get; set; }
    }

    public class ProductCategory
    {
        [JsonPropertyName("usertype")]
        public UserType? UserType { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
    }

    public class UserType
    {
        [JsonPropertyName("usertype")]
        public string Type { get; set; } = string.Empty;
    }

    public class Brand
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("brand")]
        public string Name { get; set; } = string.Empty;
    }
}