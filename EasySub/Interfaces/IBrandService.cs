using EasySub.Models;

namespace EasySub.Interfaces
{
    public interface IBrandService
    {
        Task<Brand?> FindAsync(int id);
        Task<List<Brand>> GetBrandsAsync();
    }
}
