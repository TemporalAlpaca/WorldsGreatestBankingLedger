using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldsGreatestBankingLedger_Web.Models;
using WorldsGreatestBankingLedger_Web.Repositories;

namespace WorldsGreatestBankingLedger_Console
{
    class WorldsGreatestBankLedger
    {
        private const string WITHDRAWL = "Withdrawl";
        private const string DEPOSIT = "Deposit";

        //Set banking repository instance to JSONBankingRepository()
        private IBankingRepository bankingRepository = new JSONBankingRepository();
        private AccountModel currentAccount = null;
        public void Run()
        {
            DisplayWelcomeMessage();
            LoginMenu();
            DisplayExitMessage();
        }

        //This function will display a message to the user when the application is started.
        private void DisplayWelcomeMessage()
        {
            Console.WriteLine("Welcome to The World's Greatest Bank!");
            Console.WriteLine("We are proud to be on *Totally Real Bank Digest's Top 10 banks of " + DateTime.Now.Year.ToString() + "!");
            Console.WriteLine("We will give you *FREE *MONEY!");
            Console.WriteLine("Press any key get started.");
            Console.WriteLine("\n\n\n\n\n\n\n*some fees apply, money may not be real money\n");
            Console.WriteLine("--A C# console app sample by Caleb Kauffman--");
            Console.ReadKey();
            Console.Clear();
        }

        //This function will display a message to the user when the application is closed.
        private void DisplayExitMessage()
        {
            Console.Clear();
            Console.WriteLine("Thank you for using The World's Greatest Bank!");
            Console.WriteLine("We hope to see you again soon.\n");
            Console.WriteLine("--A C# console app sample by Caleb Kauffman--");
            Console.ReadKey();
        }

        //This function displays the initial menu to the user.
        private void LoginMenu()
        {
            bool exitMenu = false;
            string menuChoice = "";
            //Process user input
            while (!exitMenu)
            {
                Console.WriteLine("Please select a menu option :)");

                Console.WriteLine("1. Create new account");
                Console.WriteLine("2. Log in");
                Console.WriteLine("3. Exit");

                //Read in menu choice
                menuChoice = Console.ReadLine();

                //Use menu choice to decide workflow process
                switch (menuChoice)
                {
                    case "1":
                        ProcessCreateAccount();
                        break;
                    case "2":
                        ProcessLogin();
                        TransactionMenu();
                        break;
                    case "3":
                        exitMenu = true;
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a valid menu choice.\n");
                        Console.ResetColor();
                        break;
                }
            }
        }

        //This function displays the transaction menu once the user has logged in.
        private void TransactionMenu()
        {
            bool exitMenu = false;
            string menuChoice = "";

            //Process user input
            while (!exitMenu)
            {
                Console.WriteLine("1. Record a deposit");
                Console.WriteLine("2. Record a withdrawl");
                Console.WriteLine("3. Check balance");
                Console.WriteLine("4. View transaction history");
                Console.WriteLine("5. Log out");

                //Read in menu choice
                menuChoice = Console.ReadLine();

                //Use menu choice to decide workflow process
                switch (menuChoice)
                {
                    case "1":
                        ProcessTransaction(DEPOSIT);
                        break;
                    case "2":
                        ProcessTransaction(WITHDRAWL);
                        break;
                    case "3":
                        Console.Clear();
                        ProcessCheckBalance();
                        break;
                    case "4":
                        ProcessViewTransactionHistory();
                        break;
                    case "5":
                        Logout();
                        exitMenu = true;
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a valid menu choice.\n");
                        Console.ResetColor();
                        break;
                }
            }
        }

        //This function is called when the user selects "Create new account"
        //This function will prompt the user for account information and create the account
        private void ProcessCreateAccount()
        {
            string name, email, secondPassword;
            string username = "", password = "";
            bool passwordCheck = false;
            bool usernameExists = true;

            //Check if username already exists
            while (usernameExists)
            {
                Console.WriteLine("Please enter your desired username:");
                username = Console.ReadLine();

                //Call repository to check if username is in use.
                usernameExists = bankingRepository.UsernameExists(username);

                if (usernameExists)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Username " + username + " is already in use.");
                    Console.ResetColor();
                }
            }

            Console.WriteLine("Please enter your full name:");
            name = Console.ReadLine();

            Console.WriteLine("Please enter your email address:");
            email = Console.ReadLine();

