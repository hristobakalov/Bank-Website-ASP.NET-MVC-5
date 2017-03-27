using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BankWebsite.Models
{
    public class User
    {
        private string firstname;
        
        [Required]
        [DisplayName("First Name")]
        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        private string lastname;

        [Required]
        [DisplayName("Last Name")]
        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        private string email;

        [Required]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string password;

        [Required]
        [DisplayName("Password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string password1;
        [DisplayName("Repeat Password ")]
        public string Password1
        {
            get { return password1; }
            set { password1 = value; }
        }

        private string dateBirth;

        [Required]
        [DisplayName("date of Birth")]
        public string DateBirth
        {
            get { return dateBirth; }
            set { dateBirth = value; }
        }

        private bool condIsChecked;

        [Required]
        [DisplayName("I have read the fake conditions checkbox")]
        public bool CondIsChecked
        {
            get { return condIsChecked; }
            set { condIsChecked = value; }
        }


        public User()
        {

        }

    }
}