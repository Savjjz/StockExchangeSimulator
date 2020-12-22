using Microsoft.AspNetCore.Mvc;
using DataLayer;
using DataLayer.Entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using StockExchangeSimulator.Models;
using Microsoft.EntityFrameworkCore;

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
