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
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 Configuration auto-incrémentation pour Subscription
            modelBuilder.Entity<Subscription>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            // 🔹 Configuration auto-incrémentation pour Account
            modelBuilder.Entity<Account>()
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
        }
    }
}
