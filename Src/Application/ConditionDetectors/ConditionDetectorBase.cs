using BTB.Domain.Entities;
using BTB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.ConditionDetectors
{
    public class ConditionDetectorBase
    {
        protected (decimal oldValue, decimal newValue) GetKlineValuesByAlertValueType(AlertValueType valueType, Kline kline)
        {
            return valueType switch
            {
                AlertValueType.Price => (kline.OpenPrice, kline.ClosePrice),
                AlertValueType.Volume => (0, kline.Volume),
                _ => throw new NotImplementedException()
            };
        }
    }
}
