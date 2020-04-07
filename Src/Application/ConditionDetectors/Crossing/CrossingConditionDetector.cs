using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.ConditionDetectors.Crossing
{
    public class CrossingConditionDetector : IAlertConditionDetector<CrossingConditionDetectorParameters>
    {
        public bool IsConditionMet(Alert alert, CrossingConditionDetectorParameters parameters)
        {
            if (alert.Condition != AlertCondition.Crossing)
            {
                return false;
            }

            decimal newValue = GetKlineValueByAlertValueType(alert.ValueType, parameters.NewKline);
            decimal oldValue = GetKlineValueByAlertValueType(alert.ValueType, parameters.OldKline);
            decimal threshold = alert.Value;

            if (newValue > threshold && threshold > oldValue || newValue < threshold && threshold < oldValue)
            {
                return true;
            }

            return false;
        }

        private decimal GetKlineValueByAlertValueType(AlertValueType valueType, Kline kline)
        {
            return valueType switch
            {
                AlertValueType.Price => kline.ClosePrice,
                AlertValueType.Volume => kline.Volume,
                _ => throw new NotImplementedException()
            };
        }
    }
}
