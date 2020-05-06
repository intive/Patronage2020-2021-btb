using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.ConditionDetectors.Between
{
    public class BetweenConditionDetector : ConditionDetectorBase,
        IAlertConditionDetector<BasicConditionDetectorParameters>, IBetConditionDetector<BasicConditionDetectorParameters>
    {
        public bool IsConditionMet(Alert alert, BasicConditionDetectorParameters parameters)
        {
            if (alert.Condition != AlertCondition.Between)
            {
                return false;
            }

            (_, decimal value) = GetKlineValuesByAlertValueType(alert.ValueType, parameters.Kline);
            decimal lower = alert.Value;
            decimal upper = alert.AdditionalValue;
            return ConditionCheck(lower, value, upper);
        }

        public bool IsConditionMet(Bet bet, BasicConditionDetectorParameters parameters)
        {
            decimal value = parameters.Kline.ClosePrice;
            decimal lower = bet.LowerPriceThreshold;
            decimal upper = bet.UpperPriceThreshold;
            return ConditionCheck(lower, value, upper);
        }

        private bool ConditionCheck(decimal lower, decimal value, decimal upper)
        {
            if (lower <= value && value <= upper)
            {
                return true;
            }

            return false;
        }
    }
}
