using System;

namespace BTB.Domain.Entities
{
    public class Alert
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Symbol { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public double Value { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
