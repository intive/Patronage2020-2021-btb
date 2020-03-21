using BTB.Client.Pages.Dto.Validation;
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
        [Range(double.Epsilon, double.MaxValue, ErrorMessage ="Please enter positive number.")]
        public double Value { get; set; }

        [Required]
        public bool SendEmail { get; set; }

        [RequiredIf("SendEmail", true)]
        [EmailAddress(ErrorMessage ="Enter valid email.")]
        public string Email { get; set; }

        [RequiredIf("SendEmail", true)]
        [StringLength(500)]
        public string Message { get; set; }
    }
}
