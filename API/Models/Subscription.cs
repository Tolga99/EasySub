using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ClientEmail { get; set; } = string.Empty;

        [Required]
        public int SubscriptionPlanId { get; set; } // 🔗 Clé étrangère vers `SubscriptionPlan`
        public SubscriptionPlan SubscriptionPlan { get; set; } = null!;

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        public DateTime? ExpirationDate { get; set; } // ✅ Ajouté pour indiquer la fin de l’abonnement
                                                      // Clé étrangère inverse (facultatif)
        public int AccountId { get; set; }

        // Navigation vers Account (relation 1-1)
        public Account Account { get; set; } = null!;
    }



    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed
    }
}
