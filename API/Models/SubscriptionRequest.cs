using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class SubscriptionRequest
    {
        [Required]
        public string OrderId { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Compare("Email", ErrorMessage = "Les emails ne correspondent pas.")]
        public string ConfirmEmail { get; set; } = string.Empty;

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        public int SubscriptionPlanId { get; set; }
    }


}
