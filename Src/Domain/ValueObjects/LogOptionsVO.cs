using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.ValueObjects
{
    public class LogOptionsVO
    {
        public bool Information { get; set; }
        public bool Warning { get; set; }
        public bool Error { get; set; }
        public bool Critical { get; set; }
    }
}
