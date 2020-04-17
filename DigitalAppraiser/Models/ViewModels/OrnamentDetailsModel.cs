using System.Collections.Generic;
using System.Web.Mvc;

namespace DigitalAppraiser.Models.ViewModels
{
    public class OrnamentDetailsModel
    {
        public Models.DBModels.OrnamentDetails ornamentDetails { get; set; }
        public List<Models.DBModels.OrnamentDetails> ornamentsList { get; set; }
        public Models.DBModels.TodayRate todayRate { get; set; }
        public IEnumerable<SelectListItem> Quantity { get; set; }
        public int customerId { get; set; }
        public Models.DBModels.LoanDetails loanDetails { get; set; }
        public string ShopDetails { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string Aadhar { get; set; }
        public string Address { get; set; }
    }
}