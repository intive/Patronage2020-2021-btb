using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Commands.GetData
{
    public class GetAuditsCommand : IRequest<IEnumerable<AuditTrail>>
    {
        public int Count { get; set; }

        public class GetAuditsCommandHandler : IRequestHandler<GetAuditsCommand, IEnumerable<AuditTrail>>
        {
            private readonly IBTBDbContext _context;

            public GetAuditsCommandHandler(IBTBDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<AuditTrail>> Handle(GetAuditsCommand request, CancellationToken cancellationToken)
            {
                var result = await _context.AuditTrails.Take(request.Count).ToListAsync();

                if (!result.Any())
                    throw new NotFoundException();

                return result;
            }
        }
    }
}
