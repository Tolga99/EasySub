namespace API.Models
{
    public class SubscriptionRequest
    {
        public string Email { get; set; } = string.Empty;
        public int SubscriptionPlanId { get; set; } // 🔗 On passe maintenant l’ID du plan
        public bool IsPaid { get; set; }
    }


}
