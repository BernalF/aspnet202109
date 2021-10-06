using System.Collections.Generic;
using Northwind.Store.Model;

namespace WA4.Models
{
    public class CategoryProductsViewModel
    {
        public string CategoryName { get; set; }
        public List<Product> Items { get; set; }

        public string Filter { get; set; }

        public CategoryProductsViewModel()
        {
            Items = new List<Product>();
        }
    }
}
