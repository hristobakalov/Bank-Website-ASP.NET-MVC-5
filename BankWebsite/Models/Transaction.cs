using SoSimpleDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankWebsite.Models
{
    public class Transaction : SoSimpleDbModelBase
    {
        public int UserId { set; get; }
        public DateTime Date { set; get; }
        public string OperationType { set; get; }

        public double Amount { set; get; }

        
        public static int getNewID()
        {
            var currentCount = SoSimpleDb<Transaction>.Instance.Count();
            return currentCount;
        }

        public Transaction()
        {

        }
    }
}