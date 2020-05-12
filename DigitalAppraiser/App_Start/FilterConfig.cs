using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DigitalAppraiser
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
        }
    }
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (LogedUser.AppraiserId.HasValue == false)
            {
                filterContext.Result = new RedirectResult("~/Login/Login");
                return;
            }
            FormsAuthentication.SetAuthCookie(LogedUser.MobileNumber, false);
            base.OnActionExecuting(filterContext);
        }
    }
    public static class LogedUser
    {
        public static string MobileNumber { get; set; }
        public static int? AppraiserId { get; set; }
        public static string UserName { get; set; }
    }
}
