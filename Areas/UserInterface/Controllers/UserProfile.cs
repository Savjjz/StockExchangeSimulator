using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeSimulator.Areas.UserInterface.Controllers
{
    [Area("UserInterface")]
    public class UserProfile : Controller 
    {
        [Route("[area]")]
        public IActionResult ShowUserProfile()
        {
            return View();
        }
    }
}
