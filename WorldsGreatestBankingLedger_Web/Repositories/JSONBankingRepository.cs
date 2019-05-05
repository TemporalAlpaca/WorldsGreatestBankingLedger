using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldsGreatestBankingLedger_Web.Models;
using Newtonsoft.Json;
using System.IO;

namespace WorldsGreatestBankingLedger_Web.Repositories
{
    public class JSONBankingRepository : IBankingRepository
    {
        //Including in source control ONLY for accessibility to others
        private const string ACCOUNT_JSON_DB = @"..\..\..\..\WorldsGreatestBankingLedger_Web\JsonRepositories\Accounts.json";
        private const string TRANSACTION_JSON_DB = @"..\..\..\..\WorldsGreatestBankingLedger_Web\JsonRepositories\Transactions.json";

        public bool UsernameExists(string username)
        {
            if (GetAllAccounts().Where(account => account.Username == username).Count() != 0)
                return true;

            return false;
        }

        public AccountModel GetAccount(string accountId)
        {
            try
            {
                return GetAllAccounts().Where(account => account.Id == accountId).First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public float GetBalance(AccountModel account)
        {
            try
            {
                return GetAllAccounts().Where(foundAccount => foundAccount.Id == account.Id).First().Balance;
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("FAILED TO READ DATA FROM JSON FILE. PLEASE CLOSE THE APPLICATION.");
                Console.ReadKey();
                return -1;
            }

        }

        public List<TransactionModel> GetTransactionHistory(string accountId)
        {
            try
            {
                return GetAllTransactions().Where(transaction => transaction.AccountId == accountId).ToList();
            }
            catch (Exception)
            {
                return new List<TransactionModel>();
            }
        }

        public void InsertAccount(AccountModel account)
        {
            List<AccountModel> accounts = GetAllAccounts();
            accounts.Add(account);
            string accountJSON = JsonConvert.SerializeObject(accounts, Formatting.Indented);

            using (StreamWriter file = File.CreateText(ACCOUNT_JSON_DB))
            using (JsonWriter writer = new JsonTextWriter(file))
            {
                //Write data to json file
                writer.WriteRaw(accountJSON);
            }
        }

        public void UpdateAccount(AccountModel account)
        {
            List<AccountModel> accounts = GetAllAccounts();
            //Remove the account and insert the new account
            accounts.Remove(accounts.Where(acc => acc.Id == account.Id).First());
            accounts.Add(account);
            WriteAccountJsonFile(accounts);
        }

        public void InsertTransaction(TransactionModel transaction)
        {
            List<TransactionModel> transactions = GetAllTransactions();
            SetBalance(transaction);
            transactions.Add(transaction);
            WriteTransactionJsonFile(transactions);
        }


        public bool IdExists(string id)
        {
            List<AccountModel> accounts = GetAllAccounts();
            if (accounts.Where(account => account.Id == id).Count() != 0)
                return true;

            return false;
        }

        public AccountModel AccountLoginCredentials(string username, string password)
        {
            try
            {
                return GetAllAccounts().Where(account => account.Username == username && account.Password == password).First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void SetBalance(TransactionModel transaction)
        {
            AccountModel account = GetAccount(transaction.AccountId);

            if (account != null)
            {
                account.Balance += transaction.Amount;
                UpdateAccount(account);
            }
            else
                Console.WriteLine("Unable to set balance on account.");
        }

        private List<AccountModel> GetAllAccounts()
        {
            try
            {
                AccountModel[] accounts = JsonConvert.DeserializeObject<AccountModel[]>(File.ReadAllText(ACCOUNT_JSON_DB));

                if (accounts != null)
                    return accounts.ToList();
                else
                    return new List<AccountModel>();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("FAILED TO READ DATA FROM JSON FILE. PLEASE CLOSE THE APPLICATION.");
                Console.ReadKey();
            }

            return null;
        }

        private List<TransactionModel> GetAllTransactions()
        {
            try
            {
                TransactionModel[] transactions = JsonConvert.DeserializeObject<TransactionModel[]>(File.ReadAllText(TRANSACTION_JSON_DB));

                if (transactions != null)
                    return transactions.ToList();
                else
                    return new List<TransactionModel>();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("FAILED TO READ DATA FROM JSON FILE. PLEASE CLOSE THE APPLICATION.");
                Console.ReadKey();
            }

            return null;
        }

        private void WriteTransactionJsonFile(List<TransactionModel> transactions)
        {
            try
            {
                string transactionJSON = JsonConvert.SerializeObject(transactions, Formatting.Indented);

                using (StreamWriter file = File.CreateText(TRANSACTION_JSON_DB))
                using (JsonWriter writer = new JsonTextWriter(file))
                {
                    //Write data to json file
                    writer.WriteRaw(transactionJSON);
                }
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("FAILED TO WRITE DATA TO JSON FILE. PLEASE CLOSE THE APPLICATION.");
                Console.ReadKey();
            }
        }

        private void WriteAccountJsonFile(List<AccountModel> accounts)
        {
            try
            {
                string accountJSON = JsonConvert.SerializeObject(accounts, Formatting.Indented);

                using (StreamWriter file = File.CreateText(ACCOUNT_JSON_DB))
                using (JsonWriter writer = new JsonTextWriter(file))
                {
                    //Write data to json file
                    writer.WriteRaw(accountJSON);
                }
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("FAILED TO WRITE DATA TO JSON FILE. PLEASE CLOSE THE APPLICATION.");
                Console.ReadKey();
            }
        }
    }
}
