using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RVWBank1.Models;

namespace RVWBank1.Controllers
{
    public class HomeController : Controller
    {
        private BankDatabase _context;

        public HomeController(BankDatabase context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


    }
}