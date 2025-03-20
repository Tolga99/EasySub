using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ISubscriptionService
    {
        Task<List<Subscription>> GetAllSubscriptions();
        Task<bool> PurchaseSubscription(string email, int subscriptionPlanId, PaymentStatus paymentStatus);
        Task<bool> ActivateSubscription(int subscriptionId);
    }
}
