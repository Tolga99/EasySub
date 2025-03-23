using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class AccountService : IAccountService
    {
        private readonly EasySubContext _context;

        public AccountService(EasySubContext context)
        {
            _context = context;
        }

        // 1. Ajouter un nouveau compte
        public async Task<Account> AddAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        // 2. Supprimer un compte
        public async Task<bool> DeleteAccount(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        // 3. Récupérer un compte spécifique
        public async Task<Account?> GetAccount(int accountId)
        {
            return await _context.Accounts
                .Include(a => a.Subscription)
                .FirstOrDefaultAsync(a => a.Id == accountId);
        }

        // 4. Récupérer tous les comptes
        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await _context.Accounts
                .Include(a => a.Subscription)
                .ToListAsync();
        }

        // 5. Modifier un compte (email, mot de passe, etc.)
        public async Task<bool> UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync() > 0;
        }

        // 6. Lier un abonnement à un compte
        public async Task<bool> AssignSubscriptionToAccount(int accountId, int subscriptionId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            var subscription = await _context.Subscriptions.FindAsync(subscriptionId);

            if (account == null || subscription == null) return false;

            // Mise à jour de la relation dans les deux sens
            account.SubscriptionId = subscriptionId;
            subscription.AccountId = accountId;

            return await _context.SaveChangesAsync() > 0;
        }


        // 7. Supprimer l'abonnement d'un compte
        public async Task<bool> UnassignSubscription(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            account.SubscriptionId = null;
            account.Subscription.ExpirationDate = null;
            return await _context.SaveChangesAsync() > 0;
        }

        //// 8. Récupérer l'abonnement lié à un compte
        //public async Task<Subscription?> GetSubscriptionForAccount(int accountId)
        //{
        //    return await _context.Subscriptions
        //        .FirstOrDefaultAsync(s => s.Account.Id  a.Id == accountId));
        //}

        // 9. Mettre à jour la date d'expiration
        public async Task<bool> UpdateExpirationDate(int accountId, DateTime newExpirationDate)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            account.Subscription.ExpirationDate = newExpirationDate;
            return await _context.SaveChangesAsync() > 0;
        }

        // 10. Récupérer les comptes expirés
        public async Task<IEnumerable<Account>> GetExpiredAccounts()
        {
            return await _context.Accounts
                .Where(a => a.Subscription.ExpirationDate != null && a.Subscription.ExpirationDate < DateTime.UtcNow)
                .ToListAsync();
        }

        // 11. Récupérer les comptes actifs
        public async Task<IEnumerable<Account>> GetActiveAccounts()
        {
            return await _context.Accounts
                .Where(a => a.Subscription.ExpirationDate != null && a.Subscription.ExpirationDate >= DateTime.UtcNow)
                .ToListAsync();
        }

        // 12. Vérifier si un email est déjà utilisé
        public async Task<bool> IsAccountAvailable(string email)
        {
            return !await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        // 13. Récupérer un compte via son email
        public async Task<Account?> FindByEmail(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        // 14. Récupérer les comptes avec un type d'abonnement spécifique
        public async Task<IEnumerable<Account>> GetAccountsBySubscriptionType(int subscriptionTypeId)
        {
            return await _context.Accounts
                .Where(a => a.Subscription != null && a.Subscription.SubscriptionPlan.SubscriptionTypeId == subscriptionTypeId)
                .ToListAsync();
        }

        // 15. Récupérer les comptes qui expirent bientôt
        public async Task<IEnumerable<Account>> GetAccountsExpiringSoon(int daysThreshold)
        {
            var targetDate = DateTime.UtcNow.AddDays(daysThreshold);
            return await _context.Accounts
                .Where(a => a.Subscription.ExpirationDate != null && a.Subscription.ExpirationDate < targetDate && a.Subscription.ExpirationDate > DateTime.UtcNow)
                .ToListAsync();
        }

        // 16. Modifier le mot de passe d'un compte
        public async Task<bool> ChangePassword(int accountId, string newPassword)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            account.Password = newPassword; // À chiffrer en prod !
            return await _context.SaveChangesAsync() > 0;
        }

        // 17. Vérifier les identifiants (authentification basique)
        public async Task<bool> Authenticate(string email, string password)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            return account != null && account.Password == password; // En prod, comparer avec un hash !
        }

        // 18. Verrouiller un compte
        public async Task<bool> LockAccount(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            account.Subscription.ExpirationDate = DateTime.UtcNow.AddYears(-100); // On met une date très ancienne
            return await _context.SaveChangesAsync() > 0;
        }

        // 19. Déverrouiller un compte
        public async Task<bool> UnlockAccount(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            account.Subscription.ExpirationDate = null;
            return await _context.SaveChangesAsync() > 0;
        }

        // 20. Récupérer les statistiques
        public async Task<int> GetTotalAccountsCount() => await _context.Accounts.CountAsync();
        public async Task<int> GetActiveAccountsCount() => await _context.Accounts.CountAsync(a => a.Subscription.ExpirationDate >= DateTime.UtcNow);
        public async Task<int> GetExpiredAccountsCount() => await _context.Accounts.CountAsync(a => a.Subscription.ExpirationDate < DateTime.UtcNow);
    }
}

