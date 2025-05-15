

namespace WWWeasySub.Models
{
    public class SubscriptionPlan
    {

        public int Id { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; } = null!;

        public int DurationMonths { get; set; } // Ex: 1, 3, 6, 12
        public decimal Price { get; set; } // Prix de vente
        public override string ToString()
        {
            return SubscriptionType?.Name ?? "Unnamed Plan";
        }
    }
}
