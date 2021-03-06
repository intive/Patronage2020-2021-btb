﻿using BTB.Application.Alerts.Queries.GetAllAlertsQuery;
using BTB.Domain.Common.Pagination;
using BTB.Domain.Enums;
using BTB.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Alerts.Queries
{
    public class GetAllAlertsQueryTests : QueryTestsBase
    {
        [Fact]
        public async Task Handle_ShouldReturnAllAlertsBelongingToUser()
        {
            var userId = "2";
            var expectedUserAlerts = new List<AlertVO>()
            {
                new AlertVO() { Id = 3, SymbolPair = "BTCUSDT", Condition = AlertCondition.Crossing, ValueType = AlertValueType.Volume, Value = 3000.0m, SendEmail = true, Email = "alert3@alert3.com" },
                new AlertVO() { Id = 4, SymbolPair = "BTCUSDT", Condition = AlertCondition.Crossing, ValueType = AlertValueType.Price, Value = 4000.0m, SendEmail = false, Email = null },
            };
            var expectedAllRecordsCount = 2;
            var userAccessorMock = GetUserAccessorMock(userId);


            var sut = new GetAllAlertsQueryHandler(_context, _mapper, userAccessorMock.Object);
            var command = new GetAllAlertsQuery()
            {
                Pagination = new PaginationDto() { Page = 1, Quantity = PaginationQuantity.Ten },
            };

            var paginatedResult = await sut.Handle(command, CancellationToken.None);
            Assert.Equal(expectedAllRecordsCount, paginatedResult.AllRecordsCount);

            var resultAlerts = paginatedResult.Result.ToList();
            Assert.Equal(resultAlerts.Count(), expectedAllRecordsCount);
            
            foreach (var alert in expectedUserAlerts)
            {
                var single = resultAlerts.SingleOrDefault(
                    resultAlert =>
                    resultAlert.Id == alert.Id && 
                    resultAlert.SymbolPair == alert.SymbolPair &&
                    resultAlert.Condition == alert.Condition &&
                    resultAlert.ValueType == alert.ValueType &&
                    resultAlert.Value     == alert.Value &&
                    resultAlert.SendEmail == alert.SendEmail &&
                    resultAlert.Email     == alert.Email
                );

                Assert.NotNull(single);
            }
        }
    }
}
