using BTB.Application.Common.Interfaces;
using BTB.Domain.Example.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Example.Queries
{
    public class ExampleQuery : IRequest<ExampleEntity>
    {
        public class ExampleQueryHandler : IRequestHandler<ExampleQuery, ExampleEntity>
        {
            private IBTBDbContext _context;

            public ExampleQueryHandler(IBTBDbContext context)
            {
                _context = context;
            }

            public async Task<ExampleEntity> Handle(ExampleQuery request, CancellationToken cancellationToken)
            {
                var entity = new ExampleEntity() { Name = "db works " };
                await _context.ExampleEntities.AddAsync(entity);
                await _context.SaveChangesAsync(CancellationToken.None);
                return await _context.ExampleEntities.FirstAsync();
            }
        }
    }
}
