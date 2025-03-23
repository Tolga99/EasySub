using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly EasySubContext _context;
        private readonly IEmailService _emailService;
        private readonly IInvoiceService _invoiceService;

        public SubscriptionService(EasySubContext context, IEmailService emailService, IInvoiceService invoiceService)
        {
            _context = context;
            _emailService = emailService;
            _invoiceService = invoiceService;
        }

        // 🎯 Récupérer toutes les subscriptions
        public async Task<List<Subscription>> GetAllSubscriptions()
        {
            return await _context.Subscriptions.AsNoTracking().ToListAsync();
        }

        // 🎯 Création d'un abonnement simple (Sans gestion du paiement)
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
                Console.WriteLine($"Erreur lors de la création de l'abonnement: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> PurchaseSubscription(string email, int planId, PaymentStatus paymentStatus)
        {
            var plan = await _context.SubscriptionPlans.AsNoTracking().FirstOrDefaultAsync(a => a.Id == planId);
            if (plan == null)
                return false;

            // 📌 Création de la souscription
            var subscription = new Subscription
            {
                ClientEmail = email,
                SubscriptionPlanId = planId,
                PaymentStatus = paymentStatus,
                ExpirationDate = DateTime.UtcNow.AddMonths(plan.DurationMonths) // ✅ Gestion expiration
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync(); // 🔄 Sauvegarde la souscription pour obtenir l'ID

            // 📌 Création de la facture après validation du paiement
            if (paymentStatus == PaymentStatus.Paid)
            {
                await _invoiceService.CreateInvoice(subscription.Id, planId, email, plan.Price);
            }

            return true;
        }


        public async Task<bool> ActivateSubscription(int subscriptionId)
        {
            var subscription = await _context.Subscriptions.AsNoTracking()
                .Include(s => s.SubscriptionPlan) // 🔥 On inclut le plan d'abonnement
                .FirstOrDefaultAsync(s => s.Id == subscriptionId);

            if (subscription == null || subscription.PaymentStatus != PaymentStatus.Paid)
                return false;

            // ✅ On récupère DurationMonths via SubscriptionPlan
            subscription.ExpirationDate = DateTime.UtcNow.AddMonths(subscription.SubscriptionPlan.DurationMonths);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
