using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Domain.ValueObjects
{
    public class ApplicationUserVO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
