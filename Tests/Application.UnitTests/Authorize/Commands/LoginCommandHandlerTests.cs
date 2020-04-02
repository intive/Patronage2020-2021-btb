using Application.UnitTests.Common;
using BTB.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Authorize.Commands
{
    public class LoginCommandHandlerTests : CommandTestsBase
    {
        [Fact]
        public async Task Test1()
        {
            var result = await _userManagerMock.Object.FindByNameAsync("admin");

            Assert.NotNull(result);
        }
    }
}