using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RVWBank1.Models;

namespace RVWBank1.Controllers
{
    public class UserController : Controller
    {

        private BankDatabase _context;

        public UserController(BankDatabase context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                if (username == "Admin" && password == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                User myUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (myUser.Username == username && myUser.Password == password)
                {
                    return RedirectToAction("UserHome", myUser);
                }
                else
                {
                    return View("UserNotFound");
                }
            }
            catch (Exception e)
            {
                return View("UserNotFound");
            }
        }

        public async Task<IActionResult> UserHome(int id)
        {
            var account = await _context.Accounts.Include(u => u.User).Include(t=>t.Transactions).FirstOrDefaultAsync(s => s.Id == id);
            if (account == null)
                return NotFound();

            return View("UserHome", account);
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(User user, String username, String email)
        {
            User myUser = null;
            User myEmail = null;
            Account account = new Account();

            myUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            myEmail = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

  
            if (_context.Users.Any(u => u.Username == username) || _context.Users.Any(u => u.Email == email))
            {

                return Content("Username or Email Already Exists");
            }
            else 
            {
                if (user.Id == 0)
                {
                    _context.Users.Add(user);
                    account.Id = user.Id;
                    account.Checkings = 500.00F;
                    account.Savings = 0.00F;
                    account.Invested = 0.00F;
                    account.User = user;
                    _context.Accounts.Add(account);

                }
            }


            await _context.SaveChangesAsync();
            return View("Index");

        }

    }
}