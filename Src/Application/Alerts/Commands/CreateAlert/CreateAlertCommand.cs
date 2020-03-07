using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Alerts.Commands.CreateAlert
{
    public class CreateAlertCommand : IRequest<string>
    {
        public string Symbol { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public double Value { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public class CreateAlertCommandHandler : IRequestHandler<CreateAlertCommand, string>
        {
            public async Task<string> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
            {
                return await Task.Run(() => request.Symbol);
            }
        }

    }
}
