using BTB.Application.Alerts.Common;
using MediatR;

namespace BTB.Application.Alerts.Commands.UpdateAlertCommand
{
    public class UpdateAlertCommand : AlertRequestBase, IRequest
    {
        public int Id { get; set; }
    }
}
