using AutomationExercise.API.Tests.Clients;
using AutomationExercise.API.Tests.Utils;

namespace AutomationExercise.API.Tests
{
    [TestClass]
    public abstract class BaseApiTest
    {
        protected ApiClient Api { get; private set; } = null!;

        public TestContext TestContext { get; set; } = null!;

        [TestInitialize]
        public void BaseApiTestInitialize()
        {
            var config = ApiTestConfig.Load();
            Api = new ApiClient(config.BaseUrl, config.Timeout);
        }

        [TestCleanup]
        public void BaseApiTestCleanup()
        {
            Api?.Dispose();
        }
    }
}