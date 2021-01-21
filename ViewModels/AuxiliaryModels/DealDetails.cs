using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeSimulator.ViewModels.AuxiliaryModels
{
    public class DealDetails
    {
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public bool IsBought { get; set; }
        public int StockNumberInDeal { get; set; }
        public DateTime DealDate { get; set; }
    }
}
