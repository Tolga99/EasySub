using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SubscriptionId { get; set; } // 🔗 Lien vers Subscription
        public Subscription Subscription { get; set; } = null!;

        [Required]
        public int SubscriptionPlanId { get; set; } // 🔗 Lien direct vers le plan acheté
        public SubscriptionPlan SubscriptionPlan { get; set; } = null!;

        [Required]
        public string ClientEmail { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")] // 🔧 Définit la précision et l'échelle
        public decimal Amount { get; set; }


        public DateTime CreatedAt { get; set; }
    }


}
