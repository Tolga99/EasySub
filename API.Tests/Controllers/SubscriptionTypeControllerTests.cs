using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace API.Tests.Controllers
{
    public class SubscriptionTypeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SubscriptionTypeControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTypes_ShouldReturn_OK()
        {
            // Act
            var response = await _client.GetAsync("/api/subscriptiontype");

            // Assert
            response.EnsureSuccessStatusCode(); // Vérifie que le statut est 200 OK
        }
    }
}
