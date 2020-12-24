using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockExchangeSimulator.Models;

namespace StockExchangeSimulator.Controllers
{
    public class HomeController : Controller
    {
        EFDBContext dataContext;

        public HomeController(EFDBContext context)
        {
            dataContext = context;
        }

        public IActionResult Index()
        {
            var stocks = dataContext.Stocks
                .Include(u => u.Company)
                .ThenInclude(c => c.Country)
                .ToList();
            return View(stocks);
        }
    }
}
