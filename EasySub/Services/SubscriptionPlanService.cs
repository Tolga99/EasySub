using EasySub.Interfaces;
using EasySub.Models;

namespace EasySub.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly HttpClient _httpClient;

        public SubscriptionPlanService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SubscriptionPlan>> GetPlansByBrandIdAsync(int brandId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7237/api/subscriptionplan/{brandId}");

            if (response.IsSuccessStatusCode)
            {
                var plans = await response.Content.ReadFromJsonAsync<List<SubscriptionPlan>>();
                return plans ?? new List<SubscriptionPlan>();
            }

            return new List<SubscriptionPlan>();
        }
    }

}
