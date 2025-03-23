using API.Models;

namespace API.Interfaces
{
    public interface IAccountService
    {
        Task<Account> AddAccount(Account account);
        Task<bool> DeleteAccount(int accountId);
        Task<Account?> GetAccount(int accountId);
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<bool> UpdateAccount(Account account);
        Task<bool> AssignSubscriptionToAccount(int accountId, int subscriptionId);
        Task<bool> UnassignSubscription(int accountId);
        Task<bool> UpdateExpirationDate(int accountId, DateTime newExpirationDate);
        Task<IEnumerable<Account>> GetExpiredAccounts();
        Task<IEnumerable<Account>> GetActiveAccounts();
        Task<bool> IsAccountAvailable(string email);
        Task<Account?> FindByEmail(string email);
        Task<IEnumerable<Account>> GetAccountsBySubscriptionType(int subscriptionTypeId);
        Task<IEnumerable<Account>> GetAccountsExpiringSoon(int daysThreshold);
        Task<bool> ChangePassword(int accountId, string newPassword);
        Task<bool> Authenticate(string email, string password);
        Task<bool> LockAccount(int accountId);
        Task<bool> UnlockAccount(int accountId);
        Task<int> GetTotalAccountsCount();
        Task<int> GetActiveAccountsCount();
        Task<int> GetExpiredAccountsCount();
    }
}
