using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Subscription = API.Models.Subscription;

namespace API.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly EasySubContext _context;
        private readonly IEmailService _emailService;
        private readonly IInvoiceService _invoiceService;
        private readonly IPromoCodeService _promoCodeService;
        private readonly ICurrencyService _currencyService;
        public SubscriptionService(EasySubContext context, IEmailService emailService, IInvoiceService invoiceService, IPromoCodeService promoCodeService, ICurrencyService currencyService)
        {
            _context = context;
            _emailService = emailService;
            _invoiceService = invoiceService;
            _promoCodeService = promoCodeService;
            _currencyService = currencyService;
        }

        // 🎯 Récupérer toutes les subscriptions
        public async Task<List<Subscription>> GetAllSubscriptions()
        {
            return await _context.Subscriptions.AsNoTracking().Include(a => a.SubscriptionPlan).ToListAsync();
        }
        public async Task<Subscription> GetById(int id)
        {
            return await _context.Subscriptions.AsNoTracking().Include(a => a.SubscriptionPlan).Include(a => a.SubscriptionPlan.Brand).Include(a => a.SubscriptionPlan.SubscriptionType).FirstOrDefaultAsync(a => a.Id == id);
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

        public async Task<int> PurchaseSubscription(int subscriptionId)
        {
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.Id == subscriptionId);

            if (subscription == null)
                return -1;

            subscription.PaymentStatus = PaymentStatus.Paid;
            //subscription.PaidAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return subscriptionId;
        }

        public async Task<int> CheckoutSubscription(OrderRequestCart cart)
        {
            var plan = await _context.SubscriptionPlans.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == cart.PlanId);

            if (plan == null)
                return -1;
            decimal priceUSD = (await _promoCodeService.GetCalculatedPrice(plan, cart.PromoCode)) ?? throw new Exception("Le prix est null");

            // 2. Convertir en fonction de la devise demandée
            var finalPrice = _currencyService.ConvertFromUSD(priceUSD, cart.CurrencyCode);
            // ⚠️ Démarre une transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var subscription = new Subscription
                {
                    ClientEmail = cart.Email,
                    SubscriptionPlanId = cart.PlanId,
                    PaymentStatus = PaymentStatus.Pending
                };

                _context.Subscriptions.Add(subscription);
                await _context.SaveChangesAsync();

                await _invoiceService.CreateInvoice(subscription.Id, subscription.SubscriptionPlanId, subscription.ClientEmail, finalPrice);

                await transaction.CommitAsync(); // ✅ Tout est ok, on valide la transaction
                return subscription.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // ❌ En cas d’erreur, on annule tout
                Console.WriteLine(ex);
                return -1;
            }
        }
        public async Task<bool> ActivateSubscription(int subscriptionId)
        {
            var subscription = await _context.Subscriptions.AsNoTracking()
                .Include(s => s.SubscriptionPlan) // 🔥 On inclut le plan d'abonnement
                .FirstOrDefaultAsync(s => s.Id == subscriptionId);

            if (subscription == null)
                return false;

            // ✅ On récupère DurationMonths via SubscriptionPlan
            subscription.ExpirationDate = DateTime.UtcNow.AddMonths(subscription.SubscriptionPlan.DurationMonths);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
