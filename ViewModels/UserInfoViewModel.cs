using System.Collections.Generic;
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
        //Число акций, которое пользователь хочет продать
        public int StockNumberForSale { get; set; }
    }
}
