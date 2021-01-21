using StockExchangeSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeSimulator.ViewModels
{
    public class SortViewModel
    {
        public SortState CountrySort { get; set; }
        public SortState PriceSort { get; set; }
        public SortState ProfitabilitySort { get; set; }
        public SortState Current { get; set; }

        public SortViewModel(SortState sortOrder)
        {
            CountrySort = sortOrder == SortState.CountryAsc ? SortState.CountryDesc : SortState.CountryAsc;
            PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            ProfitabilitySort = sortOrder == SortState.ProfitabilityAsc ? SortState.ProfitabilityDesc : SortState.ProfitabilityAsc;
            Current = sortOrder;
        }
    }
}
