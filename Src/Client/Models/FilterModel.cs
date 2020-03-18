using System.ComponentModel.DataAnnotations;

namespace BTB.Client.Models
{
    public class FilterModel
    {
        [RegularExpression(@"^[a-zA-Z]+$",  ErrorMessage = "Use letters only please")]
        public string SymbolName { get; set; } = string.Empty;
    }
}
