using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionTypeController : Controller
    {
        private readonly ISubscriptionTypeService _subscriptionTypeService;

        public SubscriptionTypeController(ISubscriptionTypeService subscriptionTypeService)
        {
            _subscriptionTypeService = subscriptionTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await _subscriptionTypeService.GetAllTypes();
            return Ok(types);
        }
    }
}
