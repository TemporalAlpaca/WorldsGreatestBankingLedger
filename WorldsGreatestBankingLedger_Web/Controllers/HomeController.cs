using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorldsGreatestBankingLedger_Web.Models;
using WorldsGreatestBankingLedger_Web.Repositories;

namespace WorldsGreatestBankingLedger_Web.Controllers
{
    public class HomeController : Controller
    {
        private static AccountModel currentAccount;

        private IBankingRepository bankingRepository;

        public HomeController(IBankingRepository bankingRepo)
        {
            bankingRepository = bankingRepo;
        }

        public IActionResult Index(AccountModel account)
        {
            if(account != null && account.Username != null)
                currentAccount = account;

            return View(currentAccount);
        }

        public IActionResult Transaction()
        {
            if(currentAccount == null || currentAccount.Username == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Name = currentAccount.Name;
            TransactionModel transaction = new TransactionModel();

            return View(transaction);
        }

        [HttpPost]
        public IActionResult Transaction(TransactionModel transaction)
        {
            if (transaction != null && currentAccount != null && currentAccount.Id != null
                && transaction.Amount != 0)
            {
                transaction.AccountId = currentAccount.Id;
                bankingRepository.InsertTransaction(transaction);
                return RedirectToAction("History");
            }

            ViewBag.TransactionFailed = "Transaction Failed. Value cannot be zero.";
            return View(transaction);
        }

        public IActionResult History()
        {
            if (currentAccount == null || currentAccount.Username == null)
                return RedirectToAction("Index", "Login");

            List<TransactionModel> transactions = bankingRepository.GetTransactionHistory(currentAccount.Id);
            ViewBag.Name = currentAccount.Name;

            return View(transactions);
        }

        public IActionResult Balance()
        {
            if (currentAccount == null || currentAccount.Username == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Name = currentAccount.Name;
            return View(bankingRepository.GetBalance(currentAccount));
        }

        public IActionResult LogOut()
        {
            currentAccount = null;
            return RedirectToAction("Index", "Login");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
