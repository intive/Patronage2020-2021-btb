using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Exceptions.GamblePointManager
{
    public class NotEnoughGamblePointsException : Exception
    {
        public NotEnoughGamblePointsException(string message = "")
            : base(message)
        {
        }
    }
}
