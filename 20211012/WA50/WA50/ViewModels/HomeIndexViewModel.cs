using Northwind.Store.Model;
using System.Collections.Generic;

namespace WA50.ViewModels
{
    public class HomeIndexViewModel
    {
        public string Filter { get; set; }
        public List<Product> Products { get; set; }
    }
}