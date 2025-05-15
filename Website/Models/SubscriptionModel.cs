using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class SubscriptionModel
    {
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
        public string SubscriptionType { get; set; } = string.Empty;

        [Required]
        public int Duration { get; set; }
    }
}
