using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVWBank1.Models;

namespace RVWBank1.Controllers
{
    public class AdminController : Controller
    {

        private BankDatabase _context;

        public AdminController(BankDatabase context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Accounts.Include(u => u.User).ToList().OrderBy(a => a.User.FirstName));
        }

        public async Task<IActionResult> Details(int id)
        {

            var account = await _context
                .Accounts
                .Include(u => u.User)
                .Include(t => t.Transactions)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
                return NotFound();

            return View(account);


        }


        public async Task<IActionResult> Edit(int id)
        {
            var account = await _context
                .Accounts
                .Include(u => u.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            return View("EditForm", account);
        }

        [HttpPost]
        public async Task<IActionResult> EditAcc(Account account, string firstname, string lastname)
        {
            //todo: validation
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return Content("form is not okay");
            }


            var accountInDb = await _context.Accounts.Include(u => u.User).FirstOrDefaultAsync(a => a.Id == account.Id);

            accountInDb.User.FirstName = firstname;
            accountInDb.User.LastName = lastname;
            accountInDb.Checkings = account.Checkings;
            accountInDb.Savings = account.Savings;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var account = await _context
                .Accounts
                .Include(t => t.Transactions)
                .FirstOrDefaultAsync(s => s.Id == id);

            var user = await _context
                .Users
                .FirstOrDefaultAsync(u => u.Id == id);


            if (account == null)
            {
                return NotFound();
            }
            //todo: remove student enrollments first



            if (account.Transactions.Count > 0)
            {
                foreach (var transaction in account.Transactions)
                {
                    transaction.Username = null;
                    transaction.AccountType = null;
                    transaction.Amount = 0F;
                    transaction.Method = null;
                }
            }

            _context.Accounts.Remove(account);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }




    }
}