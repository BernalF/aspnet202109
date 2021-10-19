using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Northwind.Store.Model;
using WA50.Models;
using WA50.ViewModels;

namespace WA50.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// MVVM = ModelViewViewModel
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(HomeIndexViewModel vm, int page=1)
        {
            using var db = new Northwind.Store.Data.NWContext();
            vm.Products = db.Products.OrderBy(d => d.ProductId).Skip((page - 1) * vm.ProductsPerPage).Take(vm.ProductsPerPage).ToList();
            vm.CurrentPage = page;
            vm.PageCount = Convert.ToInt32(Math.Ceiling(db.Products.Count() / (double)vm.ProductsPerPage));  

            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(HomeIndexViewModel vm, string filter, int page=1){

            using var db = new Northwind.Store.Data.NWContext();

            vm.Products = db.Products.Where(p => p.ProductName.Contains(filter))
                .OrderBy(d => d.ProductId).Skip((page - 1) * vm.ProductsPerPage).Take(vm.ProductsPerPage).ToList();

            vm.PageCount = Convert.ToInt32(Math.Ceiling(db.Products.Count() / (double)vm.ProductsPerPage));  
                
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
