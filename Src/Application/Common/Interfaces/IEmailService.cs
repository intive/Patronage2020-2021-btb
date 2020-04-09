using BTB.Domain.Entities;

namespace BTB.Application.Common.Interfaces
{
    public interface IEmailService
    {
        public void Send(string to, string title, string message);
        public void Send(string to, string title, string message, EmailTemplate emailTemplate);
    }
}
