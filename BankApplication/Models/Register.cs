using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User{}

    public class UserMetaData
    {
        public int ID { get; set; }
        [Required]
        [RegularExpression(@"^([A-Z]{1}[a-z]{2,})+\s+([A-Z]{1}[a-z]{2,})$", ErrorMessage = "Please Follow the format 'John Reddy'")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^[\w-\.]+@([a-z]+\.)+[a-z]{2,4}$", ErrorMessage = "Email is not in correct format!")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a value equal to 10 digit.")]
        public string MobileNumber { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{10}$", ErrorMessage = "Number is not correct!")] 
        public string UniqueId { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,10}$", ErrorMessage = "Minimum six and maximum 10 characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        [Display(Name = "Password")]        
        public string UserPassword { get; set; }
        
    }
}