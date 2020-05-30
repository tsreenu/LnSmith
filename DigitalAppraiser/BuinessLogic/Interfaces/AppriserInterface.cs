using DigitalAppraiser.Models.DBModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DigitalAppraiser.BuinessLogic.Interfaces
{
    interface AppriserInterface
    {
        Models.ViewModels.TodayRateModel GetUserDateRates(int AppraiserId);
        Models.ViewModels.TodayRateModel SaveTodayRate(Models.ViewModels.TodayRateModel model,string userName);
        IEnumerable<SelectListItem> GetAppraiserBanks(int appraiserId);
        int SaveSelfCustomerDetails(Models.ViewModels.SelfCustomerModel model,int AppraiserId, string userName);
        Models.DBModels.TodayRate GetTodayRate(int appraiserId, int bankId);
        int SaveOrnaments(string userName, int customerId, List<OrnamentDetails> model);
        Models.ViewModels.OrnamentDetailsModel GetOrnamentDetails(int customerId);
        Models.ViewModels.ReceiptModel GenerateLoan(Models.DBModels.LoanDetails model, int appraiserId);
        int SaveBankCustomerDetails(Models.ViewModels.BankCustomerModel model,int AppraiserId, string userName);
        Models.ViewModels.CustomerLoanDataModel GetCustomerLoanData(int AppraiserId);
        Models.ViewModels.ReceiptModel GetLoanDetails(string LoanId);
        int SettleLoan(string loanId, decimal collectedAmount, string collectedOn, decimal CollectedInterest, int NoOfDays);
        Models.ViewModels.SelfCustomerModel customerDetails(int loanType, int customerId);
        Models.DBModels.SubscriptionDetails GetSubscriptionDetails(int appraiserId);
        string payment(int planId);
        int AddPaymentDetails(PaytmResponse paytmResponse);
    }
}
