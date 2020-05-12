using BTB.Common;
using System;

namespace BTB.Infrastructure
{
    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
        public DateTime Today => DateTime.Today;
    }
}
