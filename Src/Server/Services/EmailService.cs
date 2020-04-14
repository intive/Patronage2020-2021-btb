using BTB.Application.Common;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
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

        private SmtpClient _client { get; }

        static EmailService()
        {
            _configurator = new EmailConfigurator();
        }

        public EmailService(IOptions<EmailConfig> config)
        {
            _client = _configurator.Configure(config.Value);
        }

        public void Send(string to, string title, string message)
        {
            try
            {
                var mail = new MailMessage(_configurator.CurrentConfig.Login, to)
                {
                    Subject = title,
                    Body = message
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
            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(emailTemplate));
            }

            try
            {
                var builder = new StringBuilder();
                builder.Append(emailTemplate.Header);
                builder.Append(message);
                builder.Append(emailTemplate.Footer);

                var mail = new MailMessage(_configurator.CurrentConfig.Login, to)
                {
                    Subject = title,
                    Body = builder.ToString(),
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
            var client = new SmtpClient(config.SmtpServer, config.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = config.Login,
                    Password = config.Password
                },
                EnableSsl = config.EnableSsl
            };

            CurrentConfig = config;

            return client;
        }
    }
}
