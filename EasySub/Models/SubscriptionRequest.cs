namespace EasySub.Models
{
    public class SubscriptionRequest
    {
        public string Type { get; set; }  // Exemple: "Premium"
        public int Duration { get; set; } // Exemple: 12 (mois)
        public string Email { get; set; } // Exemple: "client@example.com"
    }
}
