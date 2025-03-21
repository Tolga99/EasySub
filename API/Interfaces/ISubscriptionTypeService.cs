using API.Models;

namespace API.Interfaces
{
    public interface ISubscriptionTypeService
    {
        Task<List<SubscriptionType>> GetAllTypes();
        Task<SubscriptionType?> GetTypeById(int id);
        Task<SubscriptionType> CreateType(SubscriptionType type);
        Task<bool> DeleteType(int id);
    }
}
