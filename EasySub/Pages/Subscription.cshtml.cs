using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using EasySub.Interfaces;
using EasySub.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

public class SubscriptionModel : PageModel
{
    private readonly ISubscriptionPlanService _planService;
    private readonly IBrandService _brandService;

    public SubscriptionModel(ISubscriptionPlanService planService, IBrandService brandService)
    {
        _planService = planService;
        _brandService = brandService;
    }

    [BindProperty(SupportsGet = true)]
    public int BrandId { get; set; }
    [BindProperty]
    public string BrandName { get; set; }
    [BindProperty]
    public List<SubscriptionPlan> Plans { get; set; }
    [BindProperty]
    public List<string> SubscriptionTypes { get; set; }
    [BindProperty]
    public List<int> Durations { get; set; }

    [BindProperty]
    public string SelectedSubscriptionType { get; set; } = string.Empty;

    [BindProperty]
    public int SelectedDuration { get; set; }

    [BindProperty]
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required, EmailAddress]
    public string ConfirmEmail { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string PaymentMethod { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        var brand = await _brandService.FindAsync(BrandId);
        if (brand == null) return NotFound();

        BrandName = brand.Name;
        Plans = _planService.GetPlansByBrandIdAsync(BrandId).Result;
        SubscriptionTypes = Plans.Select(p => p.SubscriptionType.Name).Distinct().ToList();
        Durations = Plans.Select(p => p.DurationMonths).Distinct().ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostPurchaseAsync()
    {
        var selectedPlan = Plans.FirstOrDefault(p => p.SubscriptionType.Name == SelectedSubscriptionType && p.DurationMonths == SelectedDuration);
        if (selectedPlan == null)
        {
            return BadRequest("Le plan sélectionné n'existe pas.");
        }

        if (Email != ConfirmEmail)
        {
            ModelState.AddModelError("ConfirmEmail", "Les emails ne correspondent pas.");
            return Page();
        }

        var response = await _planService.PurchaseSubscriptionAsync(Email, selectedPlan.Id, true);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Erreur lors de l'achat");
        }

        return RedirectToPage("Success");
    }
}
