using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class EasySubContext : DbContext
    {
        public EasySubContext(DbContextOptions<EasySubContext> options) : base(options) { }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Brand> Brands { get; set; }  // ✅ Ajout de Brand
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }  // ✅ Ajout de SubscriptionType

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 Configuration auto-incrémentation pour Subscription
            modelBuilder.Entity<Subscription>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            // 🔹 Configuration auto-incrémentation pour Invoice
            modelBuilder.Entity<Invoice>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            // 🔹 Configuration auto-incrémentation pour SubscriptionPlan
            modelBuilder.Entity<SubscriptionPlan>()
                .Property(sp => sp.Id)
                .ValueGeneratedOnAdd();

            // 🔹 Relation Subscription → SubscriptionPlan
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.SubscriptionPlan)
                .WithMany()
                .HasForeignKey(s => s.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.NoAction);

            // 🔹 Relation SubscriptionPlan → Brand
            modelBuilder.Entity<SubscriptionPlan>()
                .HasOne(sp => sp.Brand)
                .WithMany()
                .HasForeignKey(sp => sp.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Relation SubscriptionPlan → SubscriptionType
            modelBuilder.Entity<SubscriptionPlan>()
                .HasOne(sp => sp.SubscriptionType)
                .WithMany()
                .HasForeignKey(sp => sp.SubscriptionTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Brand>().HasData(
    new Brand { Id = 1, Name = "Netflix" },
    new Brand { Id = 2, Name = "Spotify" },
    new Brand { Id = 3, Name = "Disney+" },
    new Brand { Id = 4, Name = "PlayStation Plus" },
    new Brand { Id = 5, Name = "Amazon Prime" },
    new Brand { Id = 6, Name = "YouTube Premium" }
);

            modelBuilder.Entity<SubscriptionType>().HasData(
                new SubscriptionType { Id = 1, Name = "Standard" },
                new SubscriptionType { Id = 2, Name = "Premium" },
                new SubscriptionType { Id = 3, Name = "Family" },
                new SubscriptionType { Id = 4, Name = "Student" }
            );

            modelBuilder.Entity<SubscriptionPlan>().HasData(
                new SubscriptionPlan { Id = 1, BrandId = 1, SubscriptionTypeId = 1, DurationMonths = 1, Price = 7.99M },
                new SubscriptionPlan { Id = 2, BrandId = 1, SubscriptionTypeId = 1, DurationMonths = 3, Price = 21.99M },
                new SubscriptionPlan { Id = 3, BrandId = 1, SubscriptionTypeId = 1, DurationMonths = 12, Price = 79.99M },
                new SubscriptionPlan { Id = 4, BrandId = 1, SubscriptionTypeId = 2, DurationMonths = 1, Price = 12.99M },
                new SubscriptionPlan { Id = 5, BrandId = 1, SubscriptionTypeId = 2, DurationMonths = 3, Price = 35.99M },
                new SubscriptionPlan { Id = 6, BrandId = 1, SubscriptionTypeId = 2, DurationMonths = 12, Price = 139.99M }
            );
        }
    }
}
