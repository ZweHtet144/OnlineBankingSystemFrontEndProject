using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACEBankingApp.Models
{
    public class CustomerLoginViewModel
    {
        public CommonMessageModel msg { get; set; }
        
        public int CustomerNo { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string OldPassword { get; set; }
        public string NRC { get; set; }
        public string Phoneno { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<Customer> lstCustomer { get; set; }
        public List<CustomerProfileEdit> lstCustomerProfileEdit { get; set; }

    }
    public class Customer
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string NRC { get; set; }
        public string Phoneno { get; set; }
        public string Address { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public decimal Amount { get; set; }
        public DateTime AccountCreatedDate { get; set; }

    }

    public class CustomerProfileEdit
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string NRC { get; set; }
        public string Phoneno { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }


}
