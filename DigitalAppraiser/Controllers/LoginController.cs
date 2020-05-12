using System;
using System.Web.Mvc;
using BL = DigitalAppraiser.BuinessLogic;
using System.Web.Security;

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
        public ActionResult Login(string mobileNumber, string password)
        {
            BL.Interfaces.LoginInterface login = new BL.Implementation.LoginClass();
            var isValidUser = login.Login(mobileNumber, password);
            if (isValidUser.ErrorMessage == "Valid User")
            {
                FormsAuthentication.SetAuthCookie(isValidUser.MobileNumber, false);
                LogedUser.MobileNumber = isValidUser.MobileNumber;
                LogedUser.UserName = isValidUser.UserName;
                LogedUser.AppraiserId = isValidUser.AppraiserId;
                //return RedirectToAction("Dashbord", "Apraiser");
            }
            //else
            //{
                return Json(isValidUser);
            //}
        }
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            Models.ViewModels.SignUpModel signUpModel = new Models.ViewModels.SignUpModel();
            BL.Interfaces.LoginInterface bl = new BL.Implementation.LoginClass();
            if(Session["AppraiserId"] != null)
            {
                int apraiserId = Convert.ToInt32(Session["AppraiserId"]);
                signUpModel = bl.GetAppraiserDetail(apraiserId);
                signUpModel.selectedBanks = bl.GetSelectedBanks(apraiserId);
            }
            signUpModel.Banks = bl.BanksList();
            signUpModel.States = bl.GetStates();
            return View(signUpModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignUp(Models.ViewModels.SignUpModel signUpModel)
        {
            BL.Interfaces.LoginInterface bl = new BL.Implementation.LoginClass();
            var result = bl.SignUpUser(signUpModel);
            if (Session["AppraiserId"] != null)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
            }
            var res = result == 1 ? Url.Action("Login", "Login") : "Mobile number already exists";
            return Json(res);
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
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(string MobileNumber, string Password, string NewPassword)
        {
            BL.Interfaces.LoginInterface bl = new BL.Implementation.LoginClass();
            var result = bl.ChangePwd(MobileNumber, Password, NewPassword);
            FormsAuthentication.SignOut();
            Session.Abandon();
            return Json(result);         
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
    }
}