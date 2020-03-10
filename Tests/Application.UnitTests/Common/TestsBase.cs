using Application.UnitTests.Common;
using AutoMapper;
using Binance.Net.Interfaces;
using BTB.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UnitTests
{
    public class TestsBase : IDisposable
    {
        protected readonly IBinanceClient _binanceClient;
        protected readonly BTBDbContext _context;
        protected readonly IMapper _mapper;

        public TestsBase()
        {
            QueryTestFixture fixture = QueryTestFixture.Get();
            this._context = fixture.Context;
            this._mapper = fixture.Mapper;

            _binanceClient = IBinanceClientFactory.BinanceClient;
        }

        public void Dispose()
        {
            BTBContextFactory.Destroy(_context);
        }
    }
}
