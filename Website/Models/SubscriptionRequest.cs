namespace Website.Models
{
    public class SubscriptionRequest
    {
        public string Email { get; set; } = string.Empty;  // Email de l'acheteur
        public int SubscriptionPlanId { get; set; }       // ID du plan choisi
        public bool IsPaid { get; set; }                  // Indique si l'achat est payé
    }
}
