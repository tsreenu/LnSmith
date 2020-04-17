namespace DigitalAppraiser.Models.ViewModels
{
    public class LoginModel
    {
        public int AppraiserId { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string NewPassword { get; set; }
        public string UserName { get; set; }
    }
}