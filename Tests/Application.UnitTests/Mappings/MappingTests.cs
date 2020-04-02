using AutoMapper;
using BTB.Application.Authorize.Common;
using BTB.Domain.Entities;
using Xunit;

namespace Application.UnitTests.Mappings
{
    public class MappingTests : IClassFixture<MappingTestsFixture>
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests(MappingTestsFixture fixture)
        {
            _configuration = fixture.ConfigurationProvider;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void ShouldMapAuthorizationInfo_ToAuthorizationInfoDto()
        {
            var entity = new AuthorizationInfo();

            var result = _mapper.Map<AuthorizationInfoDto>(entity);

            Assert.NotNull(result);
            Assert.IsType<AuthorizationInfoDto>(result);
        }
    }
}