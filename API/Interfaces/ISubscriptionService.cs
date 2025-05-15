using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ISubscriptionService
    {
        Task<List<Subscription>> GetAllSubscriptions();
        Task<bool> ActivateSubscription(int subscriptionId);
        Task<Subscription> GetById(int id);
        Task<int> CheckoutSubscription(OrderRequestCart cart);
        Task<int> PurchaseSubscription(int subscriptionId);
    }
}
