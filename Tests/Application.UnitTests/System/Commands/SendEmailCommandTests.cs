using Application.UnitTests.Common;
using BTB.Application.Common;
using BTB.Application.Common.Interfaces;
using BTB.Application.System.Commands.SendEmailCommand;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.System.Commands
{
    public class SendEmailCommandTests : CommandTestsBase
    {
        private class EmailConfigMock : IOptions<EmailConfig>
        {
            public string DefaultEmailAddress { get; set; }
            public EmailConfig Value => new EmailConfig()
            {
                Login = DefaultEmailAddress
            };
        }

        [Theory]
        [InlineData("email@email.com","default@default.com","title","content")]
        [InlineData(" ","default@default.com","title","content")]
        [InlineData("","default@default.com","title","content")]
        [InlineData(null,"default@default.com","title","content")]
        public async Task Handle_ShouldSendEmail_WhenRequestIsValid(string expectedTo, string defaultEmailAddress, string expectedTitle, string expectedContent)
        {
            var emailServiceMock = new Mock<IEmailService>();
            var emailKeeperMock = new Mock<IEmailKeeper>();
            var configMock = new EmailConfigMock() { DefaultEmailAddress = defaultEmailAddress };

            var command = new SendEmailCommand()
            {
                To = expectedTo,
                Title = expectedTitle,
                Content = expectedContent,
            };

            ILoggerFactory factory = new LoggerFactory();
            var sut = new SendEmailCommandHandler(emailServiceMock.Object, emailKeeperMock.Object, configMock, _context, factory.CreateLogger<SendEmailCommandHandler>());
            await sut.Handle(command, CancellationToken.None);

            if (string.IsNullOrWhiteSpace(expectedTo))
            {
                expectedTo = defaultEmailAddress;
            }
            emailServiceMock.Verify(mock => mock.Send(expectedTo, expectedTitle, expectedContent, _context.EmailTemplates.FirstOrDefault()));
        }
    }
}
