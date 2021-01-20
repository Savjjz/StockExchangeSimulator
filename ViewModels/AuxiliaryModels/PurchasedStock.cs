using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeSimulator.ViewModels.AuxiliaryModels
{
    public class PurchasedStock
    {
        public int StockId { get; set; }
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public int PurchasedStockNumber { get; set; }
        public double StockGrowth { get; set; }
        public double StockGrowthInPercent { get; set; }
    }
}
