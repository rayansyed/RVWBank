using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVWBank1.Models;

namespace RVWBank1.Controllers
{
    public class AccountController : Controller
    {
        private BankDatabase _context;

        public AccountController(BankDatabase context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

  

        public async Task<IActionResult> Deposit(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return View("Deposit", account);
        }

        public async Task<IActionResult> Withdraw(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return View("Withdraw", account);
        }



        public async Task<IActionResult> Transactions(int id)
        {
            var account = await _context.Accounts.Include(u => u.User).Include(t => t.Transactions).FirstOrDefaultAsync(s => s.Id == id);
            if (account == null)
                return NotFound();

            return View("Transactions", account);
        }


        public async Task<IActionResult> Logout()
        {

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SaveDeposit(Account account, float amount, string accountType)
        {
            var AccountInDb = await _context.Accounts.Include(u => u.User).Include(t => t.Transactions).FirstOrDefaultAsync(a => a.Id == account.Id);
            var transaction = new Transaction();
            transaction.Amount = amount;
            transaction.Username = AccountInDb.User.Username;
            transaction.Method = "Deposit";
            transaction.AccountType = accountType;
            AccountInDb.Transactions.Add(transaction);

            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return Content("Form is not ok");
            }
      
            if (accountType == "Checkings") 
            {
                AccountInDb.Checkings += amount;

            }

            else if (accountType == "Savings") 
            {

                AccountInDb.Savings += amount;
            }


            await _context.SaveChangesAsync();
            return RedirectToAction("Transactions", account);
        }

        [HttpPost]
        public async Task<IActionResult> SaveWithdraw(Account account, float amount, string accountType)
        {
            var AccountInDb = await _context.Accounts.Include(u => u.User).Include(t => t.Transactions).FirstOrDefaultAsync(a => a.Id == account.Id);
            var transaction = new Transaction();
            transaction.Amount = amount;
            transaction.Username = AccountInDb.User.Username;
            transaction.Method = "Withdraw";
            transaction.AccountType = accountType;
            AccountInDb.Transactions.Add(transaction);

            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return Content("Form is not ok");
            }

            if (accountType == "Checkings")
            {
                AccountInDb.Checkings -= amount;

            }

            else if (accountType == "Savings")
            {

                AccountInDb.Savings -= amount;
            }


            await _context.SaveChangesAsync();
            return RedirectToAction("Transactions", account);
        }


        public async Task<IActionResult> Invest(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return View("Invest", account);
        }

        [HttpPost]
        public async Task<IActionResult> SaveInvest(Account account, int amount, string accountType)
        {
            var AccountInDb = await _context.Accounts.Include(u => u.User).Include(t => t.Transactions).FirstOrDefaultAsync(a => a.Id == account.Id);
            var transaction = new Transaction();
            transaction.Amount = amount;
            transaction.Username = AccountInDb.User.Username;
            transaction.Method = "Invest";
            transaction.AccountType = accountType;
            AccountInDb.Transactions.Add(transaction);

            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return Content("Form is not ok");
            }
            if (accountType == "Checkings")
            {
                AccountInDb.Checkings -= amount;

            }

            else if (accountType == "Savings")
            {

                AccountInDb.Savings -= amount;
            }
            AccountInDb.Invested += amount;
            await _context.SaveChangesAsync();
            return RedirectToAction("Transactions", account);
        }

        public async Task<IActionResult> WithdrawInvest(Account account, int amount, string accountType)
        {
            int RandomNumber()
            {
                Random random = new Random();
                return random.Next(-3, 5);
            }
            var AccountInDb = await _context.Accounts.Include(u => u.User).Include(t => t.Transactions).FirstOrDefaultAsync(a => a.Id == account.Id);
            var transaction = new Transaction();
            transaction.Amount = amount;
            transaction.Username = AccountInDb.User.Username;
            transaction.Method = "Invest Withdrawal";
            transaction.AccountType = accountType;
            AccountInDb.Transactions.Add(transaction);
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return Content("Form is not ok");
            }
            int investmentCalculation = RandomNumber() * 1;
            AccountInDb.Invested += investmentCalculation;
            AccountInDb.Invested -= amount;
            if (accountType == "Checkings")
            {
                AccountInDb.Checkings += amount;

            }

            else if (accountType == "Savings")
            {

                AccountInDb.Savings += amount;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Transactions", account);
        }

        public async Task<IActionResult> Transfer(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return View("Transfer", account);
        }

        [HttpPost]
        public async Task<IActionResult> SaveETransfer(Account account, int amount, string accountType, int transferID)
        {

            if (await _context.Accounts.Include(u => u.User).Include(t => t.Transactions).FirstOrDefaultAsync(a => a.Id == transferID) == null) 
            {
                return Content("User With The ID does not Exist");
            }

            var AccountInDb = await _context.Accounts.Include(u => u.User).Include(t => t.Transactions).FirstOrDefaultAsync(a => a.Id == account.Id);
            var RecipientInDb = await _context.Accounts.Include(u => u.User).Include(t => t.Transactions).FirstOrDefaultAsync(a => a.Id == transferID);
           

            
            var transaction = new Transaction();
            transaction.Amount = amount;
            transaction.Username = AccountInDb.User.Username;
            transaction.Method = $"E-Transfer [Sent to {RecipientInDb.User.Email}]";
            transaction.AccountType = accountType;
            AccountInDb.Transactions.Add(transaction);


            var recipientTransaction = new Transaction();
            recipientTransaction.Amount = amount;
            recipientTransaction.Username = AccountInDb.User.Username;
            recipientTransaction.Method = $"E-Transfer [Received from {AccountInDb.User.Email}]";
            recipientTransaction.AccountType = "Checkings";
            RecipientInDb.Transactions.Add(recipientTransaction);


            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return Content("Form is not ok");
            }


            if (accountType == "Checkings")
            {
                AccountInDb.Checkings -= amount;
                RecipientInDb.Checkings += amount;

            }

            else if (accountType == "Savings")
            {

                AccountInDb.Savings -= amount;
                RecipientInDb.Checkings += amount;
            }


            await _context.SaveChangesAsync();
            return RedirectToAction("Transactions", account);

        }

    }

}