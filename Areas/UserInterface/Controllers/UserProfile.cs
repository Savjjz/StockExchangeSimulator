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
            //Получаем данные о текущем пользователе из БД
            var users = _dataContext.Users.ToList();
            User currentUser = users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            //Получаем кошелек текущего пользователя из БД
            var wallets = _dataContext.Wallets.ToList();
            Wallet currentWallet = wallets.FirstOrDefault(p => p.UserId == currentUser.Id);

            UserInfoViewModel model = new UserInfoViewModel()
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                TotalMoneySum = currentWallet.TotalSum
            };
            //Список сделок в бд
            var deals = _dataContext.Deals.ToList();
            //Список акций в бд
            var stocks = _dataContext.Stocks.ToList();
            //Список компаний в бд
            var companies = _dataContext.Companies.ToList();
            //Подробности о сделке, промежуточный список
            List<PurchasedStock> purchasedStocks = new List<PurchasedStock>();

            foreach(var deal in deals)
            {
                //Находим сделку, которая принадлежит текущему кошельку и добавляем ее в список
                if (deal.WalletId == currentWallet.Id)
                {
                    Stock stock = stocks.FirstOrDefault(p => p.Id == deal.StockId);
                    Company company = companies.FirstOrDefault(p => p.Id == stock.CompanyId);
                    PurchasedStock purchasedStock = new PurchasedStock()
                    {
                        StockId = stock.Id,
                        CompanyFullName = company.FullName,
                        CompanyShortName = company.ShortName,
                        PurchasedStockNumber = deal.StockNumber,
                        StockGrowth = stock.CurrentPrice - deal.CurrentStockPrice,
                        StockGrowthInPercent = (stock.CurrentPrice - deal.CurrentStockPrice) / stock.CurrentPrice * 100
                    };
                    purchasedStocks.Add(purchasedStock);
                }
            }
            //Находим количество акций в сделках с повторяющейся акцией 
            foreach(var i in purchasedStocks)
            {
                PurchasedStock purchasedStock = new PurchasedStock()
                {
                    StockId = i.StockId,
                    CompanyFullName = i.CompanyFullName,
                    CompanyShortName = i.CompanyShortName,
                    StockGrowth = i.StockGrowth,
                    StockGrowthInPercent = i.StockGrowthInPercent,
                    PurchasedStockNumber = 0
                };
                foreach (var j in purchasedStocks)
                    if (i.CompanyShortName == j.CompanyShortName)
                        purchasedStock.PurchasedStockNumber += j.PurchasedStockNumber;
                model.PurchasedStocks.Add(purchasedStock);
            }
            //Удаляем повторения
            model.PurchasedStocks = model.PurchasedStocks.GroupBy(x => x.CompanyShortName).Select(x => x.First()).ToList();
            return View(model);
        }

        [Route("[Controller]/[Action]")]
        public IActionResult TopUpBalance(UserInfoViewModel model)
        {
            //Находим в бд текущего пользователя
            var users = _dataContext.Users.ToList();
            User currentUser = users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            //Находим в бд кошелек пользователя
            var wallets = _dataContext.Wallets.ToList();
            Wallet currentWallet = wallets.FirstOrDefault(p => p.UserId == currentUser.Id);
            //Добавляем на счет пользователя необходимое колиечество средств
            currentWallet.TotalSum += model.AddedSum;
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
