using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using API.Services;
using API.Data;
using API.Models;
using API.Interfaces;
using System.Linq;

namespace API.Tests
{
    public class SubscriptionServiceTests : IDisposable
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly EasySubContext _context;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IInvoiceService> _mockInvoiceService;

        public SubscriptionServiceTests()
        {
            var options = new DbContextOptionsBuilder<EasySubContext>()
                .UseSqlServer("Server=localhost;Database=EasySubDB;Integrated Security=True;TrustServerCertificate=True;")
                .Options;

            _context = new EasySubContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            _mockEmailService = new Mock<IEmailService>();
            _mockInvoiceService = new Mock<IInvoiceService>();

            _subscriptionService = new SubscriptionService(_context, _mockEmailService.Object, _mockInvoiceService.Object);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var netflixBrand = new Brand { Name = "Netflix" };
            var spotifyBrand = new Brand { Name = "Spotify" };

            _context.Brands.AddRange(netflixBrand, spotifyBrand);
            _context.SaveChanges();

            var standardType = new SubscriptionType {  Name = "Standard" };
            var premiumType = new SubscriptionType { Name = "Premium" };

            _context.SubscriptionTypes.AddRange(standardType, premiumType);
            _context.SaveChanges();

            var netflixPlan = new SubscriptionPlan { BrandId = 1, SubscriptionTypeId = 2, DurationMonths = 1, Price = 10.99m };
            var spotifyPlan = new SubscriptionPlan { BrandId = 2, SubscriptionTypeId = 1, DurationMonths = 3, Price = 7.99m };

            _context.SubscriptionPlans.AddRange(netflixPlan, spotifyPlan);
            _context.SaveChanges();
        }

        [Fact]
        public async Task PurchaseSubscription_ShouldCreateSubscription()
        {
            // Arrange
            string testEmail = "test@example.com";
            int netflixPlanId = 1;
            PaymentStatus paymentStatus = PaymentStatus.Paid;

            var netflixPlan = _context.SubscriptionPlans.FirstOrDefault(p => p.Id == netflixPlanId);
            Assert.NotNull(netflixPlan);

            // Act
            var result = await _subscriptionService.PurchaseSubscription(testEmail, netflixPlanId, paymentStatus);

            // Assert
            Assert.True(result); // Vérifie que l'achat a bien été effectué

            var subscription = _context.Subscriptions.FirstOrDefault(s => s.SubscriptionPlanId == netflixPlanId);
            Assert.NotNull(subscription);
            Assert.Equal(netflixPlanId, subscription.SubscriptionPlanId);
        }

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Dispose();
        }
    }
}
