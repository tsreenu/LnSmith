using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DigitalAppraiser.Models.ViewModels
{
    public class LoginModel
    {
        public int AppraiserId { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Mobile number lenth should be 10.")]
        [MinLength(10, ErrorMessage = "Mobile number lenth should be 10.")]
        [DisplayName("Mobile number")]
        public string MobileNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        //[Compare(nameof(Password), ErrorMessage = "Password and New Password should be same.")]
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
        public string UserName { get; set; }
    }
}