using System;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly EasySubContext _context;

        public InvoiceService(EasySubContext context)
        {
            _context = context;
        }

        public async Task<Invoice> CreateInvoiceForSubscription(Subscription subscription)
        {
            var invoice = new Invoice
            {
                SubscriptionId = subscription.Id,
                Amount = subscription.SubscriptionPlan.Price, // 🔥 Récupération du prix du plan
                CreatedAt = DateTime.UtcNow
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }


    }
}
