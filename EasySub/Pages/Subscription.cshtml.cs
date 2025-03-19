using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySub.Pages
{
    public class SubscriptionModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public SubscriptionModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string SubscriptionType { get; set; }
        [BindProperty] public int Duration { get; set; }
        public string Message { get; set; }
        [BindProperty] public decimal Price { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var subscriptionData = new
            {
                Type = SubscriptionType,
                DurationMonths = Duration,
                ClientEmail = Email,
                Status = "Pending",
                price = Price,
            };

            var json = JsonSerializer.Serialize(subscriptionData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7237/api/subscription/purchase", content);

            Message = response.IsSuccessStatusCode ? "Abonnement acheté avec succès !" : "Erreur lors de l'achat.";

            return Page();
        }
    }
}
