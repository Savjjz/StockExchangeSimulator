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
    public class StockInfo : Controller
    {
        EFDBContext _dataContext;
        static Stock _currentStock;

        public StockInfo(EFDBContext context)
        {
            _dataContext = context;
        }

        [Route("StockInfoController/[action]/{id?}")]
        public IActionResult ShowStockInfo(int id)
        {
            var stocks = _dataContext.Stocks.ToList();
            _currentStock = stocks.FirstOrDefault(p => p.Id == id);
            StockInfoViewModel model = new StockInfoViewModel();

            var result = from stock in _dataContext.Stocks
                         join company in _dataContext.Companies on stock.CompanyId equals company.Id
                         join country in _dataContext.Countries on company.CountryId equals country.Id
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
                if (r.Id == _currentStock.Id)
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

        [Route("[Controller]/[Action]")]
        public IActionResult DealSuccess(StockInfoViewModel model)
        {
            var users = _dataContext.Users.ToList();
            User user = users.FirstOrDefault(p => p.UserName == User.Identity.Name);

            var wallets = _dataContext.Wallets.ToList();
            Wallet wallet = wallets.FirstOrDefault(p => p.UserId == user.Id);

            int stockId = _currentStock.Id;
            var stocks = _dataContext.Stocks.ToList();
            Stock stock = stocks.FirstOrDefault(p => p.Id == stockId);

            if (wallet.TotalSum >= model.StockNumber* _currentStock.CurrentPrice)
            {

                Deal deal = new Deal
                {
                    DealDate = DateTime.Now,
                    StockId = _currentStock.Id,
                    Stock = stock,
                    StockNumber = model.StockNumber,
                    WalletId = wallet.Id,
                    Wallet = wallet,
                    CurrentStockPrice = _currentStock.CurrentPrice,
                    IsBought = true
                };
                _dataContext.Deals.Add(deal);
                wallet.TotalSum -= model.StockNumber * _currentStock.CurrentPrice;
                _dataContext.SaveChanges();
            }
            return View();
        }

    }
}
