using AutoMapper;
using DigitalAppraiser.BuinessLogic.Interfaces;
using DigitalAppraiser.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace DigitalAppraiser.BuinessLogic.Implementation
{
    public class AppraiserClass : AppriserInterface
    {
        private Models.DBModels.DigitalAppraiserDB _Context = new Models.DBModels.DigitalAppraiserDB();
        public Models.ViewModels.TodayRateModel GetUserDateRates(int AppraiserId)
        {
            _Context.Configuration.LazyLoadingEnabled = false;
            Models.ViewModels.TodayRateModel model = new Models.ViewModels.TodayRateModel();
            model.AllBanks = _Context.BankMasters.Where(x => x.IsActive == true).ToList();
            var appriaserBanks = _Context.AppraiserBanks.Where(x => x.AppriaserId == AppraiserId && x.IsActive == true).ToList();
            //var date = DateTime.Now.Date;
            //model.TodayRates = _Context.TodayRates.Where(x => x.AppraiserId == AppraiserId && x.IsActive == true && x.CreatedOn.Year == date.Year && x.CreatedOn.Month == date.Month && x.CreatedOn.Day == date.Day).ToList();
            model.TodayRates = new List<TodayRate>();
            if (appriaserBanks.Count() > 0)
            {
                var bankCount = appriaserBanks.Count();
                foreach (var item in appriaserBanks)
                {
                    model.TodayRates.Add(_Context.TodayRates.Where(x => x.AppraiserId == AppraiserId && x.IsActive == true && x.BankId == item.BankId).OrderByDescending(x => x.Id).FirstOrDefault());
                }
            }
            if (model.TodayRates.Count == 0)
            {
                model.TodayRates = new List<Models.DBModels.TodayRate>();
                foreach (var item in appriaserBanks)
                {
                    model.TodayRates.Add(new Models.DBModels.TodayRate { BankId = item.BankId, AppraiserId = item.AppriaserId });
                }
            }
            return model;
        }
        public Models.ViewModels.TodayRateModel SaveTodayRate(Models.ViewModels.TodayRateModel model, string userName)
        {

            try
            {
                int? appraiserid = model.TodayRates[0].AppraiserId;
                var date = DateTime.Now.Date;
                var todayList = _Context.TodayRates.Where(x => x.AppraiserId == appraiserid && x.CreatedOn.Year == date.Year && x.CreatedOn.Month == date.Month && x.CreatedOn.Day == date.Day).ToList();

                foreach (var item in model.TodayRates)
                {
                    var rate = _Context.TodayRates.Where(x => x.Id == item.Id).FirstOrDefault();
                    _Context.TodayRates.Remove(rate);
                }
                _Context.SaveChanges();
                //if (todayList.Count == 0)
                //{
                foreach (var item in model.TodayRates)
                {
                    item.Id = 0;
                    item.CreatedBy = userName;
                    item.CreatedOn = DateTime.Now;
                    item.ModifiedBy = userName;
                    item.ModifiedOn = DateTime.Now;
                    item.IsActive = true;
                    _Context.TodayRates.Add(item);
                }
                _Context.SaveChanges();
                model.message = "Save success";
                // }
                //else
                //{
                //  foreach (var item in model.TodayRates)
                //  {
                //      todayList.Where(x => x.Id == item.Id).FirstOrDefault().Rate = item.Rate;
                //      todayList.Where(x => x.Id == item.Id).FirstOrDefault().ModifiedOn = DateTime.Now;
                // }
                // _Context.SaveChanges();
                model.message = "Update Success";
                //}

            }
            catch (Exception ex)
            {
                model.message = "failed";
            }
            return model;
        }
        public IEnumerable<SelectListItem> GetAppraiserBanks(int appraiserId)
        {
            //_Context.Configuration.LazyLoadingEnabled = false;
            var list = (from apb in _Context.AppraiserBanks.Where(x => x.AppriaserId == appraiserId && x.IsActive == true)
                        join bm in _Context.BankMasters.Where(x => x.IsActive == true && x.BankCode != "SELF") on apb.BankId equals bm.BankId
                        select new { value = apb.BankId.Value, text = bm.BankName }).OrderBy(x => x.value).ToList();
            IEnumerable<SelectListItem> dd = list.Select(x => new SelectListItem
            {
                Value = x.value.ToString(),
                Text = x.text
            });
            return dd;
        }
        public int SaveSelfCustomerDetails(Models.ViewModels.SelfCustomerModel model, int AppraiserId, string userName)
        {
            int result = 0;
            try
            {
                model.ornamentDetails.CreatedOn = model.selfCustomer.CreatedOn;
                //model.selfCustomer.CreatedOn = DateTime.Now;
                model.selfCustomer.ModifiedOn = DateTime.Now;
                model.selfCustomer.CreatedBy = userName;
                model.selfCustomer.ModifiedBy = userName;
                model.selfCustomer.AppraiserId = AppraiserId;
                model.selfCustomer.IsActive = true;
                _Context.SelfCustomerDetails.Add(model.selfCustomer);
                _Context.SaveChanges();

                List<Models.DBModels.OrnamentDetails> ornamentsList = new List<OrnamentDetails>();
                model.ornamentDetails.LoanType = 1;
                ornamentsList.Add(model.ornamentDetails);
                SaveOrnaments(userName, model.selfCustomer.CustomerId, ornamentsList);
                result = model.selfCustomer.CustomerId;
            }
            catch (Exception ex)
            {
                var exp = ex.Message;
                result = 0;
            }

            return result;
        }
        public Models.DBModels.TodayRate GetTodayRate(int appraiserId, int bankId)
        {
            _Context.Configuration.LazyLoadingEnabled = false;
            var rate = _Context.TodayRates.OrderByDescending(x => x.Id).Where(x => x.AppraiserId == appraiserId && x.IsActive == true && x.BankId == bankId).FirstOrDefault();
            return rate;
        }
        public int SaveOrnaments(string userName, int customerId, List<OrnamentDetails> model)
        {
            foreach (var item in model)
            {
                item.CreatedBy = userName;
                item.ModifiedBy = userName;
                if (item.LoanType != 1)
                {
                    item.CreatedOn = DateTime.Now;
                }
                item.ModifiedOn = DateTime.Now;
                item.CustomerId = customerId;
                item.IsActive = true;
                _Context.OrnamentDetails.Add(item);
            }
            _Context.SaveChanges();
            return 1;
        }
        public Models.ViewModels.OrnamentDetailsModel GetOrnamentDetails(int customerId, int LoanType)
        {
            Models.ViewModels.OrnamentDetailsModel model = new Models.ViewModels.OrnamentDetailsModel();
            model.ornamentsList = _Context.OrnamentDetails.Where(x => x.CustomerId == customerId && x.LoanType == LoanType && x.IsActive == true).ToList();
            return model;
        }
        public Models.ViewModels.ReceiptModel GenerateLoan(Models.DBModels.LoanDetails model, int appraiserId)
        {
            Models.ViewModels.ReceiptModel receipt = new Models.ViewModels.ReceiptModel();
            try
            {
                if (model.LoanType != 1)
                {
                    model.CreatedOn = DateTime.Now;
                }
                model.ModifiedOn = DateTime.Now;
                model.IsActive = true;
                _Context.LoanDetails.Add(model);
                _Context.SaveChanges();
                if (model.LoanType == 1)
                {
                    model.LoanId = model.ID < 10 ? "GL00" + model.ID : model.ID < 100 ? "GL0" + model.ID : "GL" + model.ID;
                    _Context.SaveChanges();
                }
                receipt.loanDetails = model;
                if (model.LoanType == 1)
                {
                    receipt.selfCustomerDetails = _Context.SelfCustomerDetails.Where(x => x.CustomerId == model.CustomerId && x.IsActive == true).FirstOrDefault();
                    var appraiser = _Context.AppraiserDetails.Where(x => x.AppraiserId == appraiserId && x.IsActive == true).FirstOrDefault();
                    receipt.ShopDetails = appraiser.ShopName;
                    receipt.ShopAddress = appraiser.ShopAddress;
                    receipt.Shopnumber = appraiser.ShopNumber;
                    receipt.Note = appraiser.Note;
                }
                else
                {
                    receipt.bankCustomerDetails = _Context.BankCustomerDetails.Where(x => x.CustomerId == model.CustomerId).FirstOrDefault();
                    receipt.todayRate = GetTodayRate(appraiserId, model.LoanType);
                    var cityId = _Context.AppraiserDetails.Where(x => x.AppraiserId == appraiserId && x.IsActive == true).FirstOrDefault().CityId;
                    receipt.Place = _Context.Cities.Where(x => x.CityId == cityId && x.IsActive == true).FirstOrDefault().CityName;
                }
                receipt.ornamentsList = _Context.OrnamentDetails.Where(x => x.CustomerId == model.CustomerId && x.LoanType == model.LoanType && x.IsActive == true).ToList();

            }
            catch (Exception ex)
            {

            }
            return receipt;
        }
        public int SaveBankCustomerDetails(Models.ViewModels.BankCustomerModel model, int AppraiserId, string userName)
        {

            int result = 0;
            try
            {
                model.bankCustomer.CreatedOn = DateTime.Now;
                model.bankCustomer.ModifiedOn = DateTime.Now;
                model.bankCustomer.CreatedBy = userName;
                model.bankCustomer.ModifiedBy = userName;
                model.bankCustomer.AppraiserId = AppraiserId;
                model.bankCustomer.IsActive = true;
                _Context.BankCustomerDetails.Add(model.bankCustomer);
                _Context.SaveChanges();

                // List<Models.DBModels.OrnamentDetails> ornamentsList = new List<OrnamentDetails>();
                //foreach()
                // model.ornamentDetails.LoanType = 2;
                //ornamentsList.Add(model.ornamentDetails);
                SaveOrnaments(userName, model.bankCustomer.CustomerId, model.ornamentsList);
                result = model.bankCustomer.CustomerId;
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
        public Models.ViewModels.CustomerLoanDataModel GetCustomerLoanData(int AppraiserId)
        {
            Models.ViewModels.CustomerLoanDataModel model = new Models.ViewModels.CustomerLoanDataModel();

            List<Models.ViewModels.CustomerLoanData> selfList = (from self in _Context.SelfCustomerDetails.Where(x => x.AppraiserId == AppraiserId && x.IsActive == true)
                                                                 join loan in _Context.LoanDetails.Where(x => x.LoanType == 1 && x.IsActive == true) on self.CustomerId equals loan.CustomerId
                                                                 join bankmaster in _Context.BankMasters.Where(x => x.IsActive == true) on loan.LoanType equals bankmaster.BankId
                                                                 select new Models.ViewModels.CustomerLoanData
                                                                 {
                                                                     Date = loan.CreatedOn,
                                                                     BankName = bankmaster.BankName,
                                                                     CustomerName = self.Name,
                                                                     InterestRate = loan.Interest,
                                                                     LoanAmount = loan.LoanAmount,
                                                                     LoanId = loan.LoanId,
                                                                     MobileNumber = self.MobileNumber,
                                                                     Area = self.Address,
                                                                     IsActive = loan.IsActive
                                                                 }
                                     ).ToList();

            List<Models.ViewModels.CustomerLoanData> bankList = (from bank in _Context.BankCustomerDetails.Where(x => x.AppraiserId == AppraiserId && x.IsActive == true)
                                                                 join loans in _Context.LoanDetails.Where(x => x.LoanType != 1 && x.IsActive == true) on bank.CustomerId equals loans.CustomerId
                                                                 join bankmaster in _Context.BankMasters.Where(x => x.IsActive == true) on loans.LoanType equals bankmaster.BankId
                                                                 select new Models.ViewModels.CustomerLoanData
                                                                 {
                                                                     Date = loans.CreatedOn,
                                                                     BankName = bankmaster.BankName,
                                                                     CustomerName = bank.Name,
                                                                     InterestRate = loans.Interest,
                                                                     LoanAmount = loans.LoanAmount,
                                                                     LoanId = loans.ID.ToString(),
                                                                     MobileNumber = bank.MobileNumber,
                                                                     Area = bank.Address,
                                                                     IsActive = loans.IsActive
                                                                 }
                                      ).ToList();

            model.selfCustomerDataList = selfList;
            model.bankCustomerDataList = bankList;
            return model;
        }
        public Models.ViewModels.ReceiptModel GetLoanDetails(string LoanId)
        {
            LoanId = LoanId.Trim();
            Models.ViewModels.ReceiptModel model = new Models.ViewModels.ReceiptModel();
            model.loanDetails = _Context.LoanDetails.Where(x => x.LoanId == LoanId).FirstOrDefault();
            if (model.loanDetails != null)
            {
                model.selfCustomerDetails = _Context.SelfCustomerDetails.Where(x => x.CustomerId == model.loanDetails.CustomerId).FirstOrDefault();
                model.ornamentsList = _Context.OrnamentDetails.Where(x => x.CustomerId == model.loanDetails.CustomerId && x.LoanType == 1).ToList();
                model.errMessage = "";
            }
            else
            {
                model.errMessage = "Loan ID not available.";
            }
            return model;
        }
        public int SettleLoan(string loanId, decimal collectedAmount, string collectedOn, decimal CollectedInterest, int NoOfDays)
        {
            int result = 1;
            try
            {
                var loanDetails = _Context.LoanDetails.Where(x => x.LoanId == loanId && x.IsActive == true).FirstOrDefault();
                if (loanDetails != null)
                {
                    loanDetails.CollectedAmount = collectedAmount;
                    loanDetails.CollectedInterest = CollectedInterest;
                    loanDetails.NoOfDays = NoOfDays;
                    loanDetails.ModifiedOn = Convert.ToDateTime(collectedOn);
                    loanDetails.IsActive = false;
                    result = _Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
        public Models.ViewModels.SelfCustomerModel customerDetails(int loanType, int customerId)
        {
            Models.ViewModels.SelfCustomerModel model = new Models.ViewModels.SelfCustomerModel();
            model.selfCustomer = new SelfCustomerDetails();
            if (loanType == 1)
            {
                var customer = _Context.SelfCustomerDetails.Where(x => x.CustomerId == customerId && x.IsActive == true).FirstOrDefault();
                model.selfCustomer.Name = customer.Name;
                model.selfCustomer.MobileNumber = customer.MobileNumber;
                model.selfCustomer.Address = customer.Address;
                model.selfCustomer.UANNumber = customer.UANNumber;
            }
            else
            {
                var customer = _Context.BankCustomerDetails.Where(x => x.CustomerId == customerId && x.IsActive == true).FirstOrDefault();
                model.selfCustomer.Name = customer.Name;
                model.selfCustomer.MobileNumber = customer.MobileNumber;
                model.selfCustomer.Address = customer.Address;
            }
            return model;
        }

        public Models.DBModels.SubscriptionDetails GetSubscriptionDetails(int appraiserId)
        {
            Models.DBModels.SubscriptionDetails subscription = new Models.DBModels.SubscriptionDetails();
            subscription = (from subs in _Context.SubscriptionDetails.Where(x => x.AppraiserId == appraiserId)
                            join plan in _Context.LnSmithPlans
                            on subs.PlanId equals plan.PlanId
                            select new
                            {
                                AppraiserId = subs.AppraiserId,
                                PlanId = subs.PlanId,
                                PlanName = plan.PlanName,
                                SubscriptionId = subs.SubscriptionId,
                                SubscriptionEndDate = subs.SubscriptionEndDate,
                                SubscriptionStartDate = subs.SubscriptionStartDate
                            }
                            ).ToList().Select(x => new Models.DBModels.SubscriptionDetails
                            {
                                AppraiserId = x.AppraiserId,
                                PlanId = x.PlanId,
                                PlanName = x.PlanName,
                                SubscriptionId = x.SubscriptionId,
                                SubscriptionEndDate = x.SubscriptionEndDate,
                                SubscriptionStartDate = x.SubscriptionStartDate
                            }).FirstOrDefault();

            subscription.lnSmithPlans = _Context.LnSmithPlans.Where(x => x.IsActive == true && x.PlanId != 1).ToList();
            return subscription;
        }

        public string payment(int planId)
        {
            var mid = ConfigurationManager.AppSettings["MerchantID"].ToString();
            var clientSecret = ConfigurationManager.AppSettings["AccountSecretKey"].ToString();
            var txnAmount = _Context.LnSmithPlans.Where(x => x.PlanId == planId).FirstOrDefault().PlanValue.ToString();
            /* initialize a TreeMap object */
            Dictionary<String, String> paytmParams = new Dictionary<String, String>();

            /* Find your MID in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys */
            paytmParams.Add("MID", mid);

            /* Find your WEBSITE in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys */
            paytmParams.Add("WEBSITE", "WEBSTAGING");

            /* Find your INDUSTRY_TYPE_ID in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys */
            paytmParams.Add("INDUSTRY_TYPE_ID", "Retail");

            /* WEB for website and WAP for Mobile-websites or App */
            paytmParams.Add("CHANNEL_ID", "WEB");

            var orderId = Guid.NewGuid().ToString("N").ToUpper();

            /* Enter your unique order id */
            paytmParams.Add("ORDER_ID", orderId);

            /* unique id that belongs to your customer */
            paytmParams.Add("CUST_ID", LogedUser.AppraiserId.Value.ToString());

            /* customer's mobile number */
            paytmParams.Add("MOBILE_NO", LogedUser.MobileNumber);

            /* customer's email */
            paytmParams.Add("EMAIL", "tsreenu.9@gmail.com");

            /**
            * Amount in INR that is payble by customer
            * this should be numeric with optionally having two decimal points
*/
            paytmParams.Add("TXN_AMOUNT", txnAmount);

            /* on completion of transaction, we will send you the response on this URL */
            paytmParams.Add("CALLBACK_URL", "http://localhost:26137/Apraiser/PaytmResponse");

            /**
            * Generate checksum for parameters we have
            * You can get Checksum DLL from https://developer.paytm.com/docs/checksum/
            * Find your Merchant Key in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys 
*/
            String checksum = paytm.CheckSum.generateCheckSum(clientSecret, paytmParams);

            /* for Staging */
            String url = "https://securegw-stage.paytm.in/order/process";

            /* for Production */
            // String url = "https://securegw.paytm.in/order/process";

            /* Prepare HTML Form and Submit to Paytm */
            String outputHtml = "";
            outputHtml += "<html>";
            outputHtml += "<head>";
            outputHtml += "<title>Merchant Checkout Page</title>";
            outputHtml += "</head>";
            outputHtml += "<body>";
            outputHtml += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHtml += "<form method='post' action='" + url + "' name='paytm_form'>";
            foreach (string key in paytmParams.Keys)
            {
                outputHtml += "<input type='hidden' name='" + key + "' value='" + paytmParams[key] + "'>";
            }
            outputHtml += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHtml += "</form>";
            outputHtml += "<script type='text/javascript'>";
            outputHtml += "document.paytm_form.submit();";
            outputHtml += "</script>";
            outputHtml += "</body>";
            outputHtml += "</html>";

            return outputHtml;
        }
        public int AddPaymentDetails(PaytmResponse paytmResponse)
        {
            int result = 0;
            paytmResponse.AppraiserId = LogedUser.AppraiserId.Value;
            _Context.PaytmResponse.Add(paytmResponse);
            if (paytmResponse.STATUS == "TXN_SUCCESS")
            {
                var userSubDetails = _Context.SubscriptionDetails.Where(x => x.AppraiserId == paytmResponse.AppraiserId).FirstOrDefault();
                var plan = _Context.LnSmithPlans.Where(x => x.PlanValue.ToString() == paytmResponse.TXNAMOUNT.Replace(".00", "")).FirstOrDefault();
                userSubDetails.SubscriptionEndDate = userSubDetails.SubscriptionEndDate.AddDays(plan.PlanDuration);
                userSubDetails.PlanId = plan.PlanId;
            }
            result = _Context.SaveChanges();
            return result;
        }
        public bool IsSelfEnabled(int AppraiserId)
        {
            var res = _Context.AppraiserBanks.Where(x => x.AppriaserId == AppraiserId && x.BankId == 1).ToList();
            bool isSelf = res.Count > 0 ? true : false;
            return isSelf;
        }
        public bool IsBankEnabled(int AppraiserId)
        {
            var res = _Context.AppraiserBanks.Where(x => x.AppriaserId == AppraiserId && x.BankId != 1).ToList();
            bool isSelf = res.Count > 0 ? true : false;
            return isSelf;
        }
        public int DeleteBankRecords(List<string> selectedIds)
        {
            string ids = string.Join(",", selectedIds.Select(n => n.ToString()).ToArray());
            string query = "update LoanDetails set IsActive = 0 where ID in (" + ids + ")";
            int rows = _Context.Database.ExecuteSqlCommand(query);
            return rows;
        }
        public int DeleteSelfRecords(List<string> selectedIds)
        {
            string ids = string.Join("','", selectedIds.Select(n => n.ToString()).ToArray());
            ids = "'" + ids + "'";
            string query = "update LoanDetails set IsActive = 0, ModifiedOn = getdate() where LoanId in (" + ids + ")";
            int rows = _Context.Database.ExecuteSqlCommand(query);
            return rows;
        }
    }
}