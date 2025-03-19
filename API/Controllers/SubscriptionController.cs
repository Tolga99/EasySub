using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseSubscription([FromBody] Subscription subscription)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdSubscription = await _subscriptionService.CreateSubscription(subscription);

            if (createdSubscription == null)
                return StatusCode(500, "Une erreur est survenue lors de l'achat.");

            return Ok(new { message = "Abonnement acheté avec succès!", subscription = createdSubscription });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptions();
            return Ok(subscriptions);
        }
    }
}
