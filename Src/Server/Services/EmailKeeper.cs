using BTB.Application.Common.Interfaces;
using BTB.Common;
using BTB.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BTB.Application.Common;
using Microsoft.Extensions.Options;

namespace BTB.Server.Services
{
    public class EmailKeeper : IEmailKeeper
    {
        private readonly IBTBDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IOptions<EmailKeeperConfig> _configuration;


        public EmailKeeper(IBTBDbContext context, IDateTime dateTime, IOptions<EmailKeeperConfig> configuration)
        {
            _context = context;
            _dateTime = dateTime;
            _configuration = configuration;
            InitEmailCounts();
        }

        private void InitEmailCounts()
        {
            var today = _dateTime.Today.ToString("d");
            EmailCount emailCount = _context.EmailCounts.SingleOrDefault(e => e.Id == today);
            if (emailCount == null)
            {
                _context.EmailCounts.Add(new EmailCount() { Id = today, DailyCount = 0 });
                _context.SaveChanges();
            }
            
        }

        public void IncrementEmailSent()
        {
            var today = _dateTime.Today.ToString("d");
            EmailCount emailCount = _context.EmailCounts.SingleOrDefault(e => e.Id == today);

            if (emailCount != null)
            {
                emailCount.DailyCount += 1;
                _context.SaveChanges();
            }
        }

        public bool CheckIfLimitHasBeenReached()
        {
            var today = _dateTime.Today.ToString("d");

            EmailCount emailCount = _context.EmailCounts.SingleOrDefault(e => e.Id == today);

            if(emailCount == null)
            {
                return false;
            }
            else
            {
                return emailCount.DailyCount >= _configuration.Value.EmailLimit;
            }
        }
    }
}
