using System.ComponentModel.DataAnnotations;

namespace BTB.Client.Pages.Dto
{
    public class AlertDto
    {

        public string Symbol { get; set; }
        [Required]
        public string Condition { get; set; }
        [Required]
        public string ValueType { get; set; }
        [Required]
        [Range(1,double.MaxValue,ErrorMessage ="Please enter valid number.")]
        public double Value { get; set; }
        public bool SendEmail { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Enter valid email.")]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }

    }
}
