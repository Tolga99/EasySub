using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // 1. Ajouter un compte
        [HttpPost("add")]
        public async Task<IActionResult> AddAccount([FromBody] Account account)
        {
            if (account == null) return BadRequest("Données invalides.");

            var createdAccount = await _accountService.AddAccount(account);
            return Ok(createdAccount);
        }
        [HttpPost("addandlink")]
        public async Task<IActionResult> AddAndLinkAccount([FromBody] AccountRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Données invalides.");

            var account = new Account
            {
                Email = request.Email,
                Password = request.Password // TODO: Ajouter un hashage plus tard
            };

            var createdAccountId = await _accountService.AddAccount(account);
            await _accountService.AssignSubscriptionToAccount(createdAccountId, request.SubscriptionId);

            return Ok(createdAccountId);
        }

        // 2. Supprimer un compte
        [HttpDelete("delete/{accountId}")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var result = await _accountService.DeleteAccount(accountId);
            if (!result) return NotFound("Compte non trouvé.");

            return NoContent(); // 204 - Suppression réussie
        }

        // 3. Récupérer un compte spécifique
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(int accountId)
        {
            var account = await _accountService.GetAccount(accountId);
            if (account == null) return NotFound("Compte non trouvé.");

            return Ok(account);
        }

        // 4. Récupérer tous les comptes
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccounts();
            return Ok(accounts);
        }
    }
}
