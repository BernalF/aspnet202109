using Northwind.Store.Model;
using System.Collections.Generic;
using X.PagedList;

namespace WA50.ViewModels
{
    public class HomeIndexViewModel
    {
        public string Filter { get; set; }
        public IPagedList<Product> Products { get; set; }
    }
}