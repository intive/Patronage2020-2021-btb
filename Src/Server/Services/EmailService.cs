using BTB.Application.Common;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BTB.Server.Services
{
    public class EmailService : IEmailService
    {
        private static EmailConfigurator _configurator;

        private readonly SmtpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        static EmailService()
        {
            _configurator = new EmailConfigurator();
        }

        public EmailService(IOptions<EmailConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            _client = _configurator.Configure(config.Value);
            _httpContextAccessor = httpContextAccessor;
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
            }
            catch (Exception e)
            { 
                // TODO: Log exception
                Console.WriteLine(e);
            }
        }

        public void Send(string to, string title, string message, EmailTemplate emailTemplate)
        {
            var domainUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(emailTemplate));
            }

            try
            {
                string mailMessage = emailTemplate.Content.Replace("[DOMAIN_URL]", domainUrl).Replace("[MESSAGE]", message);

                var mail = new MailMessage(_configurator.CurrentConfig.Login, to)
                {
                    Subject = title,
                    Body = mailMessage,
                    IsBodyHtml = true
                };

                _client.Send(mail);
            }
            catch (Exception e)
            {
                // TODO: Log exception
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