            //Validate that second password matches the first
            while (!passwordCheck)
            {
                Console.WriteLine("Please enter a password for your account:");
                password = Console.ReadLine();

                Console.WriteLine("Please re-enter password:");
                secondPassword = Console.ReadLine();

                if(password == secondPassword)
                    passwordCheck = true;
                else
                {
                    Console.Clear();
                    Console.WriteLine("Your passwords do not match.");
                }
            }
            Console.WriteLine("Creating account...");
            CreateAccount(username, password, name, email);
            Console.Clear();
            Console.WriteLine("Account created successfully.\n");
        }

        //This function is called when the user selects "Login"
        //This function will prompt user for their username and password and validate them.
        private void ProcessLogin()
        {
            bool validLogin = false;
            string username = "", password = "";

            while(!validLogin)
            {
                Console.WriteLine("Please enter your username:");
                username = Console.ReadLine();

                Console.WriteLine("Please enter your password:");
                password = Console.ReadLine();

                //Check if login information is valid
                validLogin = Login(username, password);

                if(!validLogin)
                {
                    Console.Clear();
                    Console.WriteLine("Login Failed! Please try again.");
                }
            }
            Console.Clear();
            Console.WriteLine("Login successful! Welcome back " + currentAccount.Name + ".\n");
            Console.WriteLine("What would you like to do?");
        }

        //This function handles prompting the user for a deposit or withdrawl
        private void ProcessTransaction(string transactionType)
        {
            bool validEntry = false;
            string entry = "";
            float transactionAmount = 0;

            while(!validEntry)
            {
                Console.Clear();
                Console.WriteLine("Please enter " + transactionType + " amount:");
                entry = Console.ReadLine();

                try
                {
                    transactionAmount = float.Parse(entry);
                    transactionAmount = (float)Math.Round(transactionAmount, 2);
                    validEntry = true;

                    if (transactionType == "Withdrawl" && transactionAmount > 0)
                        transactionAmount *= -1;

                    Console.WriteLine("Carrying out transaction...");
                    CreateTransaction(transactionAmount);
                    Console.Clear();
                    Console.WriteLine("Your transaction was processed successfully.");
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Please enter a non-zero numerical value." +
                        "For example: 40 dollars would be entered as '40' or '40.00'");
                }
            }
        }

        //This function displays the user's current balance to the console.
        private void ProcessCheckBalance()
        {
            float balance = bankingRepository.GetBalance(currentAccount);
            if (balance > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Looking good " + currentAccount.Name + "! Your balance is: $"
                    + bankingRepository.GetBalance(currentAccount));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Oh geez that's not good " + currentAccount.Name + "! Your balance is: $"
                    + bankingRepository.GetBalance(currentAccount) + " :(");
            }
            Console.ResetColor();
        }

        //This function displays the user's transaction history to the console.
        private void ProcessViewTransactionHistory()
        {
            List <TransactionModel> transactionHistory = bankingRepository.GetTransactionHistory(currentAccount.Id);

            Console.Clear();
            Console.WriteLine("Here is your transaction history, " + currentAccount.Name + ".\n");

            Console.WriteLine("**************************************************");
            foreach (TransactionModel transaction in transactionHistory)
            {
                //Set console color to green for deposits, red for withdrawls
                if (transaction.Amount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(DEPOSIT);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(WITHDRAWL);
                }
                Console.WriteLine(" amount: " + transaction.Amount);
                Console.ResetColor();
                Console.WriteLine("Transaction date: " + transaction.TransactionDate);
                Console.WriteLine("**************************************************");
            }
            ProcessCheckBalance();
        }

        //This function handles creating a new account.
        private void CreateAccount(string username, string password, string name, string email)
        {
            AccountModel newAccount = new AccountModel(username, password, name, email);
            bankingRepository.InsertAccount(newAccount);
        }

        //This function handles creating a new transaction.
        private void CreateTransaction(float amount)
        {
            if (currentAccount != null)
            {
                TransactionModel newTransaction = new TransactionModel(currentAccount.Id, amount, DateTime.Now);
                bankingRepository.InsertTransaction(newTransaction);
            }
            else
                Console.WriteLine("Unable to process transaction. Current account could not be identified.");
        }

        //This function handles login authentication
        private bool Login(string username, string password)
        {
            currentAccount = bankingRepository.AccountLoginCredentials(username, password);
            return currentAccount != null;
        }

        //This function sets the current active account to null.
        private void Logout()
        {
            currentAccount = null;
            Console.Clear();
            Console.WriteLine("You have successfully been logged out.\n");
        }
    }
}
