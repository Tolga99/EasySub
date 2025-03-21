using EasySub.Models;

namespace EasySub.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlan>> GetPlansByBrandIdAsync(int brandId);
    }

}
