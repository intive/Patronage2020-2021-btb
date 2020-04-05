using AutoMapper;
using BTB.Application.Common.Mappings;
using BTB.Persistence;
using System;
using Xunit;

namespace Application.UnitTests.Common
{
    public class QueryTestFixture : IDisposable
    {
        public BTBDbContext Context { get; private set; }
        public IMapper Mapper { get; private set; }

        public QueryTestFixture()
        {
            Context = BTBContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EntitiesToDtosMappings>();
                cfg.AddProfile<RequestsToEntitiesMappings>();
            });

            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            BTBContextFactory.Destroy(Context);
        }

        public static QueryTestFixture Get()
        {
            return new QueryTestFixture();
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
