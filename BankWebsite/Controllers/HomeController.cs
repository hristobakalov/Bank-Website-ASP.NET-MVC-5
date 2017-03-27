using reCAPTCHA.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BankWebsite.Models;

namespace BankWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            

            ViewBag.Title = "Contact";

            return View();
        }

        [HttpPost]
        [CaptchaValidator]
        public ActionResult ContactPost(FormCollection formCollection, bool captchaValid)
        {
            string email = formCollection["email"];
            string message = formCollection["message"];

            bool emailValid = false;

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                emailValid = true;
            }
            else
            {
                ViewBag.FormMessage = "Email not correct! ";
            }

            if (!ModelState.IsValid)
            {
                ViewBag.FormMessage = "Captcha not valid! ";
            }

            if(emailValid && ModelState.IsValid)
            {
                ViewBag.FormMessage = "Mail is sent! ";
                sendEmail(email, message);
            }
            ViewBag.Title = "Contact";

            return View("Contact");
        }

        public ActionResult SignUp()
        {

            ViewBag.Title = "SignUp";

            return View();
        }

        [HttpGet]
        public ActionResult SignUp2()
        {
            
            ViewBag.Title = "SignUp2";

            return View();
        }

        [HttpPost]
        public ActionResult SignUp2Post(User user)
        {
            User newUser = user;
            bool isValid = ModelState.IsValid;
            

            
            
            return View("SignUp2");
        }

        private void sendEmail(string email, string message)
        {
            SmtpClient client = new SmtpClient();

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("someone@somewhere.com");
            mailMessage.To.Add(email);
            mailMessage.Subject = "KeepX Bank Website Contact Mail";
            mailMessage.Body = message;
            client.Send(mailMessage);
        }


    }
}
