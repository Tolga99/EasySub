using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class OrderRequestCart
    {
        public int PlanId { get; set; }
        public string CurrencyCode { get; set; }
        public string? PromoCode { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Culture { get; set; } = string.Empty;
    }
}
