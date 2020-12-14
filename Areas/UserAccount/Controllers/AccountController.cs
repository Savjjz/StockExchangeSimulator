using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeSimulator.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    public class AccountController : Controller
    {
        [Route("[area]/[controller]/[action]")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("[area]/[controller]/[action]")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
