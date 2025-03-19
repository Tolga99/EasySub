using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using API;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API.Tests
{


    public class SubscriptionControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SubscriptionControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetSubscriptions_ShouldReturn_OK()
        {
            // Act
            var response = await _client.GetAsync("/api/subscriptions");

            // Assert
            response.EnsureSuccessStatusCode(); // Vérifie que le statut est 200 OK
        }
    }

}
