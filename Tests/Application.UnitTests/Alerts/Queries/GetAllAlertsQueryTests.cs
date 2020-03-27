﻿using BTB.Application.Alerts.Common;
using BTB.Application.Alerts.Queries.GetAllAlertsQuery;
using BTB.Domain.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static BTB.Application.Alerts.Queries.GetAllAlertsQuery.GetAllAlertsQuery;

namespace Application.UnitTests.Alerts.Queries
{
    public class GetAllAlertsQueryTests : QueryTestsBase
    {
        [Fact]
        public async Task Handle_ShouldReturnAllAlertsBelongingToUser()
        {
            var userId = "2";
            var expectedUserAlerts = new List<AlertVm>()
            {
                new AlertVm() { Id = 3, Symbol = "BTCUSDT", Condition = "crossing", ValueType = "volume", Value = 3000.0d, SendEmail = true, Email = "alert3@alert3.com", Message = "alert id: 3, user id: 2" },
                new AlertVm() { Id = 4, Symbol = "BTCUSDT", Condition = "crossing", ValueType = "price", Value = 4000.0d, SendEmail = false, Email = null, Message = null },
            };
            var expectedAllRecordsCount = 2;
            var userIdentityMock = GetUserIdentityMock(userId);


            var sut = new GetAllAlertsQueryHandler(_context, _mapper, userIdentityMock.Object);
            var command = new GetAllAlertsQuery()
            {
                Pagination = new PaginationDto() { Page = 1, Quantity = PaginationQuantity.Ten },
            };

            var paginatedResult = await sut.Handle(command, CancellationToken.None);
            Assert.Equal(expectedAllRecordsCount, paginatedResult.AllRecorsCount);

            var resultAlerts = paginatedResult.Result.ToList();
            Assert.Equal(resultAlerts.Count(), expectedAllRecordsCount);
            
            foreach (var alert in expectedUserAlerts)
            {
                var single = resultAlerts.SingleOrDefault(
                    resultAlert =>
                    resultAlert.Id == alert.Id && 
                    resultAlert.Symbol == alert.Symbol &&
                    resultAlert.Condition == alert.Condition &&
                    resultAlert.ValueType == alert.ValueType &&
                    resultAlert.Value     == alert.Value &&
                    resultAlert.SendEmail == alert.SendEmail &&
                    resultAlert.Email     == alert.Email &&
                    resultAlert.Message   == alert.Message
                );

                Assert.NotNull(single);
            }
        }
    }
}
