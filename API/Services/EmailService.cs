using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer<Emails> _localizer;

        public EmailService(IConfiguration configuration, IServiceProvider serviceProvider, IStringLocalizer<Emails> localizer)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _localizer = localizer;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                string smtpServer = _configuration["EmailSettings:SmtpServer"];
                int port = int.Parse(_configuration["EmailSettings:Port"]);
                string username = _configuration["EmailSettings:Username"];
                string password = _configuration["EmailSettings:Password"];
                string fromEmail = _configuration["EmailSettings:FromEmail"] ?? username;

                using var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi de l'email : {ex.Message}");
                return false;
            }
        }
        public async Task<bool> SendOrderEmailsAsync(int id)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var subscriptionService = scope.ServiceProvider.GetRequiredService<ISubscriptionService>();

                    var subscription = await subscriptionService.GetById(id);
                    string supportEmail = "sva.records.o@gmail.com";

                    // 1. Email pour SVA
                    string internalSubject = $"Nouvelle commande {subscription.Id} : {subscription.SubscriptionPlan.ToString()}";
                    string internalBody = $"Un client ({subscription.ClientEmail}) a commandé un abonnement {subscription.SubscriptionPlan.ToString()}.\n" +
                                          $"Merci de préparer la commande et de la livrer dans les 24 à 48h.";

                    var internalSend = await SendEmailAsync(supportEmail, internalSubject, internalBody);

                    // 2. Email pour le CLIENT
                    //string clientSubject = $"Confirmation de commande {subscription.Id} - {subscription.SubscriptionPlan.Brand.Name + " " + subscription.SubscriptionPlan.SubscriptionType.Name + " (" + subscription.SubscriptionPlan.DurationMonths + ")"}";
                    //string clientBody = $"Bonjour,\n\n" +
                    //                    $"Nous avons bien reçu votre commande pour un abonnement {subscription.SubscriptionPlan.Brand.Name + " " + subscription.SubscriptionPlan.SubscriptionType.Name + " (" + subscription.SubscriptionPlan.DurationMonths + ")"}.\n" +
                    //                    $"Vous recevrez vos identifiants dans un délai de 24 à 48h.\n\n" +
                    //                    $"Pour toute question, vous pouvez nous contacter à l'adresse : {supportEmail}\n\n" +
                    //                    $"Merci pour votre confiance.\n\n" +
                    //                    $"L’équipe Subeasy.";
                    string clientSubject = _localizer["ClientEmailSubject", subscription.Id, subscription.SubscriptionPlan.ToString()];
                    string clientBody = _localizer["ClientEmailBody", subscription.SubscriptionPlan.ToString(), supportEmail];
                    var clientSend = await SendEmailAsync(subscription.ClientEmail, clientSubject, clientBody);

                    return internalSend && clientSend;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi des emails de commande : {ex.Message}");
                return false;
            }
        }
        public async Task<bool> SendActivationEmailAsync(int subscriptionId, string accountEmail, string accountPassword)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var subscriptionService = scope.ServiceProvider.GetRequiredService<ISubscriptionService>();
                    var subscription = await subscriptionService.GetById(subscriptionId);

                    string supportEmail = "sva.records.o@gmail.com";
                    string subject = _localizer["SubscriptionActivatedSubject",
                        subscription.Id,
                        $"{subscription.SubscriptionPlan.Brand.Name} {subscription.SubscriptionPlan.SubscriptionType.Name} ({subscription.SubscriptionPlan.DurationMonths})"
                    ];
                    string body = _localizer["SubscriptionActivatedBody",
                        $"{subscription.SubscriptionPlan.Brand.Name} {subscription.SubscriptionPlan.SubscriptionType.Name} ({subscription.SubscriptionPlan.DurationMonths})",
                        accountEmail,
                        accountPassword,
                        supportEmail
                    ];
                    return await SendEmailAsync(subscription.ClientEmail, subject, body);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi de l'email d'activation : {ex.Message}");
                return false;
            }
        }

    }
}
