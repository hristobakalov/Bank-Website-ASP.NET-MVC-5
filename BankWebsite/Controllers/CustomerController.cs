using BankWebsite.Models;
using SoSimpleDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankWebsite.Controllers
{
    
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [RedirectNotSignedIn]
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult UserActivation()
        {

            int userId = Convert.ToInt32(this.Request.QueryString["id"]);
            string userToken = this.Request.QueryString["token"];
            var user = SoSimpleDb<User>.Instance.Select(userId);
            bool isUserAuthenticate;
            if (user != null)
            {
                isUserAuthenticate = user.Token == userToken;
                if (isUserAuthenticate)
                {
                    user.IsDisabled = false;
                    SoSimpleDb<User>.Instance.Update(user);
                    ViewBag.ActivationMessage = "Your account is activated!";
                }
                else
                {
                    ViewBag.ActivationMessage = "Your token sucks!";
                }
            }
             

            return View();
        }
    }
}