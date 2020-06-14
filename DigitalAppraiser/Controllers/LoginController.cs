using System;
using System.Web.Mvc;
using BL = DigitalAppraiser.BuinessLogic;
using System.Web.Security;
using DigitalAppraiser.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace DigitalAppraiser.Controllers
{

    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            Models.ViewModels.LoginModel model = new Models.ViewModels.LoginModel();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {
            BL.Interfaces.LoginInterface login = new BL.Implementation.LoginClass();
            ModelState["NewPassword"].Errors.Clear();
            if (ModelState.IsValid == true)
            {
                LoginModel isValidUser = login.Login(model.MobileNumber, model.Password);
                if (isValidUser.ErrorMessage == "Valid User")
                {
                    FormsAuthentication.SetAuthCookie(isValidUser.MobileNumber, false);
                    LogedUser.MobileNumber = isValidUser.MobileNumber;
                    LogedUser.UserName = isValidUser.UserName;
                    LogedUser.AppraiserId = isValidUser.AppraiserId;
                    return RedirectToAction("TodayRate", "Apraiser");
                }
                else
                {
                    model.ErrorMessage = isValidUser.ErrorMessage;
                    return View(model);
                }
            }
            else
            {
                //model.ErrorMessage = "Please fill all mandatory fields.";
                return View(model);
            }
        }
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            Models.ViewModels.SignUpModel signUpModel = new Models.ViewModels.SignUpModel();
            BL.Interfaces.LoginInterface bl = new BL.Implementation.LoginClass();
            if (LogedUser.AppraiserId != null)
            {
                int apraiserId = LogedUser.AppraiserId.Value;
                signUpModel = bl.GetAppraiserDetail(apraiserId);
                signUpModel.selectedBanks = bl.GetSelectedBanks(apraiserId);
            }
            signUpModel.StatesList = BindStates();
            var city = new List<SelectListItem>();
            city.Add(new SelectListItem { Text = "Select" });
            signUpModel.CitiesList = city;
            signUpModel.bankList = BindBanks();
            return View(signUpModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignUp(Models.ViewModels.SignUpModel signUpModel)
        {
            BL.Interfaces.LoginInterface bl = new BL.Implementation.LoginClass();
            int result = 0;
            if (signUpModel.IsSelfLoan == false)
            {
                ModelState["ShopName"].Errors.Clear();
            }

            if (ModelState.IsValid == true)
            {
                result = bl.SignUpUser(signUpModel);
                if (Session["AppraiserId"] != null)
                {
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                }
            }
            if (result == 1)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                signUpModel.StatesList = BindStates();
                var city = new List<SelectListItem>();
                city.Add(new SelectListItem { Text = "Select", Value = "0" });
                signUpModel.CitiesList = city;
                signUpModel.bankList = BindBanks();
                return View(signUpModel);
            }
        }
        [AllowAnonymous]
        public JsonResult GetCities(int stateId)
        {
            BL.Interfaces.LoginInterface bl = new BL.Implementation.LoginClass();
            var cities = bl.GetCities(stateId);
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            Models.ViewModels.LoginModel model = new Models.ViewModels.LoginModel();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(LoginModel model)
        {
            BL.Interfaces.LoginInterface bl = new BL.Implementation.LoginClass();
            LoginModel result = new LoginModel();
            if (ModelState.IsValid == true)
            {
                result = bl.ChangePwd(model.MobileNumber, model.Password, model.NewPassword);
                if (result.ErrorMessage == "Password changed successfully")
                {
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                    return RedirectToAction("Login");
                }
            }
            model.ErrorMessage = result.ErrorMessage;
            return View(model);
        }
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            LogedUser.AppraiserId = null;
            LogedUser.MobileNumber = null;
            LogedUser.UserName = null;
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }

        private IEnumerable<SelectListItem> BindStates()
        {
            BL.Interfaces.LoginInterface bl = new BL.Implementation.LoginClass();
            var states = bl.GetStates();
            var stateList = from s in states
                            select new SelectListItem
                            {
                                Text = s.StateName,
                                Value = s.StateId.ToString()
                            };
            return stateList;
        }
        private IEnumerable<SelectListItem> BindBanks()
        {
            BL.Interfaces.LoginInterface bl = new BL.Implementation.LoginClass();
            var banks = bl.BanksList();
            var bankList = from s in banks
                           select new SelectListItem
                           {
                               Text = s.BankName,
                               Value = s.BankId.ToString()
                           };
            return bankList;
        }
    }
}