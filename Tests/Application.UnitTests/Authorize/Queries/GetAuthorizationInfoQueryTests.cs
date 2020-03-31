using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using BTB.Application.Authorize.Queries.GetAuthorizationInfo;
using System.Security.Claims;

namespace Application.UnitTests.Authorize.Queries
{
    public class GetAuthorizationInfoQueryTests : QueryTestsBase
    {
        [Fact]
        public async Task Handle_ShouldReturnSameAuthorizationInfo_AsQueryRequest()
        {
            var expectedIsAuthenticated = true;
            var expectedUserName = "username_test";

            var sut = new GetAuthorizationInfoQueryHandler(_mapper);
            var query = new GetAuthorizationInfoQuery();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, expectedUserName)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            query.User = new ClaimsPrincipal(identity);

            var result = await sut.Handle(query, CancellationToken.None);

            Assert.Equal(expectedIsAuthenticated, result.IsAuthenticated);
            Assert.Equal(expectedUserName, result.UserName);
            Assert.Single(result.ExposedClaims);
        }
    }
}