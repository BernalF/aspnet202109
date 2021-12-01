using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Northwind.Store.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Store.Model;
using Northwind.Store.UI.Web.Internet.Models;
using Northwind.Store.UI.Web.Internet.Settings;
using X.PagedList;

namespace Northwind.Store.UI.Web.Internet.Controllers
{
    [ResponseCache(CacheProfileName = "Basic")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SessionSettings _ss;
        private readonly NWContext _context;

        public HomeController(ILogger<HomeController> logger, SessionSettings ss, NWContext db)
        {
            _logger = logger;
            _ss = ss;
            _context = db;
        }

        public async Task<IActionResult> Index(int? page, string filter)
        {
            var pageNumber = page ?? 1;
            ViewData["filter"] = filter;
            
            var items = await GetDataFiltered(pageNumber, filter);

            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_HomePartial", items);
            }
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string filter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = filter;
            }

            ViewData["filter"] = searchString;

            var pageNumber = page ?? 1;
            var items = await GetDataFiltered(pageNumber, filter);

            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_HomePartial", items);
            }
            
            return View(items);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        private async Task<IPagedList<Product>> GetDataFiltered(int? page, string filter)
        {
            var pageNumber = page ?? 1;

            IPagedList<Product> items;

            if (filter != null)
            {
                items = await _context.Products.Include(p => p.Category).Where(x => x.ProductName.Contains(filter)).ToPagedListAsync(pageNumber, 5);
            }
            else
            {
                items = await _context.Products.Include(p => p.Category).ToPagedListAsync(pageNumber, 5);
            }
            return items;
        }
    }
}
