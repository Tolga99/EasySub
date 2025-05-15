using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;
using Stripe;
using API.Interfaces;
using Stripe.Checkout;

namespace API.Controllers
{
    [ApiController]
    [Route("api/webhook/stripe")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ISubscriptionService _subscriptionService;

        public StripeWebhookController(IConfiguration configuration, IEmailService emailService, ISubscriptionService subscriptionService)
        {
            _configuration = configuration;
            _emailService = emailService;
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var endpointSecret = _configuration["Stripe:WebhookSecret"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    endpointSecret
                );

                if (stripeEvent.Type == Stripe.EventTypes.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var subId = session.Metadata["subscriptionId"];
                    if (!string.IsNullOrEmpty(subId))
                    {
                        var success = await _subscriptionService.PurchaseSubscription(int.Parse(subId));

                        if (success == -1)
                            return StatusCode(500, "Une erreur est survenue lors de l'achat.");
                        _emailService.SendOrderEmailsAsync(success);
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Erreur Stripe : {e.Message}");
                return BadRequest();
            }
        }
    }

}
