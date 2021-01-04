using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeSimulator.ViewModels
{
    public class StockInfoViewModel
    {
        public int StockId {get; set;}
        public double CurrentPrice { get; set; }
        public double YearProfitability { get; set; }
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public string CountryName { get; set; }
    }
}
