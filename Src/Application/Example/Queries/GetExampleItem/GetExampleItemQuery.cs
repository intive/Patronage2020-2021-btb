using BTB.Application.Common.Exceptions;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace BTB.Application.Example.Queries.GetExampleItem
{
    // Added for the purpose of testing global exception handling
    public class GetExampleItemQuery : IRequest<int>
    {
        public int Id { get; set; }

        public class GetExampleItemQueryHandler : IRequestHandler<GetExampleItemQuery, int>
        {
            public async Task<int> Handle(GetExampleItemQuery request, CancellationToken cancellationToken)
            {
                throw new NotFoundException(nameof(Task<int>), request.Id);
            }
        }
    }
}
