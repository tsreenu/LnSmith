using System.Collections.Generic;
using System.Web.Mvc;

namespace DigitalAppraiser.Models.ViewModels
{
    public class ProcessLoanModel
    {
        public List<Models.DBModels.AppraiserBank> appraiserBanks { get; set; }
        public IEnumerable<SelectListItem> Banks { get; set; }
        public int selectedBank { get; set; }
        public SelfCustomerModel selfCustomer { get; set; }
    }
}