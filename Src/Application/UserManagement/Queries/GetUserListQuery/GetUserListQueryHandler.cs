using AutoMapper;
using BTB.Application.Common.Interfaces;
using BTB.Application.Common.Models;
using BTB.Domain.Entities;
using BTB.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BTB.Application.Common.Behaviours;
using BTB.Application.Common.Exceptions;
using BTB.Domain.Extensions;
using BTB.Domain.Common;

namespace BTB.Application.UserManagement.Queries.GetUserListQuery
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, PaginatedResult<ApplicationUserVO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public GetUserListQueryHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ApplicationUserVO>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users
                .Select(u => _mapper.Map<ApplicationUserVO>(u))
                .ToList();

            return new PaginatedResult<ApplicationUserVO>
            {
                Result = users.OrderByDescending(u => u.UserId).Paginate(request.Pagination),
                AllRecordsCount = users.Count(),
                RecordsPerPage = (int)request.Pagination.Quantity
            };
        }
    }
}
