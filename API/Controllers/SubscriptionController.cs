using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;
        public SubscriptionController(ISubscriptionService subscriptionService, IEmailService emailService, IAccountService accountService)
        {
            _subscriptionService = subscriptionService;
            _emailService = emailService;
            _accountService = accountService;
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseSubscription([FromBody] SubscriptionRequest request)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            ////var paymentStatus = request.IsPaid ? PaymentStatus.Paid : PaymentStatus.Pending;
            //var success = await _subscriptionService.PurchaseSubscription(request);

            //if (success == -1)
            //    return StatusCode(500, "Une erreur est survenue lors de l'achat.");
            //_emailService.SendOrderEmailsAsync(success);
            //return Ok(new { message = "Abonnement acheté avec succès!", subscriptionId = success });
            return Ok();
        }
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateSubscription([FromBody] int subscriptionId, string accountMail, string accountPass)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var paymentStatus = request.IsPaid ? PaymentStatus.Paid : PaymentStatus.Pending;
            var success = await _subscriptionService.ActivateSubscription(subscriptionId);

            if (!success)
                return StatusCode(500, "Une erreur est survenue lors de l'achat.");
            await _accountService.AddAndLinkAccount(new AccountRequest(subscriptionId, accountMail, accountPass));
            await _emailService.SendActivationEmailAsync(subscriptionId, accountMail, accountPass);
            return Ok(new { message = "Abonnement acheté avec succès!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptions();
            return Ok(subscriptions);
        }
    }
}
