﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Store.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Northwind.Store.Model;
using X.PagedList;

namespace Northwind.Store.UI.Web.Internet.Controllers
{
    public class HomeController : Controller
    {
        private readonly NwContext context;
        private readonly IMemoryCache memoryCache;

        public HomeController(NwContext db, IMemoryCache memoryCache)
        {
            context = db;
            this.memoryCache = memoryCache;
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
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new[] { "filter" })]
        public async Task<IActionResult> Index(string filter, string searchString, int? page)
        {
            filter ??= string.Empty;
            
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

            if (memoryCache.Get(filter) != null)
                ViewData["cached"] = "From Cache";

            if (!memoryCache.TryGetValue(filter, out IPagedList<Product> items))
            {
                items = await GetDataFiltered(pageNumber, filter);
                memoryCache.Set(filter, items, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromSeconds(10) });
                ViewData["cached"] = "From DB";
            }
            
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

            var product = await context.Products
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
                items = await context.Products.Include(p => p.Category).Where(x => x.ProductName.Contains(filter)).ToPagedListAsync(pageNumber, 5);
            }
            else
            {
                items = await context.Products.Include(p => p.Category).ToPagedListAsync(pageNumber, 5);
            }
            return items;
        }
    }
}
