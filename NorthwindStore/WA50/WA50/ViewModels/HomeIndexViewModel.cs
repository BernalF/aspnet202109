using System;
using Northwind.Store.Model;
using System.Collections.Generic;
using System.Linq;

namespace WA50.ViewModels
{
    public class HomeIndexViewModel
    {
        public string Filter { get; set; }
        public List<Product> Products { get; set; }

        public int ProductsPerPage { get; set; } = 5;
        public int CurrentPage { get; set; }  
  
        public int PageCount  { get; set; } 
         
        public IEnumerable<Product> PaginatedProducts()  
        {  
            int start = (CurrentPage - 1) * ProductsPerPage;  
            return Products.OrderBy(b=>b.ProductId).Skip(start).Take(ProductsPerPage);  
        }  

    }
}