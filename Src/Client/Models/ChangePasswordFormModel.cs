namespace BTB.Client.Models
{
    public class ChangePasswordFormModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
    }
}
