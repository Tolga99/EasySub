using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IBrandService
    {
        Task<List<Brand>> GetAllBrands();
        Task<Brand?> GetBrandById(int id);
        Task<Brand?> GetBrandByName(string name);
        Task<List<Brand>> GetByCategory(string category);
    }
}
