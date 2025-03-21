
using EasySub.Interfaces;
using EasySub.Models;
using EasySub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EasySub.Pages;

public class IndexModel : PageModel
{
    private readonly IBrandService _brandService;

    public List<Brand> Brands { get; set; }

    public IndexModel(IBrandService brandService)
    {
        _brandService = brandService;
    }

    public async Task OnGetAsync()
    {
        Brands = await _brandService.GetBrandsAsync();
    }
}
