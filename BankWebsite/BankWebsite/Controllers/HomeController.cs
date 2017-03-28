using reCAPTCHA.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BankWebsite.Models;
using SoSimpleDb;

namespace BankWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            ViewBag.LoginSuccessful = false;
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

            if (emailValid && ModelState.IsValid)
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

        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
        {
            string email = formCollection["email"];
            string password = formCollection["password"];

            //Func<User, bool> searchFunc = (x) => x.Email.Contains(email) && x.Password.Contains(password);
            //var persons = SoSimpleDb<User>.Instance.Select(searchFunc).ToList();
            var persons = SoSimpleDb<User>.Instance.Select(x => x.Email.Contains(email) && x.Password.Contains(password)).ToList();

            var loginSuccess = persons.Count != 0;
            ViewBag.LoginSuccessful = loginSuccess;
            if (loginSuccess && !persons[0].IsDisabled)
            {
                //ViewBag.FirstName = persons[0].Firstname;
                //ViewBag.LastName = persons[0].Lastname;
                System.Web.HttpContext.Current.Session["userCredentionals"] = persons[0];
                var currUser = System.Web.HttpContext.Current.Session["userCredentionals"] as User;
                return RedirectToAction("../Customer/Dashboard");
              // return View("../Customer/Dashboard");
            }

            
            return View("Index");
        }

        public ActionResult SignOut()
        {
            System.Web.HttpContext.Current.Session["userCredentionals"] = null;
            return View("Index");
        }

        [HttpPost]
        public ActionResult SignUp2Post(User user)
        {
            User newUser = user;
            bool isValid = ModelState.IsValid;
            string errorMessages = "";
            if (!newUser.CondIsChecked)
            {
                errorMessages += "Please read our stupid rules! ";
            }
            if (isValid && newUser.CondIsChecked)
            {
                if (newUser.Password != newUser.Password1)
                {
                    errorMessages += "The user passwords do not match! ";
                }
                else
                {
                    var personCount = SoSimpleDb<User>.Instance.Count();
                    newUser.Id = personCount;
                    string randToken = Guid.NewGuid().ToString();
                    newUser.Token = randToken;

                    SoSimpleDb<User>.Instance.Insert(newUser);

                    string message = String.Format("Please click the confirmation link: http://localhost:51669/Customer/UserActivation?id={0}&token={1}", newUser.Id, randToken);
                    sendEmail(newUser.Email, message);
                    return RedirectToAction("EmailSent");
                   // return View("EmailSent");

                }


            }

            ViewBag.ErrorMessage = errorMessages;
            return View("SignUp2");
        }

        [HttpGet]
        public ActionResult SignUp2()
        {

            ViewBag.Title = "SignUp2";

            return View();
        }

        public ActionResult EmailSent()
        {

            ViewBag.Title = "EmailSent";

            return View();
        }
        private void sendEmail(string email, string message)
        {
            SmtpClient client = new SmtpClient();

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("hristobak@gmail.com");
            mailMessage.To.Add(email);
            mailMessage.Subject = "KeepX Bank Website Contact Mail";
            mailMessage.Body = message;
            client.Send(mailMessage);
        }


    }
}
