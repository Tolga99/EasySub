namespace API.Models
{
    public class AccountRequest
    {
        public AccountRequest(int subId, string mail, string password) 
        {
            SubscriptionId = subId;
            Email = mail;
            Password = password;
        }
        public int SubscriptionId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // À crypter plus tard
    }
}
