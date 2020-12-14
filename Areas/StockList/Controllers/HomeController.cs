using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StockExchangeSimulator.Areas.StockList.Controllers
{
    [Area("StockList")]
    public class HomeController : Controller
    {
        [Route("[area]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
