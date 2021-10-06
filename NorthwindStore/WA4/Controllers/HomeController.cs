using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Northwind.Store.Data;
using WA4.Models;
using WA4.Extensions;

namespace WA4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private NWContext db;

        public HomeController(ILogger<HomeController> logger, NWContext db)
        {
            this.logger = logger;
            this.db = db;
        }

        public IActionResult Index(IEnumerable<CategoryProductsViewModel> vm)
        {
            vm = from p in db.Products.Include(p => p.Category).ToList()
                group p by p.Category?.CategoryName ?? "Sin Categoría"
                into categoryProducts
                select new CategoryProductsViewModel
                {
                    CategoryName = categoryProducts.Key, Items = categoryProducts.ToList()
                };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(string filter)
        {
            IEnumerable<CategoryProductsViewModel> vm = 
                from p in db.Products.Include(p => p.Category).ToList()
                where p.ProductName.Contains(filter)
                group p by p.Category?.CategoryName ?? "Sin Categoría"
                into categoryProducts
                select new CategoryProductsViewModel
                {
                    CategoryName = categoryProducts.Key, Items = categoryProducts.ToList()
                };

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
        
        public IActionResult IndexPartial(int? id)
        {
            var isAjax = Request.IsAjaxRequest();

            if (id != null)
            {
                return PartialView("ProductPartial", db.Products.Where(p => p.ProductId == id).SingleOrDefault());
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult IndexViewComponent(int? id)
        {
            var isAjax = Request.IsAjaxRequest();

            if (id != null)
            {
                return ViewComponent("Product", new { id });
            }
            else
            {
                return NotFound();
            }
        }
    }
}
