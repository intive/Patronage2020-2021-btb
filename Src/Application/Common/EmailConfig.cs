using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common
{
    public class EmailConfig
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Login { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public string ApplicationDefaultUrl { get; set; }
    }
}
