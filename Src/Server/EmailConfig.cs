using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Server
{
    public class EmailConfig
    {
        public string smtpServer { get; set; }
        public int port { get; set; }
        public bool enableSsl { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    }
}
