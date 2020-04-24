using BTB.Application.Common;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;

namespace BTB.Server.Services
{
    public class EmailService : IEmailService
    {
        private static EmailConfigurator _configurator;

        private readonly SmtpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _domainUrl;
        private ILogger _logger;
        private const string DefaultDomainUrl = "https://dev-patronage-btb.azurewebsites.net/";

        static EmailService()
        {
            _configurator = new EmailConfigurator();
        }

        public EmailService(IOptions<EmailConfig> config, IHttpContextAccessor httpContextAccessor, ILogger<EmailService> logger)
        {
            _client = _configurator.Configure(config.Value);
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            if (httpContextAccessor.HttpContext == null)
            {
                _domainUrl = DefaultDomainUrl;
            }
            else
            {
                _domainUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
            }
        }

        public void Send(string to, string title, string message)
        {
            try
            {
                var mail = new MailMessage(_configurator.CurrentConfig.Login, to)
                {
                    Subject = title,
                    Body = message,
                    IsBodyHtml = true
                };

                _client.Send(mail);
                _logger.LogInformation($"Email was send to {to} was send to adress {title}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occured during attempt to send an email titled {title} to adress {to}");
                Console.WriteLine(e);
            }
        }

        public void Send(string to, string title, string message, EmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(emailTemplate));
            }

            try
            {
                string mailMessage = emailTemplate.Content.Replace("[DOMAIN_URL]", _domainUrl).Replace("[MESSAGE]", message);

                var mail = new MailMessage(_configurator.CurrentConfig.Login, to)
                {
                    Subject = title,
                    Body = mailMessage,
                    IsBodyHtml = true
                };

                _client.Send(mail);
                _logger.LogInformation($"Email was send to {to} was send to adress {title}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occured during attempt to send an email titled {title} to adress {to}");
                Console.WriteLine(e);
            }            
        }
    }

    internal sealed class EmailConfigurator
    {
        public EmailConfig CurrentConfig { get; private set; }

        public SmtpClient Configure(EmailConfig config)
        {
            var client = new SmtpClient
            {
                Host = config.SmtpServer,
                Port = config.Port,
                EnableSsl = config.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = config.Login,
                    Password = config.Password
                },
                Timeout = 20000
            };

            CurrentConfig = config;

            return client;
        }
    }
}
