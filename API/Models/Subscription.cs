using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Subscription
    {
        [Key] // Définit l'ID comme clé primaire
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incrémentation
        public int Id { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty; // Exemple: "Netflix", "Spotify"

        [Required]
        [Range(1, 24, ErrorMessage = "Duration must be between 1 and 24 months.")]
        public int DurationMonths { get; set; } // 1, 3, 12 mois

        [Required]
        [EmailAddress]
        public string ClientEmail { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(SubscriptionStatus))]
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Pending;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ExpirationDate => CreatedAt.AddMonths(DurationMonths);
    }

    public enum SubscriptionStatus
    {
        Active,
        Pending,
        Paid,
        Canceled
    }
}
