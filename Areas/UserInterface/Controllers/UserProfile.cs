using DataLayer;
using DataLayer.Entityes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using StockExchangeSimulator.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;
using StockExchangeSimulator.ViewModels.AuxiliaryModels;
using Microsoft.EntityFrameworkCore;

namespace StockExchangeSimulator.Areas.UserInterface.Controllers
{
    [Area("UserInterface")]
    public class UserProfile : Controller
    {
        private EFDBContext _dataContext;

        public UserProfile(EFDBContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Route("Account/[area]")]
        public IActionResult ShowUserProfile()
        {
            var users = _dataContext.Users.Include(x => x.Wallet).ThenInclude(y => y.Deals).ThenInclude(z => z.Stock).ThenInclude(t => t.Company);
            User currentUser = users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            UserInfoViewModel model = new UserInfoViewModel
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                TotalMoneySum = currentUser.Wallet.TotalSum
            };
            List<PurchasedStock> purchasedStocks = new List<PurchasedStock>();
            foreach (var deal in currentUser.Wallet.Deals)
            {
                PurchasedStock purchasedStock = new PurchasedStock
                {
                    StockId = deal.Stock.Id,
                    CompanyFullName = deal.Stock.Company.FullName,
                    CompanyShortName = deal.Stock.Company.ShortName,
                    PurchasedStockNumber = deal.StockNumber,
                    StockGrowth = deal.Stock.CurrentPrice - deal.CurrentStockPrice,
                    StockGrowthInPercent = (deal.Stock.CurrentPrice - deal.CurrentStockPrice) / deal.Stock.CurrentPrice * 100
                };
                purchasedStocks.Add(purchasedStock);
            }
            foreach (var p in purchasedStocks)
            {
                PurchasedStock purchasedStock = new PurchasedStock
                {
                    StockId = p.StockId,
                    CompanyFullName = p.CompanyFullName,
                    CompanyShortName = p.CompanyShortName,
                    StockGrowth = p.StockGrowth,
                    StockGrowthInPercent = p.StockGrowthInPercent,
                    PurchasedStockNumber = 0
                };
                foreach (var t in purchasedStocks)
                    if (p.CompanyShortName == t.CompanyShortName)
                        purchasedStock.PurchasedStockNumber += t.PurchasedStockNumber;
                model.PurchasedStocks.Add(purchasedStock);
            }
            model.PurchasedStocks = model.PurchasedStocks.GroupBy(x => x.CompanyShortName).Select(x => x.First()).ToList();
            foreach (var deal in currentUser.Wallet.Deals)
            {
                DealDetails dealDetails = new DealDetails
                {
                    CompanyName = deal.Stock.Company.FullName,
                    CompanyShortName = deal.Stock.Company.ShortName,
                    IsBought = deal.IsBought,
                    DealDate = deal.DealDate,
                    StockNumberInDeal = deal.StockNumber
                };
                model.DealsDetails.Add(dealDetails);
            }
            return View(model);
        }

        [Route("[Controller]/[Action]")]
        public IActionResult TopUpBalance(UserInfoViewModel model)
        {
            //Находим в бд текущего пользователя
            var users = _dataContext.Users.Include(x => x.Wallet).ToList();
            User currentUser = users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            //Пополняем кошелек
            currentUser.Wallet.TotalSum += model.AddedSum;
            _dataContext.SaveChanges();
            return View();
        }

        [Route("[Controller]/[Action]/{id?}")]
        public IActionResult SellOutStock(int id, UserInfoViewModel model)
        {
            //Находим в бд текущего пользователя
            var users = _dataContext.Users.ToList();
            User currentUser = users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            //Находим в бд кошелек пользователя
            var wallets = _dataContext.Wallets.ToList();
            Wallet currentWallet = wallets.FirstOrDefault(p => p.UserId == currentUser.Id);
            //Находим текущую акцию 
            var stocks = _dataContext.Stocks.ToList();
            Stock stock = stocks.FirstOrDefault(p => p.Id == id);
            //Пополняем кошелек
            currentWallet.TotalSum += stock.CurrentPrice * model.StockNumberForSale;
            //Сохраняем в бд сделку продажи
            Deal deal = new Deal()
            {
                CurrentStockPrice = stock.CurrentPrice,
                DealDate = DateTime.Now,
                Stock = stock,
                Wallet = currentWallet,
                StockNumber = -model.StockNumberForSale,
                IsBought = false
            };
            _dataContext.Deals.Add(deal);
            _dataContext.SaveChanges();
            return View();
        }
    }
}
