using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using API.Models;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using Polly;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly ICurrencyService _currencyService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly string _secretKey;
        private readonly IConfiguration _configuration;
        public PaymentsController(IConfiguration configuration, IPromoCodeService promoCodeService, ISubscriptionPlanService subscriptionPlanService, ICurrencyService currencyService, ISubscriptionService subscriptionService)
        {
            _configuration = configuration;
            _secretKey = _configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _secretKey;
            _promoCodeService = promoCodeService;
            _subscriptionPlanService = subscriptionPlanService;
            _currencyService = currencyService;
            _subscriptionService = subscriptionService;
        }

        [HttpGet("success")]
        public async Task<IActionResult> GetSuccessData([FromQuery] string session_id)
        {
            var service = new SessionService();
            var session = await service.GetAsync(session_id);

            if (session?.Metadata == null || !session.Metadata.ContainsKey("subscriptionId"))
                return BadRequest("Session invalide");

            var subscriptionId = int.Parse(session.Metadata["subscriptionId"]);

            var subscription = await _subscriptionService.GetById(subscriptionId);

            if (subscription == null)
                return NotFound("Abonnement introuvable");

            return Ok(new
            {
                subscriptionId = subscription.Id,
                plan = subscription.SubscriptionPlan.ToString(),
                email = subscription.ClientEmail,
                price = subscription.SubscriptionPlan.Price
            });
        }
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] OrderRequestCart cart)
        {
            var plan = await _subscriptionPlanService.GetPlanById(cart.PlanId);
            // 1. Appliquer le code promo (en USD ici)
            decimal priceUSD = (await _promoCodeService.GetCalculatedPrice(plan, cart.PromoCode)) ?? throw new Exception("Le prix est null");

            // 2. Convertir
            var finalPrice = _currencyService.ConvertFromUSD(priceUSD, cart.CurrencyCode);

            // 3. Stripe attend un montant en centimes
            long montantStripe = (long)(finalPrice * 100);
            var sub = await _subscriptionService.CheckoutSubscription(cart);

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
        {
          new SessionLineItemOptions
          {
            PriceData = new SessionLineItemPriceDataOptions
            {
              UnitAmount = montantStripe,
              Currency = cart.CurrencyCode.ToLower(),
              ProductData = new SessionLineItemPriceDataProductDataOptions
              {
                Name = $"({sub}) - {plan.ToString()}",
              },
            },
            Quantity = 1,
          },
        },
                Mode = "payment",
                SuccessUrl = $"https://api.easysub.me/subscription/{plan.Brand.Name}?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"https://api.easysub.me/subscription/{plan.Brand.Name}?canceled=true",
                Metadata = new Dictionary<string, string>
                {
                    { "subscriptionId", sub.ToString() },
                    { "culture", cart.Culture ?? "en" }
                }
            };
            var service = new SessionService();
            Session session = service.Create(options);

            return Ok(new { sessionId = session.Id });
        }
    }
}
