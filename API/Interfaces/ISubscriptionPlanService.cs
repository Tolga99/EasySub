using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlan>> GetAllPlans();
        Task<SubscriptionPlan?> GetPlanById(int id);
        Task<SubscriptionPlan> CreatePlan(SubscriptionPlan plan);
        Task<bool> DeletePlan(int id);
    }
}
