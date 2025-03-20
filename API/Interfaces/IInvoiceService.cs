using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface IInvoiceService
    {
        Task<Invoice?> CreateInvoiceForSubscription(Subscription subscription);
    }
}
