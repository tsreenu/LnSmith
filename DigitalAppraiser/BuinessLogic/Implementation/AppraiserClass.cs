﻿using AutoMapper;
using DigitalAppraiser.BuinessLogic.Interfaces;
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

            if (appriaserBanks.Count() > 0)
            {
                var bankCount = appriaserBanks.Count();
                model.TodayRates = _Context.TodayRates.Where(x => x.AppraiserId == AppraiserId && x.IsActive == true).OrderByDescending(x => x.Id).Take(bankCount).ToList();
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

                if (todayList.Count == 0)
                {
                    foreach (var item in model.TodayRates)
                    {
                        item.CreatedBy = userName;
                        item.CreatedOn = DateTime.Now;
                        item.ModifiedBy = userName;
                        item.ModifiedOn = DateTime.Now;
                        item.IsActive = true;
                        _Context.TodayRates.Add(item);
                    }
                    _Context.SaveChanges();
                    model.message = "Save success";
                }
                else
                {
                    foreach (var item in model.TodayRates)
                    {
                        todayList.Where(x => x.Id == item.Id).FirstOrDefault().Rate = item.Rate;
                        todayList.Where(x => x.Id == item.Id).FirstOrDefault().ModifiedOn = DateTime.Now;
                    }
                    _Context.SaveChanges();
                    model.message = "Update Success";
                }

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
                        join bm in _Context.BankMasters.Where(x => x.IsActive == true) on apb.BankId equals bm.BankId
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
            int result = 1;
            try
            {
                model.CreatedOn = DateTime.Now;
                model.ModifiedOn = DateTime.Now;
                model.CreatedBy = userName;
                model.ModifiedBy = userName;
                model.AppraiserId = AppraiserId;
                model.IsActive = true;
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Models.ViewModels.SelfCustomerModel, Models.DBModels.SelfCustomerDetails>();
                });
                IMapper mapper = config.CreateMapper();
                var selfCustomerDetails = mapper.Map<Models.ViewModels.SelfCustomerModel, Models.DBModels.SelfCustomerDetails>(model);
                _Context.SelfCustomerDetails.Add(selfCustomerDetails);
                _Context.SaveChanges();
                result = selfCustomerDetails.CustomerId;
            }
            catch (Exception ex)
            {
                result = 0;
            }

            return result;
        }
        public Models.DBModels.TodayRate GetTodayRate(int appraiserId, int bankId)
        {
            _Context.Configuration.LazyLoadingEnabled = false;
            //var appriaserBanks = _Context.AppraiserBanks.Where(x => x.AppriaserId == appraiserId && x.IsActive == true).ToList();
            //var date = DateTime.Now;
            //var rate = _Context.TodayRates.Where(x => x.AppraiserId == appraiserId && x.BankId == bankId && x.CreatedOn.Year == date.Year && x.CreatedOn.Month == date.Month && x.CreatedOn.Day == date.Day).FirstOrDefault();
                var rate = _Context.TodayRates.OrderByDescending(x => x.Id).Where(x => x.AppraiserId == appraiserId && x.IsActive == true && x.BankId == bankId).FirstOrDefault();
            return rate;
        }
        public int SaveOrnaments(string userName, int customerId, Models.ViewModels.OrnamentDetailsModel model)
        {
            foreach (var item in model.ornamentsList)
            {
                item.CreatedBy = userName;
                item.ModifiedBy = userName;
                item.CreatedOn = DateTime.Now;
                item.ModifiedOn = DateTime.Now;
                item.CustomerId = customerId;
                item.IsActive = true;
                _Context.OrnamentDetails.Add(item);
            }
            _Context.SaveChanges();
            return 1;
        }
        public Models.ViewModels.OrnamentDetailsModel GetOrnamentDetails(int customerId, int BankId)
        {
            Models.ViewModels.OrnamentDetailsModel model = new Models.ViewModels.OrnamentDetailsModel();
            model.ornamentsList = _Context.OrnamentDetails.Where(x => x.CustomerId == customerId && x.LoanType == BankId && x.IsActive == true).ToList();
            return model;
        }
        public Models.ViewModels.ReceiptModel GenerateLoan(Models.DBModels.LoanDetails model, int appraiserId)
        {
            Models.ViewModels.ReceiptModel receipt = new Models.ViewModels.ReceiptModel();
            try
            {
                model.CreatedOn = DateTime.Now;
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

            int result = 1;
            try
            {
                model.CreatedOn = DateTime.Now;
                model.ModifiedOn = DateTime.Now;
                model.CreatedBy = userName;
                model.ModifiedBy = userName;
                model.AppraiserId = AppraiserId;
                model.IsActive = true;
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Models.ViewModels.BankCustomerModel, Models.DBModels.BankCustomerDetails>();
                });
                IMapper mapper = config.CreateMapper();
                var bankCustomerDetails = mapper.Map<Models.ViewModels.BankCustomerModel, Models.DBModels.BankCustomerDetails>(model);
                _Context.BankCustomerDetails.Add(bankCustomerDetails);
                _Context.SaveChanges();
                result = bankCustomerDetails.CustomerId;
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
                                                                 join loan in _Context.LoanDetails.Where(x => x.LoanType == 1) on self.CustomerId equals loan.CustomerId
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
                                                                 join loans in _Context.LoanDetails.Where(x => x.LoanType != 1) on bank.CustomerId equals loans.CustomerId
                                                                 join bankmaster in _Context.BankMasters.Where(x => x.IsActive == true) on loans.LoanType equals bankmaster.BankId
                                                                 select new Models.ViewModels.CustomerLoanData
                                                                 {
                                                                     Date = loans.CreatedOn,
                                                                     BankName = bankmaster.BankName,
                                                                     CustomerName = bank.Name,
                                                                     InterestRate = loans.Interest,
                                                                     LoanAmount = loans.LoanAmount,
                                                                     LoanId = loans.LoanId,
                                                                     MobileNumber = bank.MobileNumber,
                                                                     Area = bank.Address,
                                                                     IsActive = loans.IsActive
                                                                 }
                                      ).ToList();
            model.customerDataList = selfList.Concat(bankList);
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
                model.ornamentsList = _Context.OrnamentDetails.Where(x => x.CustomerId == model.loanDetails.CustomerId).ToList();
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
            if (loanType == 1)
            {
                var customer = _Context.SelfCustomerDetails.Where(x => x.CustomerId == customerId && x.IsActive == true).FirstOrDefault();
                model.Name = customer.Name;
                model.MobileNumber = customer.MobileNumber;
                model.Address = customer.Address;
                model.UANNumber = customer.UANNumber;
            }
            else
            {
                var customer = _Context.BankCustomerDetails.Where(x => x.CustomerId == customerId && x.IsActive == true).FirstOrDefault();
                model.Name = customer.Name;
                model.MobileNumber = customer.MobileNumber;
                model.Address = customer.Address;
            }
            return model;
        }

        public Models.DBModels.SubscriptionDetails GetSubscriptionDetails( int appraiserId)
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

            return subscription;
        }

        public void payment()
        {
            var mid = ConfigurationManager.AppSettings["MerchantID"].ToString();
            var clientSecret = ConfigurationManager.AppSettings["AccountSecretKey"].ToString();
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

            /* Enter your unique order id */
            paytmParams.Add("ORDER_ID", "123456789");

            /* unique id that belongs to your customer */
            paytmParams.Add("CUST_ID", "8");

            /* customer's mobile number */
            paytmParams.Add("MOBILE_NO", "9030834737");

            /* customer's email */
            paytmParams.Add("EMAIL", "tsreenu.9@gmail.com");

            /**
            * Amount in INR that is payble by customer
            * this should be numeric with optionally having two decimal points
*/
            paytmParams.Add("TXN_AMOUNT", "1");

            /* on completion of transaction, we will send you the response on this URL */
            paytmParams.Add("CALLBACK_URL", "http://localhost:26137/Apraiser/TodayRate");

            /**
            * Generate checksum for parameters we have
            * You can get Checksum DLL from https://developer.paytm.com/docs/checksum/
            * Find your Merchant Key in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys 
*/
            String checksum = paytm.CheckSum.generateCheckSum("YOUR_KEY_HERE", paytmParams);

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
        }
    }
}