using BTB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Interfaces
{
    public interface IBetConditionDetector<TParameters>
    {
        bool IsConditionMet(Bet bet, TParameters parameters);
    }
}
