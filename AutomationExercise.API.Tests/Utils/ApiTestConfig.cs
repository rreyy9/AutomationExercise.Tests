using Microsoft.Extensions.Configuration;

namespace AutomationExercise.API.Tests.Utils
{
    public class ApiTestConfig
    {
        public string BaseUrl { get; private init; } = string.Empty;
        public int Timeout { get; private init; }

        private ApiTestConfig() { }

        public static ApiTestConfig Load()
        {
            // DOTNET_ENVIRONMENT=ci activates appsettings.ci.json overrides in CI
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment.ToLower()}.json", optional: true, reloadOnChange: false)
                .Build();

            var section = configuration.GetSection("TestSettings");

            return new ApiTestConfig
            {
                BaseUrl = section["BaseUrl"] ?? throw new InvalidOperationException("TestSettings:BaseUrl is required in appsettings.json"),
                Timeout = int.TryParse(section["Timeout"], out var timeout) ? timeout : 30000
            };
        }
    }
}