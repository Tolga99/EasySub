using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class BrandService : IBrandService
    {
        private readonly EasySubContext _context;

        public BrandService(EasySubContext context)
        {
            _context = context;
        }
        public async Task<List<Brand>> GetAllBrands()
        {
            return await _context.Brands.ToListAsync();
        }
        public async Task<Brand?> GetBrandById(int id)
        {
            return await _context.Brands.FindAsync(id);
        }
    }
}
