using MediatR;

namespace BTB.Application.Example.Queries.GetExampleItem
{
    // Added for the purpose of testing global exception handling
    public class GetExampleItemQuery : IRequest<int>
    {
        public int Id { get; set; }
    }
}
