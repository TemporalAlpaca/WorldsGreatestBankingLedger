using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsGreatestBankingLedger_Web.Models;

namespace WorldsGreatestBankingLedger_Web.Repositories
{
    public interface IBankingRepository
    {
        void InsertAccount(AccountModel account);
        void InsertTransaction(TransactionModel transaction);

        bool UsernameExists(string username);
        AccountModel AccountLoginCredentials(string username, string password);
        bool IdExists(string id);

        List<TransactionModel> GetTransactionHistory(string accountId);

        float GetBalance(AccountModel account);

        AccountModel GetAccount(string accountId);

    }
}
