using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    [MetadataType(typeof(ApplicationMetadata))]
    public partial class ApplicationForm
    {

    }
    public class ApplicationMetadata
    {
        [Required]
        public string Gender { get; set; }
        [Required]
        public System.DateTime BirthDate { get; set; }
        [Required]
        public int Age { get; set; }        
        public string EmailId { get; set; }
        //[RegularExpression(@"^[0-9]{10}$")]
        public string PhoneNumber { get; set; }
        //[Required]
        public string PancardNumber { get; set; }
        [RegularExpression(@"^[2-9]{1}[0-9]{3}\s[0-9]{4}\s[0-9]{4}$",ErrorMessage = "Please add valid number '2000 0000 0000' ")]
        public string AadhaarCardNumber { get; set; }
        [Required]
        public string PermanentAddress { get; set; }
        [Required]
        public string CurrentAddress { get; set; }
        [Required]
        public string LoanType { get; set; }
        [Required]
        [RegularExpression(@"^(?!0)\d+(\.\d+)?$",ErrorMessage = "Invalid!")]
        public decimal RequiredLoanAmount { get; set; }
        [Required]
        [RegularExpression(@"^(?!0)\d+(\.\d+)?$", ErrorMessage = "Invalid!")]
        public decimal RateOfInterest { get; set; }
        [Required]
        [RegularExpression(@"^(?!0)\d{1,3}(\.\d+)?$", ErrorMessage = "Invalid!")]
        public int LoanTenureInMonth { get; set; }
        [Required]
        [RegularExpression(@"^(?!0)\d+(\.\d+)?$", ErrorMessage = "Invalid!")]
        public decimal LoanEMI { get; set; }
        [Required]
        public string Summary { get; set; }
    }
}