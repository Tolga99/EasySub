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

        public async Task<Invoice> CreateInvoice(int subscriptionId, int planId, string clientEmail, decimal amount)
        {
            var invoice = new Invoice
            {
                SubscriptionId = subscriptionId,
                SubscriptionPlanId = planId,
                ClientEmail = clientEmail,
                Amount = amount,
                CreatedAt = DateTime.UtcNow
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }


    }
}
