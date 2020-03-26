using AutoMapper;
using BTB.Application.Authorize.Common;
using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Application.Authorize.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginParametersDto>
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginCommandHandler(IConfiguration config, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public async Task<LoginParametersDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginParameters = _mapper.Map<LoginParameters>(request);

            var user = await _userManager.FindByNameAsync(loginParameters.UserName);
            if (user == null)
            {
                throw new BadRequestException("Incorrect username or password.");
            }

            var singInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!singInResult.Succeeded)
            {
                throw new BadRequestException("Incorrect username or password.");
            }

            await _signInManager.SignInAsync(user, loginParameters.RememberMe);

            loginParameters.Token = BuildToken();

            return _mapper.Map<LoginParametersDto>(loginParameters);
        }

        private string BuildToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtToken:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["JwtToken:Issuer"],
              _config["JwtToken:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
