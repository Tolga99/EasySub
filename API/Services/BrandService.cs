using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
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
            return await _context.Brands.AsNoTracking().Where(a => a.Enabled == true).ToListAsync();
        }
        public async Task<Brand?> GetBrandById(int id)
        {
            return await _context.Brands.AsNoTracking().Where(a => a.Enabled == true).FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Brand?> GetBrandByName(string name)
        {
            return await _context.Brands.AsNoTracking().Where(a => a.Enabled == true).FirstOrDefaultAsync(a => a.Name.ToLower() == name.ToLower());
        }
        public async Task<List<Brand>> GetByCategory(string category)
        {
            if (category.ToLower() == "tendances")
            {
                // Ids ou noms des marques en tendance (à ajuster)
                //var trendingNames = new List<string> { "Netflix", "Spotify"};
                //var trendingBrands = await _context.Brands
                //    .Where(b => trendingNames.Contains(b.Name))
                //    .ToListAsync();
                var trendingBrands = await _context.Brands.AsNoTracking().Where(a => a.Enabled == true).Take(5)
                   .ToListAsync();
                return trendingBrands;
            }

            var brands = await _context.Brands
                .AsNoTracking().Where(b => b.Category.ToLower() == category.ToLower() && b.Enabled == true)
                .ToListAsync();

            return brands;
        }
    }
}
