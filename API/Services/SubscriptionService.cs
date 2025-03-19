using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace API.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly EasySubContext _context;
        private readonly IEmailService _emailService;

        public SubscriptionService(EasySubContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<List<Subscription>> GetAllSubscriptions()
        {
            return await _context.Subscriptions.ToListAsync();
        }

        public async Task<Subscription?> CreateSubscription(Subscription subscription)
        {
            try
            {
                _context.Subscriptions.Add(subscription);
                await _context.SaveChangesAsync();
                return subscription;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'achat d'un abonnement: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> PurchaseSubscription(string email, string type, int duration, decimal price)
        {
            // 1️⃣ Créer l'abonnement en "Pending"
            var subscription = new Subscription
            {
                Type = type,
                DurationMonths = duration,
                ClientEmail = email,
                Status = SubscriptionStatus.Pending
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            // 2️⃣ Créer la facture liée à l'abonnement
            var invoice = new Invoice
            {
                Email = email,
                SubscriptionId = subscription.Id,
                Amount = price
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // 3️⃣ Envoyer un mail de notification
            string subject = "Nouvelle commande d'abonnement";
            string body = $"Un nouvel abonnement {type} ({duration} mois) a été acheté par {email}. Montant: {price}€";
            await _emailService.SendEmailAsync("sva.records.o@gmail.com", subject, body);

            return true;
        }
    }
}
