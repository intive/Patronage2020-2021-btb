using AutoMapper;
using BTB.Application.Indicator.Commands.CalculateRSI;
using BTB.Application.Indicator.Commands.CalculateSMA;
using BTB.Application.System.Common;
using BTB.Application.UserProfile.Common;
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
        public void ShouldMapCalculateRSICommandToCalculateRSIDto()
        {
            var entity = new CalculateRSICommand();

            var result = _mapper.Map<CalculateRSIDto>(entity);

            Assert.NotNull(result);
            Assert.IsType<CalculateRSIDto>(result);
        }

        [Fact]
        public void ShouldMapCalculateSMACommandToCalculateSMADto()
        {
            var entity = new CalculateSMACommand();

            var result = _mapper.Map<CalculateSMADto>(entity);

            Assert.NotNull(result);
            Assert.IsType<CalculateSMADto>(result);
        }

        [Fact]
        public void ShouldMapAuditTrailToAuditTrailVm()
        {
            var entity = new AuditTrail();

            var result = _mapper.Map<AuditTrailVm>(entity);

            Assert.NotNull(result);
            Assert.IsType<AuditTrailVm>(result);
        }

        [Fact]
        public void ShouldMapUserProfileInfoToUserProfileInfoVm()
        {
            var entity = new UserProfileInfo();

            var result = _mapper.Map<UserProfileInfoVm>(entity);

            Assert.NotNull(result);
            Assert.IsType<UserProfileInfoVm>(result);
        }
    }
}