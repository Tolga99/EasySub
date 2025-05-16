using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class SubscriptionPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        [Required]
        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; } = null!;

        [Required]
        public int DurationMonths { get; set; } // Ex: 1, 3, 6, 12

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; } // Prix de vente
        public bool? Enabled { get; set; }
        public override string ToString()
        {
            return $"{Brand?.Name} - {SubscriptionType?.Name} ({DurationMonths})";
        }

    }

}
