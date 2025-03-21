using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : Controller
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var plans = await _subscriptionPlanService.GetAllPlans();
            return Ok(plans);
        }
        [HttpGet("{brandId}")]
        public async Task<IActionResult> GetByBrandId(int brandId)
        {
            var plans = await _subscriptionPlanService.GetPlansByBrandId(brandId);
            if (!plans.Any())
            {
                return NotFound(new { message = "No subscription plans found for this brand" });
            }
            return Ok(plans);
        }
    }
}
