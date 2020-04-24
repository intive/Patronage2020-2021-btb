using AutoMapper;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Application.System.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.System.Queries.GetAuditTrail
{
    public class GetAuditsQuery : IRequest<IEnumerable<AuditTrailVm>>
    {
        public int Count { get; set; }

        public class GetAuditsQueryHandler : IRequestHandler<GetAuditsQuery, IEnumerable<AuditTrailVm>>
        {
            private readonly IBTBDbContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public GetAuditsQueryHandler(IBTBDbContext context, IMapper mapper, IUserAccessor userAccessor)
                => (_context, _mapper, _userAccessor) = (context, mapper, userAccessor);

            public async Task<IEnumerable<AuditTrailVm>> Handle(GetAuditsQuery request, CancellationToken cancellationToken)
            {
                return await _context.AuditTrails
                        .Where(audit => audit.UserId == _userAccessor.GetCurrentUserId())
                        .Select(audit => _mapper.Map<AuditTrailVm>(audit))
                        .Take(request.Count < 0 ? 0 : request.Count)
                        .ToListAsync();
            }
        }
    }
}
