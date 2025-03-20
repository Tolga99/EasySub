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
            return await _context.Subscriptions.ToListAsync();
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

        // 🎯 Achat d'un abonnement avec gestion du paiement et de la facture
        public async Task<bool> PurchaseSubscription(string email, int subscriptionPlanId, PaymentStatus paymentStatus)
        {
            try
            {
                // 🔍 Vérifier si le plan d’abonnement existe
                var subscriptionPlan = await _context.SubscriptionPlans
                    .Include(sp => sp.Brand)
                    .Include(sp => sp.SubscriptionType)
                    .FirstOrDefaultAsync(sp => sp.Id == subscriptionPlanId);
                if (subscriptionPlan == null)
                    return false; // ❌ Plan introuvable

                // 1️⃣ Création de l'abonnement
                var subscription = new Subscription
                {
                    ClientEmail = email,
                    SubscriptionPlanId = subscriptionPlan.Id, // ✅ Lien avec le plan d’abonnement
                    PaymentStatus = paymentStatus
                };

                _context.Subscriptions.Add(subscription);
                await _context.SaveChangesAsync();

                // 2️⃣ Création de la facture SI l'abonnement est payé
                Invoice? invoice = null;
                if (paymentStatus == PaymentStatus.Paid)
                {
                    invoice = await _invoiceService.CreateInvoiceForSubscription(subscription);
                }

                // 3️⃣ Envoi d'un mail de confirmation
                string subject = "Nouvelle commande d'abonnement";
                string body = $"Un nouvel abonnement {subscriptionPlan.Brand.Name} - {subscriptionPlan.SubscriptionType.Name} ({subscriptionPlan.DurationMonths} mois) a été acheté par {email}. Montant: {subscriptionPlan.Price}€. Statut du paiement: {paymentStatus}";
                await _emailService.SendEmailAsync(email, subject, body);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'achat de l'abonnement: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> ActivateSubscription(int subscriptionId)
        {
            var subscription = await _context.Subscriptions
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
