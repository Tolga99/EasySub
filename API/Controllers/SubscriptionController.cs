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
        public async Task<IActionResult> PurchaseSubscription([FromBody] SubscriptionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var paymentStatus = request.IsPaid ? PaymentStatus.Paid : PaymentStatus.Pending;
            var success = await _subscriptionService.PurchaseSubscription(request.Email, request.SubscriptionPlanId, paymentStatus);

            if (!success)
                return StatusCode(500, "Une erreur est survenue lors de l'achat.");

            return Ok(new { message = "Abonnement acheté avec succès!" });
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptions();
            return Ok(subscriptions);
        }
    }
}
