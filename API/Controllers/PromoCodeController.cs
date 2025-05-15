using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromoCodeController : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidatePromoCode([FromQuery] string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Le code est requis.");

            var promo = await _promoCodeService.ValidateCodeAsync(code);

            if (promo == null)
                return NotFound("Code promo invalide ou expiré.");

            return Ok(new
            {
                promo.Code,
                promo.DiscountPercentage,
                promo.ExpirationDate
            });
        }
    }
}
