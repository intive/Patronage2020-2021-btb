using BTB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Interfaces
{
    public interface IAlertConditionDetector<TParameters>
    {
        bool IsConditionMet(Alert alert, TParameters parameters);
    }
}
