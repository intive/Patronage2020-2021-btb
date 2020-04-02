using BTB.Application.Alerts.Common;
using MediatR;

namespace BTB.Application.Alerts.Commands.CreateAlertCommand
{
    public class CreateAlertCommand : IRequest<AlertVm>
    {
        public string SymbolPair { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public double Value { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
