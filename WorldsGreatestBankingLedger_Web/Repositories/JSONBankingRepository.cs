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
        private string ACCOUNT_JSON_DB = @"..\JsonRepositories\Accounts.json";
        private string TRANSACTION_JSON_DB = @"..\JsonRepositories\Transactions.json";
        private const int ID_LENGTH = 16;

        //Constructor for the JSONBankingRepository class
        public JSONBankingRepository()
        {
            //Set JSON_DB paths for either console or web app
            //This will fail if executables are moved or file names are changed

            //Reset the connection strings if the current application is the console ledger
            //Default values for the connection strings are for the web application
            if (Environment.CurrentDirectory.Contains("Console"))
            {
                ACCOUNT_JSON_DB = @"..\..\..\..\JsonRepositories\Accounts.json";
                TRANSACTION_JSON_DB = @"..\..\..\..\JsonRepositories\Transactions.json";
            }
        }

        //This function checks the account JSON "database" to see if a username is already present
        public bool UsernameExists(string username)
        {
            if (GetAllAccounts().Where(account => account.Username == username).Count() != 0)
                return true;

            return false;
        }

        //This function gets an account by using the account Id string.
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

        //This function gets the current balance for an account
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

        //This function retrieves all transactions for an account
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

        //This function inserts an account into the JSON "database"
        public void InsertAccount(AccountModel account)
        {
            account.Id = GenerateId();
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

        //This function updates an account in the JSON "database"
        public void UpdateAccount(AccountModel account)
        {
            List<AccountModel> accounts = GetAllAccounts();
            //Remove the account and insert the new account
            accounts.Remove(accounts.Where(acc => acc.Id == account.Id).First());
            accounts.Add(account);
            WriteAccountJsonFile(accounts);
        }

        //This function inserts a transaction into the JSON "database"
        public void InsertTransaction(TransactionModel transaction)
        {
            List<TransactionModel> transactions = GetAllTransactions();
            SetBalance(transaction);
            transactions.Add(transaction);
            WriteTransactionJsonFile(transactions);
        }

        //Thif function checks if an ID string is already present in the JSON "database"
        public bool IdExists(string id)
        {
            List<AccountModel> accounts = GetAllAccounts();
            if (accounts.Where(account => account.Id == id).Count() != 0)
                return true;

            return false;
        }

        //This function handles the "log in" for the applications
        //A token is not used in this application so the account itself is returned
        public AccountModel AccountLoginCredentials(string username, string password)
        {
            try
            {
                return GetAllAccounts().Where(account => account.Username == username && account.Password == password).First();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown error while logging in: " + ex.Message);
                return null;
            }
        }

        //This function takes a transaction and updates the balance on the corresponding account
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

        //This functions gets all accounts from the JSON "database"
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
                Console.WriteLine("FAILED TO READ DATA FROM JSON FILE. PLEASE CLOSE THE APPLICATION." + ex.Message);
                Console.ReadKey();
            }

            return null;
        }

        //This functions gets all transactions from the JSON "database"
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
                Console.WriteLine("FAILED TO READ DATA FROM JSON FILE. PLEASE CLOSE THE APPLICATION." + ex.Message);
                Console.ReadKey();
            }

            return null;
        }

        //This function writes transaction to the transactions JSON "database"
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

        //This function writes accounts to the account JSON "database"
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

        //This function creates a unique 16 character ID value for a new user
        private string GenerateId()
        {
            bool idExists = true;
            string newId = "";

            while (idExists)
            {
                Random random = new Random();
                string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*?-_";
                //Use linq statement to select a random character string
                newId = new string(Enumerable.Repeat(characters, ID_LENGTH).Select(str => str[random.Next(str.Length)]).ToArray());

                //Check if id has been generated
                idExists = IdExists(newId);
            }

            return newId;
        }
    }
}
