using Website.Interfaces;
using Website.Models;

namespace Website.Services
{
    public class BrandService : IBrandService
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
        public async Task<Brand?> FindAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7237/api/brand/" + id);

                if (response.IsSuccessStatusCode)
                {
                    var brand = await response.Content.ReadFromJsonAsync<Brand>();
                    return brand;
                }
                else
                {
                    Console.WriteLine($"Erreur API : {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de l'appel API : {ex.Message}");
                return null;
            }
        }
        public async Task<Brand?> GetByName(string name)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7237/api/brand/name/" + name);

                if (response.IsSuccessStatusCode)
                {
                    var brand = await response.Content.ReadFromJsonAsync<Brand>();
                    return brand;
                }
                else
                {
                    Console.WriteLine($"Erreur API : {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception lors de l'appel API : {ex.Message}");
                return null;
            }
        }
        public async Task<List<Brand>> GetBrandsByCategoryAsync(string category)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7237/api/brand/category/{category}");

            if (response.IsSuccessStatusCode)
            {
                var brands = await response.Content.ReadFromJsonAsync<List<Brand>>();

                var brandList = brands?.Select(b => new Brand
                {
                    Id = b.Id,
                    Name = b.Name,
                    Category = b.Category,
                    ImageData = b.ImageData
                }).ToList();

                return brandList ?? new List<Brand>();
            }

            return new List<Brand>();
        }
    }
}
