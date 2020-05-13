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
        }

        public async Task IncrementEmailSentAsync(CancellationToken cancellationtoken)
        {
            var today = _dateTime.Today.ToString("d");

            EmailCount emailCount = _context.EmailCounts.SingleOrDefault(e => e.Id == today);

            if(emailCount == null)
            {
                await _context.EmailCounts.AddAsync(new EmailCount() { Id = today, DailyCount = 1 });
                await _context.SaveChangesAsync(cancellationtoken);
            }
            else
            {
                emailCount.DailyCount += 1;
                await _context.SaveChangesAsync(cancellationtoken);
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
                if(emailCount.DailyCount >= _configuration.Value.EmailLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
