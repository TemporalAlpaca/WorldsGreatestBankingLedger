using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorldsGreatestBankingLedger_Web.Models;
using WorldsGreatestBankingLedger_Web.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace WorldsGreatestBankingLedger_Web.Controllers
{
    public class LoginController : Controller
    {
        private IBankingRepository bankingRepository;

        public LoginController(IBankingRepository bankingRepo)
        {
            bankingRepository = bankingRepo;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            AccountModel account = new AccountModel();
            return View(account);
        }

        [HttpPost]
        public IActionResult Index(AccountModel account)
        {
            //Get the account with the username and password given.
            AccountModel loggedInAccount = bankingRepository.AccountLoginCredentials(account.Username, account.Password);

            //Send user to home page if "login" was successful
            if (loggedInAccount != null)
                return RedirectToAction("Index", "Home", loggedInAccount);

            ViewBag.LoginFailed = "Incorrect username or password. Please try again.";
            return View("Index");
        }

        [HttpPost]
        public IActionResult CreateAccount(AccountModel account)
        {
            //Check to see if the user entered values for all fields
            if (account != null && account.Username != null && account.Password != null
                && account.Email != null && account.Name != null)
            {
                //Check if username already exists.
                //Return error message if true
                if (bankingRepository.UsernameExists(account.Username))
                {
                    ViewBag.CreateAccountFailed = "Username already in use. Please try again.";
                    return View("Index");
                }
                //Store password check temporarily in Id field to compare the values
                //Return error message if they do not match
                if (account.Password == account.Id)
                    bankingRepository.InsertAccount(account);
                else
                {
                    ViewBag.CreateAccountFailed = "Your passwords do not match. Please try again.";
                    return View("Index");
                }
            ViewBag.CreateAccountSuccess = "Account Created Successfully!";
            return View("Index");
            }
            //Display error if fields were left empty
            ViewBag.CreateAccountFailed = "You must enter a value for every field.";
            return View("Index");
        }
    }
}
