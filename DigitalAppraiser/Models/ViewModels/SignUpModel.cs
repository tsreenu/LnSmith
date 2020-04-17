using System;
using System.Collections.Generic;

namespace DigitalAppraiser.Models.ViewModels
{
    public class SignUpModel
    {
        public int AppraiserId { get; set; }
        public string AppraiserName { get; set; }
        public string AppraiserNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        //public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string ShopName { get; set; }
        public bool IsSelfLoan { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public List<Models.DBModels.State> States { get; set; }
        public List<Models.DBModels.BankMaster> Banks { get; set; }
        public int?[] selectedBanks { get; set; }
        public string ShopAddress { get; set; }
        public string ShopNumber { get; set; }
    }
}