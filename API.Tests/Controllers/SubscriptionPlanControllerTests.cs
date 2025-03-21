using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace API.Tests.Controllers
{
    public class SubscriptionPlanControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SubscriptionPlanControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPlans_ShouldReturn_OK()
        {
            // Act
            var response = await _client.GetAsync("/api/subscriptionplan");

            // Assert
            response.EnsureSuccessStatusCode(); // Vérifie que le statut est 200 OK
        }
    }
}
