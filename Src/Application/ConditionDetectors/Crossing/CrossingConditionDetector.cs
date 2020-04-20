﻿using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using BTB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.ConditionDetectors.Crossing
{
    public class CrossingConditionDetector : CrossingConditionDetectorBase, IAlertConditionDetector<CrossingConditionDetectorParameters>
    {
        public bool IsConditionMet(Alert alert, CrossingConditionDetectorParameters parameters)
        {
            if (alert.Condition != AlertCondition.Crossing)
            {
                return false;
            }

            (decimal oldValue, decimal newValue) = GetKlineValuesByAlertValueType(alert.ValueType, parameters.Kline);
            decimal threshold = alert.Value;

            if (newValue >= threshold && threshold >= oldValue || newValue <= threshold && threshold <= oldValue)
            {
                return true;
            }

            return false;
        }
    }
}
