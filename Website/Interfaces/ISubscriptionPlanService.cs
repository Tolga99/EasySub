using Website.Models;

namespace Website.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlan>> GetPlansByBrandIdAsync(int brandId);
        Task<decimal?> GetPriceAsync(string subscriptionType, int duration);
        Task<HttpResponseMessage> PurchaseSubscriptionAsync(string email, int planId, bool isPaid);
    }
}
