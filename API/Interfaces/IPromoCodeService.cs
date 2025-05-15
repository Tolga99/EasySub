using API.Models;

namespace API.Interfaces
{
    public interface IPromoCodeService
    {
        Task<decimal?> GetCalculatedPrice(SubscriptionPlan plan, string? code);
        Task<PromoCode?> ValidateCodeAsync(string code);
    }
}
