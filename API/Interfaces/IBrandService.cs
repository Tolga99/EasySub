using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IBrandService
    {
        Task<List<Brand>> GetAllBrands();
    }
}
