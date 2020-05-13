using System;

namespace BTB.Common
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}
