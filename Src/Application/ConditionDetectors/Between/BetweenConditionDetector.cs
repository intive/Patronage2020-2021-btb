using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.ConditionDetectors.Between
{
    public class BetweenConditionDetector : ConditionDetectorBase, IConditionDetector<BasicConditionDetectorParameters>
    {
        public bool IsConditionMet(Alert alert, BasicConditionDetectorParameters parameters)
        {
            if (alert.Condition != AlertCondition.Between)
            {
                return false;
            }

            (_, decimal value) = GetKlineValuesByAlertValueType(alert.ValueType, parameters.Kline);
            decimal lowerThreshold = alert.Value;
            decimal upperThreshold = alert.AdditionalValue;

            if (lowerThreshold <= value && value <= upperThreshold)
            {
                return true;
            }

            return false;
        }

        public bool IsConditionMet(Bet bet, BasicConditionDetectorParameters parameters)
        {
            decimal value = parameters.Kline.ClosePrice;
            if (bet.LowerPriceThreshold <= value && value <= bet.UpperPriceThreshold)
            {
                return true;
            }

            return false;
        }
    }
}
