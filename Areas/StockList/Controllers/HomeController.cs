using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Entityes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StockExchangeSimulator.Areas.StockList.Controllers
{
    [Area("StockList")]
    public class HomeController : Controller
    {
        EFDBContext dataContext;

        public HomeController(EFDBContext context)
        {
            dataContext = context;
        }

        [Route("[area]")]
        public IActionResult Index()
        {
            /*var stocks = dataContext.Stocks
                .Include(u => u.Company)
                .ThenInclude(c => c.Country)
                .ToList();

            return View(stocks);*/
            return View();
        }
    }
}
