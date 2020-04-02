using BTB.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UnitTests.Authorize.Commands
{
    public class LoginCommandHandlerTestsFixture
    {
        public readonly UserManager<ApplicationUser> UserManager;
        public readonly SignInManager<ApplicationUser> SignInManager;

        public LoginCommandHandlerTestsFixture()
        {
        }
    }
}