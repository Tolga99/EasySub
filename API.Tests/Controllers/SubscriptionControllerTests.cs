using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using API.Controllers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using API.Models;

namespace API.Tests.Controllers
{


    public class SubscriptionControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfBrands()
        {
            // Arrange (Prépare les données)
            var mockService = new Mock<IBrandService>();
            var brands = new List<Brand> { new Brand { Id = 1, Name = "Spotify" } };
            mockService.Setup(s => s.GetAllBrands()).ReturnsAsync(brands);

            var controller = new BrandController(mockService.Object);

            // Act (Exécute l'action)
            var result = await controller.GetAll();

            // Assert (Vérifie le résultat)
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBrands = Assert.IsType<List<Brand>>(okResult.Value);
            Assert.Single(returnedBrands);
            Assert.Equal("Spotify", returnedBrands[0].Name);
        }
    }

}
