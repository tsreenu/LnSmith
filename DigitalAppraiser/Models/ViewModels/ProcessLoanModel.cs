using System.Collections.Generic;
using System.Web.Mvc;

namespace DigitalAppraiser.Models.ViewModels
{
    public class ProcessLoanModel
    {
        public List<Models.DBModels.AppraiserBank> appraiserBanks { get; set; }
        public int selectedBank { get; set; }
        public SelfCustomerModel selfCustomer { get; set; }
        public BankCustomerModel bankCustomer { get; set; }
        public string ActiveTab { get; set; }
        public string isFirstLoad { get; set; }
        public bool isSelfEnable { get; set; }
    }
}