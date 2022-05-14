using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACEBankingApp.Models
{
    public class AccountModel
    {
        public CommonMessageModel msg { get; set; }
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public decimal Amount { get; set; }
        public string CustomerId { get; set; }
        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MM yyyy}")]
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Version { get; set; }
        public bool IsDeleted { get; set; }
        public List<Account> lstAccount { get; set; }
    }

    public class Account
    {
        public string AccountNo { get; set; }
        public string AccountType { get; set; }
        public decimal Amount { get; set; }
        public string CustomerId { get; set; }
        [DisplayName("Date")]
        [DisplayFormat(DataFormatString = "{0:dd MM yyyy}")]
        public DateTime CreateDate { get; set; }
        //transaction History
    }

    public class CreateTransfer
    {
        public CommonMessageModel msg { get; set; }
        public string SenderAccountNo { get; set; }
        public string ReceiverAccountNo { get; set; }
        public decimal TransferAmount { get; set; }
       
    }
    public class TopUp
    {
        public CommonMessageModel msg { get; set; }
        public string SenderAccountNo { get; set; }
        public string OperatorName { get; set; }
        public string TransferAmount { get; set; }
    }

    public class TransactionByCustomerModel
    {
        public CommonMessageModel msg { get; set; }
        public List<TransactionByCustomer> lstTransactionByCustomer { get; set; } = new List<TransactionByCustomer>();
        public int lstTransactionByCustomerTotal { get { return lstTransactionByCustomer == null ? 0 : lstTransactionByCustomer.Count; } }

    }
    public class TransactionByCustomer //class for customer inqeury
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SenderAccountNo { get; set; }
        public string ReceiverAccountNo { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }

    }

    public class TopUpListModel
    {
        public CommonMessageModel msg { get; set; }
        public string AccountType { get; set; }
        public string AccountNo { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public List<TopUpList> lstTopUpList { get; set; } = new List<TopUpList>();
    }

    public class TopUpList
    {
        public string AccountType { get; set; }
        public string AccountNo { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}