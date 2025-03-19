using API.Data;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests
{
    public class SubscriptionServiceTests
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly EasySubContext _context;
        private readonly Mock<IEmailService> _mockEmailService;

        public SubscriptionServiceTests()
        {
            var options = new DbContextOptionsBuilder<EasySubContext>()
                .UseSqlServer("Server=localhost;Database=EasySubDB;Integrated Security=True;TrustServerCertificate=True;")
                .Options;

            _context = new EasySubContext(options);
            _mockEmailService = new Mock<IEmailService>(); // 🎭 Mock du service email

            _subscriptionService = new SubscriptionService(_context, _mockEmailService.Object);

            SeedDatabase(); // Remplir la DB avec des données fictives
        }
        [Fact]
        private void SeedDatabase()
        {
            // 🔄 Nettoyer la table avant le test
            //ClearDatabase();

            _context.Subscriptions.AddRange(new List<Subscription>
            {
                new Subscription {Type = "Netflix", DurationMonths = 3, ClientEmail = "beks@example.com", Status = SubscriptionStatus.Active },
                new Subscription {Type = "Spotify", DurationMonths = 6, ClientEmail = "donnie@example.com", Status = SubscriptionStatus.Pending }
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllSubscriptions_ShouldReturn_List()
        {
            // Act
            var result = await _subscriptionService.GetAllSubscriptions();

        }
        [Fact]
        public async Task CreateSubscription_ShouldAddToDatabase()
        {

            // Arrange
            var subscription = new Subscription
            {
                Type = "Netflix",
                DurationMonths = 3,
                ClientEmail = "kamil@example.com",
                Status = SubscriptionStatus.Active
            };

            await _subscriptionService.CreateSubscription(subscription);

            // Vérification
            var allSubscriptions = await _subscriptionService.GetAllSubscriptions();

            Console.WriteLine($"Nombre d'abonnements dans la DB : {allSubscriptions.Count}");
        }
        [Fact]
        public async Task CreateSubscription_ShouldSendEmail()
        {
            // Arrange
            var subscription = new Subscription
            {
                Type = "Disney+",
                DurationMonths = 3,
                ClientEmail = "test@example.com",
                Status = SubscriptionStatus.Pending
            };

            // Act
            await _subscriptionService.CreateSubscription(subscription);

            // ✅ Vérifier que l'abonnement a bien été ajouté
            var allSubscriptions = await _subscriptionService.GetAllSubscriptions();
            Assert.Contains(allSubscriptions, s => s.ClientEmail == "test@example.com");

            // 🚀 Vérifier si l'email a été envoyé (Regarder la console ou tester avec un mock)
        }
        private void ClearDatabase()
        {
            _context.Subscriptions.RemoveRange(_context.Subscriptions);
            _context.SaveChanges();
        }
    }
}
