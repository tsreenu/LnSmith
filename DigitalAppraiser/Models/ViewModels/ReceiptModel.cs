using System.Collections.Generic;

namespace DigitalAppraiser.Models.ViewModels
{
    public class ReceiptModel
    {
        public List<Models.DBModels.OrnamentDetails> ornamentsList { get; set; }
        public Models.DBModels.SelfCustomerDetails selfCustomerDetails { get; set; }
        public Models.DBModels.LoanDetails loanDetails { get; set; }
        public string ShopDetails { get; set; }
        public string ShopAddress { get; set; }
        public string Shopnumber { get; set; }
        public Models.DBModels.BankCustomerDetails bankCustomerDetails { get; set; }
        public Models.DBModels.TodayRate todayRate { get; set; }
        public string Place { get; set; }
        public string errMessage { get; set; }
        public string Note { get; set; }
    }
}