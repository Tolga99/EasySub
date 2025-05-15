using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly EasySubContext _context;

        public PromoCodeService(EasySubContext context)
        {
            _context = context;
        }

        public async Task<PromoCode?> ValidateCodeAsync(string code)
        {
            return await _context.PromoCodes
                .FirstOrDefaultAsync(p => p.Code == code &&
                                          p.IsActive &&
                                          p.ExpirationDate > DateTime.UtcNow);
        }
        public async Task<decimal?> GetCalculatedPrice(SubscriptionPlan plan, string? code)
        {
            if (plan == null) return null;

            // Si pas de code promo fourni, on retourne directement le prix original
            if (string.IsNullOrWhiteSpace(code))
                return plan.Price;

            var promo = await _context.PromoCodes
                .FirstOrDefaultAsync(p => p.Code == code &&
                                          p.IsActive &&
                                          p.ExpirationDate > DateTime.UtcNow);

            // Si pas de promo valide, retourne le prix original
            if (promo == null)
                return plan.Price;

            // Calcul du prix réduit
            var discount = plan.Price * (promo.DiscountPercentage / 100m);
            var finalPrice = plan.Price - discount;

            return finalPrice;
        }

    }
}
