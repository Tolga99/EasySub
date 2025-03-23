using EasySub.Interfaces;
using EasySub.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7237/api/subscriptionplan/{brandId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Erreur API : {response.StatusCode}");
                }

                return await response.Content.ReadFromJsonAsync<List<SubscriptionPlan>>() ?? new List<SubscriptionPlan>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des plans: {ex.Message}");
                return new List<SubscriptionPlan>();
            }
        }

        public async Task<decimal?> GetPriceAsync(string subscriptionType, int duration)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7237/api/subscriptionplan/price?subscriptionType={subscriptionType}&duration={duration}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var price = await response.Content.ReadFromJsonAsync<decimal?>();
                return price;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du prix: {ex.Message}");
                return null;
            }
        }
        public async Task<HttpResponseMessage> PurchaseSubscriptionAsync(string email, int planId, bool isPaid)
        {
            var request = new SubscriptionRequest
            {
                Email = email,
                SubscriptionPlanId = planId,
                IsPaid = isPaid
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync("https://localhost:7237/api/subscription/purchase", content);
        }


    }
}
