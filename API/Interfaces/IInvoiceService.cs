using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IInvoiceService
    {
        Task<Invoice> CreateInvoice(int subscriptionId, int planId, string clientEmail, decimal amount);
    }
}
