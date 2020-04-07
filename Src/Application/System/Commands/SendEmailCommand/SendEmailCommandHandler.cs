using BTB.Application.Common;
using BTB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.SendEmailCommand
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand>
    {
        private readonly IEmailService _emailService;
        private readonly string _defaultEmailAddress;

        public SendEmailCommandHandler(IEmailService emailService, IOptions<EmailConfig> config)
        {
            _emailService = emailService;
            _defaultEmailAddress = config.Value.login;
        }

        public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.To))
            {
                request.To = _defaultEmailAddress;
            }

            _emailService.Send(request.To, request.Title, request.Content);
            return Unit.Value;
        }
    }
}
