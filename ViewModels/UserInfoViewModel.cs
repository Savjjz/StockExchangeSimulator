using System.Collections.Generic;
using DataLayer.Entityes;
using StockExchangeSimulator.ViewModels.AuxiliaryModels;

namespace StockExchangeSimulator.ViewModels
{
    public class UserInfoViewModel
    {
        //Id текущего пользователя
        public string Id { get; set; }
        //Никнейм пользователя
        public string UserName { get; set; }
        //Сумма денег на пользовательском счету
        public double TotalMoneySum { get; set; }
        //Добавляемая сумма денег
        public double AddedSum { get; set; }
        //Список акций, которые держит пользователь, с дополнительной информацией
        public List<PurchasedStock> PurchasedStocks { get; set; } = new List<PurchasedStock>();
        //Список сделок, совершенных пользователем
        public List<DealDetails> DealsDetails { get; set; } = new List<DealDetails>();
        //Число акций, которое пользователь хочет продать
        public int StockNumberForSale { get; set; }
    }
}
