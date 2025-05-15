using Website.Models;

namespace Website.Interfaces
{
    public interface IBrandService
    {
        Task<Brand?> FindAsync(int id);
        Task<List<Brand>> GetBrandsAsync();
        Task<List<Brand>> GetBrandsByCategoryAsync(string category);
        Task<Brand?> GetByName(string name);
    }
}
