using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Entityes;
using Microsoft.AspNetCore.Mvc;
using StockExchangeSimulator.ViewModels;

namespace StockExchangeSimulator.Areas.StockPage.Controllers
{
    [Area("StockPage")]
    public class StockInfoController : Controller
    {
        EFDBContext dataContext;

        public StockInfoController(EFDBContext context)
        {
            dataContext = context;
        }

        [Route("StockInfoController/[action]/{id?}")]
        public IActionResult ShowStockInfo(int id)
        {
            var stocks = dataContext.Stocks.ToList();
            Stock currentStock = stocks.FirstOrDefault(p => p.Id == id);
            StockInfoViewModel model = new StockInfoViewModel();

            var result = from stock in dataContext.Stocks
                         join company in dataContext.Companies on stock.CompanyId equals company.Id
                         join country in dataContext.Countries on company.CountryId equals country.Id
                         select new
                         { 
                            Id = stock.Id,
                            CurrentPrice = stock.CurrentPrice,
                            YearPorfitability = stock.YearPorfitability,
                            FullName = company.FullName,
                            ShortName = company.ShortName,
                            CountryName = country.Name
                         };

            foreach (var r in result)
            {
                if (r.Id == currentStock.Id)
                {
                    model.StockId = r.Id;
                    model.CurrentPrice = r.CurrentPrice;
                    model.YearProfitability = r.YearPorfitability;
                    model.CompanyFullName = r.FullName;
                    model.CompanyShortName = r.ShortName;
                    model.CountryName = r.CountryName;
                }
            }

            return View(model);
        }
    }
}
