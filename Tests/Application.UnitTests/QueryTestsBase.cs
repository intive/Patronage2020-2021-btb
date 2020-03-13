using Application.UnitTests.Common;
using AutoMapper;
using Binance.Net.Interfaces;
using BTB.Application.Common.Interfaces;
using BTB.Persistence;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace Application.UnitTests
{
    [Collection("QueryCollection")]
    public class QueryTestsBase : HandlerTestsBase
    {
    }
}
