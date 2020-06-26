using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalAppraiser.Models.ViewModels
{
    public class SignUpModel
    {
        public int AppraiserId { get; set; }
        [Required]
        [DisplayName("Appraiser Name")]
        public string AppraiserName { get; set; }
        public string AppraiserNumber { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Mobile number lenth should be 10.")]
        [MinLength(10, ErrorMessage = "Mobile number lenth should be 10.")]
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }
        [Required]
        [MinLength(6)]
        [DisplayName("Password")]
        public string Password { get; set; }
        //public string Address { get; set; }
        [Required]
        [DisplayName("State")]
        public int StateId { get; set; }
        [Required]
        [DisplayName("City")]
        public int CityId { get; set; }
        [Required]
        [DisplayName("Shop Name")]
        public string ShopName { get; set; }
        public bool IsSelfLoan { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        [NotMapped]
        public List<Models.DBModels.State> States { get; set; }
        [NotMapped]
        public List<Models.DBModels.BankMaster> Banks { get; set; }
        [NotMapped]
        [Required]
        public int?[] selectedBanks { get; set; }
        public string ShopAddress { get; set; }
        public string ShopNumber { get; set; }

        [NotMapped]
        [Compare(nameof(Password), ErrorMessage = "Password and New Password should be same.")]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public IEnumerable<System.Web.Mvc.SelectListItem> StatesList { get; set; }
        [NotMapped]
        public IEnumerable<System.Web.Mvc.SelectListItem> CitiesList { get; set; }
        [NotMapped]
        public IEnumerable<System.Web.Mvc.SelectListItem> bankList { get; set; }
        public int bankId { get; set; }
        public string Note { get; set; }
    }
}