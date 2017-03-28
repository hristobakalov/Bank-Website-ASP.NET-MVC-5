using BankWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace BankWebsite.Controllers
{
    public class RedirectNotSignedIn : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = HttpContext.Current.Session["userCredentionals"] as User;
            // The action filter logic.
            if (user == null || user.IsDisabled)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));

            }

        }
    }
}