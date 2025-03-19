using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ISubscriptionService
    {
        Task<Subscription?> CreateSubscription(Subscription subscription);
        Task<List<Subscription>> GetAllSubscriptions();
        Task<bool> PurchaseSubscription(string email, string type, int duration, decimal price);
    }
}
