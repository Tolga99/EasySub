namespace API.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // À crypter plus tard

        // Clé étrangère vers Subscription (facultatif)
        public int? SubscriptionId { get; set; }

        // Navigation vers Subscription (relation 1-1)
        public Subscription? Subscription { get; set; }
    }

}
