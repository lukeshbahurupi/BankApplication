using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class Login
    {
        public int ID { get; set; }
        [Required]
        [RegularExpression(@"^[\w-\.]+@([a-z]+\.)+[a-z]{2,4}$", ErrorMessage = "Email is not in correct format!")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,10}$", ErrorMessage = "Minimum six and maximum 10 characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}