using MediatR;

namespace BTB.Application.Alerts.Commands.DeleteAlertCommand
{
    public class DeleteAlertCommand : IRequest
    {
        public int Id { get; set; }
    }
}
