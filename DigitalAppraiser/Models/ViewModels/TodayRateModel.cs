using System.Collections.Generic;

namespace DigitalAppraiser.Models.ViewModels
{
    public class TodayRateModel
    {
        public List<Models.DBModels.TodayRate> TodayRates { get; set; }
        public List<Models.DBModels.BankMaster> AllBanks { get; set; }
        public string message { get; set; }
    }
}