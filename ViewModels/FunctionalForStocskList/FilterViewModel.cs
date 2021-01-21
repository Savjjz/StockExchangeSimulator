using DataLayer.Entityes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeSimulator.ViewModels
{
    public class FilterViewModel
    {
        public FilterViewModel(List<Country> countries, int? country, string name)
        {
            countries.Insert(0, new Country { Name = "Все", Id = 0 });
            Countries = new SelectList(countries, "Id", "Name", country);
            SelectedCountry = country;
            SelectedName = name;
        }
        public SelectList Countries { get; private set; }
        public int? SelectedCountry { get; private set; }
        public string SelectedName { get; private set; }
    }
}
