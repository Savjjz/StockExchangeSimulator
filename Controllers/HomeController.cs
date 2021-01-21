using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Entityes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockExchangeSimulator.Models;
using StockExchangeSimulator.ViewModels;

namespace StockExchangeSimulator.Controllers
{
    public class HomeController : Controller
    {
        EFDBContext dataContext;

        public HomeController(EFDBContext context)
        {
            dataContext = context;
        }

        public async Task<IActionResult> Index(int? country, string name, int page = 1, SortState sortOrder = SortState.CountryAsc)
        {
            int pageSize = 5;

            IQueryable<Stock> stocks = dataContext.Stocks.Include(x => x.Company).ThenInclude(y => y.Country);

            if ((country != null) && (country != 0))
            {
                stocks = stocks.Where(p => p.Company.CountryId == country);
            }
            if (!String.IsNullOrEmpty(name))
            {
                stocks = stocks.Where(p => p.Company.FullName.Contains(name));
            }
            
            switch (sortOrder)
            {
                case SortState.CountryDesc:
                    stocks = stocks.OrderByDescending(s => s.Company.Country.Name);
                    break;
                case SortState.PriceAsc:
                    stocks.OrderBy(s => s.CurrentPrice);
                    break;
                case SortState.PriceDesc:
                    stocks.OrderByDescending(s => s.CurrentPrice);
                    break;
                case SortState.ProfitabilityAsc:
                    stocks.OrderBy(s => s.YearPorfitability);
                    break;
                case SortState.ProfitabilityDesc:
                    stocks.OrderByDescending(s => s.YearPorfitability);
                    break;
                default:
                    stocks = stocks.OrderBy(s => s.Company.Country.Name);
                    break;
            }

            var count = await stocks.CountAsync();
            var items = await stocks.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(dataContext.Countries.ToList(), country, name),
                Stocks = items
            };

            return View(viewModel);
        } 
    }
}
