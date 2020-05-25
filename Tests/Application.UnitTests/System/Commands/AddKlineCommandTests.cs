using Application.UnitTests.Common;
using BTB.Application.Common.Exceptions;
using BTB.Application.System.Commands.AddKlineCommand;
using BTB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.System.Commands
{
    public class AddKlineCommandTests : CommandTestsBase
    {
        [Fact]
        public async Task Handle_ShouldAddKline_WhenRequestIsValid()
        {
            var expectedKlineId = 2;
            var expectedSymbolPairId = 1;
            var expectedSymbolPair = "BTCUSDT";
            var volume = 100.0m;
            var openPrice = 150.0m;
            var closePrice = 200.0m;

            _btbBinanceClientMock.Setup(mock => mock.GetSymbolPairByName(expectedSymbolPair))
                .Returns(Task.Run(() => new SymbolPair() { Id = expectedSymbolPairId }));

            var command = new AddKlineCommand()
            {
                SymbolPair = expectedSymbolPair,
                Volume = volume,
                OpenPrice = openPrice,
                ClosePrice = closePrice
            };

            ILoggerFactory factory = new LoggerFactory();
            var sut = new AddKlineCommandHandler(_context, _btbBinanceClientMock.Object, _mapper,
                factory.CreateLogger<AddKlineCommandHandler>(), _dateTimeMock.Object);
            await sut.Handle(command, CancellationToken.None);

            Kline dbKline = await _context.Klines
                .Include(kline => kline.SymbolPair).ThenInclude(symbolPair => symbolPair.BuySymbol)
                .Include(kline => kline.SymbolPair).ThenInclude(symbolPair => symbolPair.SellSymbol)
                .SingleOrDefaultAsync(kline => kline.Id == expectedKlineId);

            Assert.Equal(expectedSymbolPairId, dbKline.SymbolPairId);
            Assert.Equal(expectedSymbolPair, dbKline.SymbolPair.PairName);
            Assert.Equal(volume, dbKline.Volume);
            Assert.Equal(openPrice, dbKline.OpenPrice);
            Assert.Equal(closePrice, dbKline.ClosePrice);
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenSymbolPairDoesNotExist()
        {
            var symbolPair = "AAABBB";
            var volume = 100.0m;
            var closePrice = 200.0m;

            _btbBinanceClientMock.Setup(mock => mock.GetSymbolPairByName(symbolPair));

            var command = new AddKlineCommand()
            {
                SymbolPair = symbolPair,
                Volume = volume,
                ClosePrice = closePrice
            };

            var sut = new AddKlineCommandHandler(_context, _btbBinanceClientMock.Object, _mapper,
                _disabledLoggerFactory.CreateLogger<AddKlineCommandHandler>(), _dateTimeMock.Object);
            await Assert.ThrowsAsync<BadRequestException>(async () => await sut.Handle(command, CancellationToken.None));
        }
    }
}
