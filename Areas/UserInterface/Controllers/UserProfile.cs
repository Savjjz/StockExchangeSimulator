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

namespace StockExchangeSimulator.Areas.UserInterface.Controllers
{
    [Area("UserInterface")]
    public class UserProfile : Controller
    {
        private EFDBContext _dataContext;
        private User _currentUser;
        private Wallet _currentWallet;

        public UserProfile(EFDBContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Route("Account/[area]")]
        public IActionResult ShowUserProfile()
        {
            var users = _dataContext.Users.ToList();
            User currentUser = users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            _currentUser = currentUser;

            var wallets = _dataContext.Wallets.ToList();
            Wallet currentWallet = wallets.FirstOrDefault(p => p.UserId == currentUser.Id);
            _currentWallet = currentWallet;

            UserInfoViewModel model = new UserInfoViewModel()
            {
                Id = _currentUser.Id,
                UserName = _currentUser.UserName,
                TotalMoneySum = _currentWallet.TotalSum
            };

            return View(model);
        }

        [Route("[Controller]/[Action]")]
        public IActionResult TopUpBalance(UserInfoViewModel model)
        {
            var users = _dataContext.Users.ToList();
            User currentUser = users.FirstOrDefault(p => p.UserName == User.Identity.Name);

            var wallets = _dataContext.Wallets.ToList();
            Wallet currentWallet = wallets.FirstOrDefault(p => p.UserId == currentUser.Id);

            currentWallet.TotalSum += model.AddedSum;
            _dataContext.SaveChanges();

            return View();
        }
    }
}
