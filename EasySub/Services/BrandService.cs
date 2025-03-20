using EasySub.Models;
using System.Text.Json;

namespace EasySub.Services
{
    public class BrandService
    {
        private readonly HttpClient _httpClient;

        public BrandService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Brand>> GetBrandsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7237/api/brand");

                if (response.IsSuccessStatusCode)
                {
                    var brands = await response.Content.ReadFromJsonAsync<List<Brand>>();
                    return brands ?? new List<Brand>();
                }
                else
                {
                    Console.WriteLine($"Erreur API : {response.StatusCode}");
                    return new List<Brand>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de l'appel API : {ex.Message}");
                return new List<Brand>();
            }

        }
    }
}
