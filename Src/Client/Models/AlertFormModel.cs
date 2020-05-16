using BTB.Domain.ValueObjects;

namespace BTB.Client.Models
{
    public class AlertFormModel
    {
        public string SymbolPair { get; set; }
        public string Condition { get; set; }
        public string ValueType { get; set; }
        public decimal Value { get; set; }
        public decimal AdditionalValue { get; set; }
        public bool SendEmail { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool TriggerOnce { get; set; }

        public AlertFormModel()
        {
        }

        public AlertFormModel(AlertVO vo)
        {
            SymbolPair = vo.SymbolPair;
            Condition = vo.Condition.ToString();
            ValueType = vo.ValueType.ToString();
            Value = vo.Value;
            AdditionalValue = vo.AdditionalValue;
            SendEmail = vo.SendEmail;
            Email = vo.Email;
            Message = vo.Message;
            TriggerOnce = vo.TriggerOnce;
        }
    } 
}
