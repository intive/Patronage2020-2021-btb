using Application.UnitTests.Common;
using AutoMapper;
using Binance.Net.Interfaces;
using BTB.Application.Common.Interfaces;
using BTB.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UnitTests
{
    public class HandlerTestsBase : IDisposable
    {
        protected readonly IBinanceClient _binanceClient;
        protected readonly IBTBBinanceClient _btbClient;
        protected readonly BTBDbContext _context;
        protected readonly IMapper _mapper;

        public HandlerTestsBase()
        {
            QueryTestFixture fixture = QueryTestFixture.Get();
            this._context = fixture.Context;
            this._mapper = fixture.Mapper;

            _binanceClient = IBinanceClientFactory.BinanceClient;
            _btbClient = IBTBBinanceClientFactory.BTBClient;
        }

        public void Dispose()
        {
            BTBContextFactory.Destroy(_context);
        }

        protected Mock<ICurrentUserIdentityService> GetUserIdentityMock(string userId)
        {
            var userIdentityMock = new Mock<ICurrentUserIdentityService>();
            userIdentityMock
                .Setup(x => x.UserId)
                .Returns(userId);
            return userIdentityMock;
        }
    }
}
