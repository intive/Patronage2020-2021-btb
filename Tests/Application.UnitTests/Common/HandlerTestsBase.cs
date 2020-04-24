﻿using Application.UnitTests.Common;
using AutoMapper;
using Binance.Net.Interfaces;
using BTB.Application.Common.Interfaces;
using BTB.Persistence;
using Moq;
using System;

namespace Application.UnitTests
{
    public class HandlerTestsBase : IDisposable
    {
        protected readonly Mock<IBinanceClient> _binanceClientMock;
        protected readonly Mock<IBTBBinanceClient> _btbBinanceClientMock;
        protected readonly BTBDbContext _context;
        protected readonly IMapper _mapper;

        public HandlerTestsBase()
        {
            QueryTestFixture fixture = QueryTestFixture.Get();
            _context = fixture.Context;
            _mapper = fixture.Mapper;

            _binanceClientMock = BinanceClientMockFactory.ClientMock;
            _btbBinanceClientMock = BTBBinanceClientMockFactory.ClientMock;

        }

        public void Dispose()
        {
            BTBContextFactory.Destroy(_context);
        }

        protected Mock<IUserAccessor> GetUserAccessorMock(string userId)
        {
            var userAccessorMock = new Mock<IUserAccessor>();
            userAccessorMock
                .Setup(x => x.GetCurrentUserId())
                .Returns(userId);
            return userAccessorMock;
        }
    }
}
