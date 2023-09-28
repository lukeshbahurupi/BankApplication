using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class Login_Signup
    { 
        public Login Login { get; set; }
        public User Signup { get; set; }

    }
}