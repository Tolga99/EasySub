using System.Net.Http.Json;
using WWWeasySub.Interfaces;

namespace WWWeasySub.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly HttpClient _http;

        public PromoCodeService(HttpClient http)
        {
            _http = http;
        }

        public async Task<PromoCodeResult?> ValidateCodeAsync(string code)
        {
            if (String.IsNullOrEmpty(code))
                return null;
            var response = await _http.GetAsync($"api/promocode/validate?code={Uri.EscapeDataString(code)}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<PromoCodeResult>();
        }
    }

    public class PromoCodeResult
    {
        public required string Code { get; set; }
        public required decimal DiscountPercentage { get; set; }
        public required DateTime ExpirationDate { get; set; }
    }
}
