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

            var user = System.Web.HttpContext.Current.Session["userCredentionals"] as User;
            Func<Transaction, bool> searchFunc = (x) => x.UserId == (user.Id);
            var transactions = SoSimpleDb<Transaction>.Instance.Select(searchFunc);

            System.Web.HttpContext.Current.Session["Transactions"] = transactions.OrderByDescending(x => x.Date);

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

        [RedirectNotSignedIn]
        public ActionResult Operations()
        {
            ViewBag.CurrBalance = (System.Web.HttpContext.Current.Session["userCredentionals"] as User).Balance;
            return View();
        }

        [RedirectNotSignedIn]
        [HttpPost]
        public ActionResult Operations(FormCollection formCollection)
        {
            double depositAmount = 0;
            bool resultParse = Double.TryParse(formCollection["amount"], out depositAmount);
            if (!resultParse)
            {
                System.Web.HttpContext.Current.Session["OperationsMessage"] = "You should input a number!";
                ViewBag.OperationsMessage = "You should input a number!";
                return View();
            }
            string operationsMessage = "";
            if (depositAmount <= 0)
            {
                System.Web.HttpContext.Current.Session["OperationsMessage"] = "You can not input negative amount!";
                operationsMessage = "You can not input negative amount";
            }
            else
            {
                var user = System.Web.HttpContext.Current.Session["userCredentionals"] as User;
                operationsMessage = String.Format("You have added {0}$ to your balance (before: {1} - after {2})",
                    depositAmount, user.Balance, user.Balance + depositAmount);
                user.Balance += depositAmount;
                SoSimpleDb<User>.Instance.Update(user);

                createTransactionAndsaveDB(user, depositAmount, "Deposit");
            }
            System.Web.HttpContext.Current.Session["OperationsMessage"] = operationsMessage;
            ViewBag.OperationsMessage = operationsMessage;
            return View();
        }


        [RedirectNotSignedIn]
        [HttpPost]
        public ActionResult WithdrawalMoneyPost(FormCollection formCollection)
        {

            string input = formCollection["amountWithdrawal"];
            double withdrawAmount = 0;
            bool resultParse = Double.TryParse(input, out withdrawAmount);
            string operationsMessage = "";
            var user = System.Web.HttpContext.Current.Session["userCredentionals"] as User;


            if (!resultParse)
            {
                operationsMessage = "You should input a number!";
                return RedirectToAction("Operations");
            }
            
            if (withdrawAmount <= 0)
            {
                operationsMessage = "You can not input negative amount";
            }
            else if (withdrawAmount > user.Balance)
            {
                operationsMessage = "The user doesn't have that much money";
            }
            else
            {

                operationsMessage = String.Format("Your cash withdrawal of {0}$ has been received (before: {1} - after {2})",
                    withdrawAmount, user.Balance, user.Balance - withdrawAmount);
                user.Balance -= withdrawAmount;
                SoSimpleDb<User>.Instance.Update(user);

                createTransactionAndsaveDB(user, withdrawAmount, "Cash Withdraw");
            }
            ViewBag.OperationsMessage = operationsMessage;
            ViewBag.WithdrawAmount = withdrawAmount;
            ViewBag.BalanceAfterWithdraw = user.Balance;
            System.Web.HttpContext.Current.Session["OperationsMessage"] = operationsMessage;
            return RedirectToAction("Operations");
            // return View("Operations");
        }


        private void createTransactionAndsaveDB(User user,double amount, string operationTypeStr)
        {
            Transaction currTransaction = new Transaction();
            currTransaction.Amount = amount;
            currTransaction.Date = DateTime.Now;
            currTransaction.OperationType = operationTypeStr;
            currTransaction.UserId = user.Id;
            currTransaction.Id = Transaction.getNewID();
            SoSimpleDb<Transaction>.Instance.Insert(currTransaction);
        }
    }
}
