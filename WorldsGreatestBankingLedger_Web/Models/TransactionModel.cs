using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldsGreatestBankingLedger_Web.Models
{
    public class TransactionModel
    {
        public TransactionModel()
        {
            TransactionDate = DateTime.Now;
        }
        public TransactionModel(string accountId, float amount, DateTime transactionDate)
        {
            AccountId = accountId;
            Amount = amount;
            TransactionDate = transactionDate;
        }
        public string AccountId { get; set; }
        public float Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
