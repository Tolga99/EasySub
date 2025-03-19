using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class EasySubContext : DbContext
    {
        public EasySubContext(DbContextOptions<EasySubContext> options) : base(options) { }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration auto-incrémentation pour Subscription
            modelBuilder.Entity<Subscription>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            // Configuration auto-incrémentation pour Invoice
            modelBuilder.Entity<Invoice>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
