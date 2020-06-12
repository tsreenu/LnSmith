using DigitalAppraiser.Models.DBModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BL = DigitalAppraiser.BuinessLogic;
using Models = DigitalAppraiser.Models;

namespace DigitalAppraiser.Controllers
{
    [Authorize]
    [SessionTimeoutAttribute]
    public class ApraiserController : Controller
    {
        // GET: Apraiser
        public ActionResult DashBoard()
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            //int AppraiserId = LogedUser.AppraiserId.Value;
            //var model = bl.GetUserDateRates(AppraiserId);
            return View();
        }
        public ActionResult TodayRate()
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int AppraiserId = LogedUser.AppraiserId.Value;
            var model = bl.GetUserDateRates(AppraiserId);
            return View(model);
        }
        [HttpPost]
        public ActionResult TodayRate(Models.ViewModels.TodayRateModel model)
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            if (ModelState.IsValid == true)
            {
                string userName = LogedUser.UserName;
                var result = bl.SaveTodayRate(model, userName);
                return RedirectToAction("ProcessLoan");
            }
            int AppraiserId = LogedUser.AppraiserId.Value;
            model = bl.GetUserDateRates(AppraiserId);
            return View(model);
        }
        [HttpGet]
        public ActionResult ProcessLoan()
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int AppraiserId = LogedUser.AppraiserId.Value;
            Models.ViewModels.ProcessLoanModel model = new Models.ViewModels.ProcessLoanModel();
            model.bankCustomer = new Models.ViewModels.BankCustomerModel();
            model.bankCustomer.Banks = bl.GetAppraiserBanks(AppraiserId);
            model.selfCustomer = new Models.ViewModels.SelfCustomerModel();
            int[] list = Enumerable.Range(1, 30).ToArray();
            model.selfCustomer.Quantity = list.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            });
            var relList = new[] { new { Name = "Son of", Value = 1 }, new { Name = "Daughter of", Value = 2 }, new { Name = "Wife of", Value = 3 } };
            //model.BankId = BankId;
            model.bankCustomer.RelationTypes = relList.Select(x => new SelectListItem
            {
                Value = x.Value.ToString(),
                Text = x.Name
            });
            model.bankCustomer.Quantity = list.Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            });
            ViewBag.TodayRate = bl.GetTodayRate(AppraiserId, 1);
            model.bankCustomer.bankCustomer = new BankCustomerDetails();
            model.bankCustomer.ornamentDetails = new OrnamentDetails();
            model.selfCustomer.selfCustomer = new SelfCustomerDetails();
            model.selfCustomer.ornamentDetails = new OrnamentDetails();
            return View(model);
        }
        [HttpGet]
        public ActionResult SelfCustomer()
        {
            var model = new Models.ViewModels.SelfCustomerModel();
            return PartialView("SelfCustomer", model);
        }
        [HttpPost]
        public ActionResult SelfCustomer(Models.ViewModels.SelfCustomerModel model)
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int AppraiserId = LogedUser.AppraiserId.Value;
            string userName = LogedUser.UserName;
            if (ModelState.IsValid == true)
            {
                int customerId = bl.SaveSelfCustomerDetails(model, AppraiserId, userName);
                return RedirectToAction("Estimation", new { customerId = customerId });
            }
            else
            {
                Models.ViewModels.ProcessLoanModel procModel = new Models.ViewModels.ProcessLoanModel();
                procModel.bankCustomer = new Models.ViewModels.BankCustomerModel();
                procModel.bankCustomer.Banks = bl.GetAppraiserBanks(AppraiserId);
                procModel.selfCustomer = model;
                int[] list = Enumerable.Range(1, 30).ToArray();
                procModel.selfCustomer.Quantity = list.Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.ToString()
                });
                var relList = new[] { new { Name = "Son of", Value = 1 }, new { Name = "Daughter of", Value = 2 }, new { Name = "Wife of", Value = 3 } };
                //model.BankId = BankId;
                procModel.bankCustomer.RelationTypes = relList.Select(x => new SelectListItem
                {
                    Value = x.Value.ToString(),
                    Text = x.Name
                });
                procModel.bankCustomer.Quantity = list.Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.ToString()
                });
                procModel.ActiveTab = "Self";
                return View("ProcessLoan", procModel);
            }
        }
        [HttpGet]
        public ActionResult GetTodayRate(int bankId)
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int AppraiserId = LogedUser.AppraiserId.Value;
            TodayRate todayRate = new TodayRate();
            todayRate = bl.GetTodayRate(AppraiserId, bankId);
            return Json(todayRate, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Estimation(int customerId)
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            Models.ViewModels.OrnamentDetailsModel model = new Models.ViewModels.OrnamentDetailsModel();
            model.customerId = customerId;
            string userName = LogedUser.UserName;
            model.ornamentsList = bl.GetOrnamentDetails(customerId).ornamentsList;
            model.loanDetails = new Models.DBModels.LoanDetails();
            model.loanDetails.CustomerId = customerId;
            model.loanDetails.CreatedBy = userName;
            model.loanDetails.ModifiedBy = userName;
            model.loanDetails.LoanType = model.ornamentsList[0].LoanType;
            var customer = bl.customerDetails(model.loanDetails.LoanType, customerId);
            model.CustomerName = customer.selfCustomer.Name;
            model.MobileNumber = customer.selfCustomer.MobileNumber;
            model.Address = customer.selfCustomer.Address;
            model.Aadhar = customer.selfCustomer.UANNumber;
            return View(model);
        }
        [HttpPost]
        public ActionResult GenerateLoan(Models.DBModels.LoanDetails model)
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int AppraiserId = LogedUser.AppraiserId.Value;
            var returnModel = bl.GenerateLoan(model, AppraiserId);
            if (returnModel.loanDetails.LoanType == 1)
            {
                return PartialView("SelfReceipt", returnModel);
            }
            else
            {
                return PartialView("SbiReceipt", returnModel);
            }
        }
        [HttpGet]
        public ActionResult BankCustomer(int BankId)
        {
            Models.ViewModels.BankCustomerModel model = new Models.ViewModels.BankCustomerModel();
            var relList = new[] { new { Name = "Son of", Value = 1 }, new { Name = "Daughter of", Value = 2 }, new { Name = "Wife of", Value = 3 } };
            //model.BankId = BankId;
            model.RelationTypes = relList.Select(x => new SelectListItem
            {
                Value = x.Value.ToString(),
                Text = x.Name
            });
            return PartialView("BankCustomer", model);
        }
        [HttpPost]
        public ActionResult BankCustomer(Models.ViewModels.BankCustomerModel model)
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int AppraiserId = LogedUser.AppraiserId.Value;
            string userName = LogedUser.UserName;
            //int BankId = model.BankId;
            if (ModelState.IsValid == true)
            {
                int customerId = bl.SaveBankCustomerDetails(model, AppraiserId, userName);
                return RedirectToAction("Estimation", new { customerId = customerId });
            }
            else
            {
                Models.ViewModels.ProcessLoanModel procModel = new Models.ViewModels.ProcessLoanModel();
                procModel.selfCustomer = new Models.ViewModels.SelfCustomerModel();
                procModel.bankCustomer = model;
                procModel.bankCustomer.Banks = bl.GetAppraiserBanks(AppraiserId);
                int[] list = Enumerable.Range(1, 30).ToArray();
                procModel.selfCustomer.Quantity = list.Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.ToString()
                });
                var relList = new[] { new { Name = "Son of", Value = 1 }, new { Name = "Daughter of", Value = 2 }, new { Name = "Wife of", Value = 3 } };
                //model.BankId = BankId;
                procModel.bankCustomer.RelationTypes = relList.Select(x => new SelectListItem
                {
                    Value = x.Value.ToString(),
                    Text = x.Name
                });
                procModel.bankCustomer.Quantity = list.Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.ToString()
                });
                procModel.ActiveTab = "Bank";
                return View("ProcessLoan", procModel);
            }
        }
        public ActionResult SbiReceipt()
        {
            return PartialView("SbiReceipt");
        }
        public ActionResult CustomerLoanData()
        {
            Models.ViewModels.CustomerLoanDataModel model = new Models.ViewModels.CustomerLoanDataModel();
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int AppraiserId = LogedUser.AppraiserId.Value;
            model = bl.GetCustomerLoanData(AppraiserId);
            return View("CustomerLoanData", model);
        }
        [HttpGet]
        public ActionResult CollectLoan()
        {
            if (TempData["LoanNotFound"] != null)
            {
                ViewBag.LoanNotFound = TempData["LoanNotFound"];
            }
            Models.DBModels.LoanDetails loanDetails = new LoanDetails();
            return View("CollectLoanDetails", loanDetails);
        }
        [HttpGet]
        public ActionResult LoanDetails(string LoanId)
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            var model = bl.GetLoanDetails(LoanId);
            if (model.errMessage == "")
            {
                return View(model);
            }
            else
            {
                TempData["LoanNotFound"] = model.errMessage;
                return RedirectToAction("CollectLoan");
            }
        }
        [HttpPost]
        public ActionResult SettleLoan(string loanId, decimal collectedAmount, string collectedOn, decimal CollectedInterest, int NoOfDays)
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int result = bl.SettleLoan(loanId, collectedAmount, collectedOn, CollectedInterest, NoOfDays);
            return Json(result);
        }
        public void DownLoadExcel(int bankId)
        {
            Models.ViewModels.CustomerLoanDataModel model = new Models.ViewModels.CustomerLoanDataModel();
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int AppraiserId = LogedUser.AppraiserId.Value;
            model = bl.GetCustomerLoanData(AppraiserId);

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("CustomerData");
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;

            //Header of table  
            //  
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Cells[1, 1].Value = "S No";
            workSheet.Cells[1, 2].Value = "Date";
            workSheet.Cells[1, 3].Value = "Loan Type";
            workSheet.Cells[1, 4].Value = "Loan ID";
            workSheet.Cells[1, 5].Value = "Name of the Customer";
            workSheet.Cells[1, 6].Value = "Mobile Number";
            workSheet.Cells[1, 7].Value = "Area";
            workSheet.Cells[1, 8].Value = "Loan Amount";
            workSheet.Cells[1, 9].Value = "Rate of Interest";
            workSheet.Cells[1, 10].Value = "Loan Status";
            //Body of table  
            //  
            int recordIndex = 2;
            IEnumerable<Models.ViewModels.CustomerLoanData> CustomerLoanData = new List<Models.ViewModels.CustomerLoanData>();
            if (bankId == 1)
            {
                CustomerLoanData = model.selfCustomerDataList;
            }
            else
            {
                CustomerLoanData = model.bankCustomerDataList;
            }
            foreach (var customer in CustomerLoanData)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = customer.Date.Date.ToShortDateString();
                workSheet.Cells[recordIndex, 3].Value = customer.BankName;
                workSheet.Cells[recordIndex, 4].Value = customer.LoanId;
                workSheet.Cells[recordIndex, 5].Value = customer.CustomerName;
                workSheet.Cells[recordIndex, 6].Value = customer.MobileNumber;
                workSheet.Cells[recordIndex, 7].Value = customer.Area;
                workSheet.Cells[recordIndex, 8].Value = customer.LoanAmount;
                workSheet.Cells[recordIndex, 9].Value = customer.InterestRate;
                workSheet.Cells[recordIndex, 10].Value = customer.IsActive == false ? "Settled" : "Active";
                recordIndex++;
            }
            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();
            workSheet.Column(4).AutoFit();
            workSheet.Column(5).AutoFit();
            workSheet.Column(6).AutoFit();
            workSheet.Column(7).AutoFit();
            workSheet.Column(8).AutoFit();
            workSheet.Column(9).AutoFit();
            workSheet.Column(10).AutoFit();
            string excelName = "CustomerData" + DateTime.Now.ToShortDateString();

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
        public ActionResult Settings()
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int AppraiserId = LogedUser.AppraiserId.Value;
            Models.ViewModels.SignUpModel model = new Models.ViewModels.SignUpModel();

            return RedirectToAction("SignUp", "Login");
        }

        public ActionResult Subscription()
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            int appraiserId = LogedUser.AppraiserId.Value;
            Models.DBModels.SubscriptionDetails subscription = bl.GetSubscriptionDetails(appraiserId);
            return View(subscription);
            //bl.payment();
            //return View();
        }

        public ActionResult Payment(int planId)
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            ViewBag.paytm = bl.payment(planId);
            return View("PaymentPage");
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult PaytmResponse(PaytmResponse paytmResponse)
        {
            BL.Interfaces.AppriserInterface bl = new BL.Implementation.AppraiserClass();
            var res = bl.AddPaymentDetails(paytmResponse);
            return RedirectToAction("Dashboard");
        }
    }
}