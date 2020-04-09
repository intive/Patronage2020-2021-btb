using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common
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
