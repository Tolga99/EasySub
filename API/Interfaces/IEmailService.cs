using API.Models;

namespace API.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendActivationEmailAsync(int subscriptionId, string accountEmail, string accountPassword);
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
        Task<bool> SendOrderEmailsAsync(int id);
    }
}
