using EasySub.Interfaces;
using EasySub.Models;
using EasySub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySub.Pages
{
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

        public string BrandName { get; set; }
        public List<SubscriptionPlan> Plans { get; set; }
        public List<string> SubscriptionTypes { get; set; }
        public List<int> Durations { get; set; }
        public decimal SelectedPrice { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var brand = await _brandService.FindAsync(BrandId);
            if (brand == null) return NotFound();

            BrandName = brand.Name;

            // Récupérer les plans depuis l’API
            Plans = await _planService.GetPlansByBrandIdAsync(BrandId);

            // Extraire les SubscriptionTypes et Durations
            SubscriptionTypes = Plans.Select(p => p.SubscriptionType.Name).Distinct().ToList();
            Durations = Plans.Select(p => p.DurationMonths).Distinct().ToList();

            return Page();
        }


        public IActionResult OnPostGetPrice(string subscriptionType, int duration)
        {
            var plan = Plans.FirstOrDefault(p => p.SubscriptionType.Name == subscriptionType && p.DurationMonths == duration);
            return new JsonResult(new { price = plan?.Price ?? 0 });
        }
    }
}
