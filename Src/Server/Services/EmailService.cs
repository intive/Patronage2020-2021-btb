using BTB.Application.Common;
using BTB.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

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
            MailMessage mail = new MailMessage(_configurator.CurrentConfig.login, to);
            mail.Subject = title;
            mail.Body = message;

            _client.Send(mail);
        }
    }

    internal sealed class EmailConfigurator
    {
        public EmailConfig CurrentConfig { get; private set; }

        public SmtpClient Configure(EmailConfig config)
        {
            SmtpClient client = new SmtpClient(config.smtpServer, config.port);
            client.Credentials = new NetworkCredential()
            {
                UserName = config.login,
                Password = config.password
            };
            client.EnableSsl = config.enableSsl;

            CurrentConfig = config;

            return client;
        }
    }
}
