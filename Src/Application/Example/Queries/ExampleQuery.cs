using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Example.Queries
{
    public class ExampleQuery : IRequest<bool>
    {
        public class ExampleQueryHandler : IRequestHandler<ExampleQuery, bool>
        {
            public async Task<bool> Handle(ExampleQuery request, CancellationToken cancellationToken)
            {
                return true;
            }
        }
    }
}
