using System.Text.Json;
using System.Text;
using WWWeasySub.Models;
using WWWeasySub.Interfaces;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WWWeasySub.Services
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
                var response = await _httpClient.GetAsync($"api/subscriptionplan/{brandId}");

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
                var response = await _httpClient.GetAsync($"api/subscriptionplan/price?subscriptionType={subscriptionType}&duration={duration}");

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
        public async Task<int?> PurchaseSubscriptionAsync(SubscriptionRequest subscription)
        {
            subscription.PaymentMethod = "Paypal";
            var response = await _httpClient.PostAsJsonAsync("api/subscription/purchase", subscription);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            if (result.TryGetProperty("subscriptionId", out var idElement))
                return idElement.GetInt32();

            return null;
        }

    }
}
