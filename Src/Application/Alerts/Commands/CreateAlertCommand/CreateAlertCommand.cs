using BTB.Application.Alerts.Common;
using BTB.Domain.ValueObjects;
using MediatR;

namespace BTB.Application.Alerts.Commands.CreateAlertCommand
{
    public class CreateAlertCommand : AlertRequestBase, IRequest<AlertVO>
    {
    }
}
