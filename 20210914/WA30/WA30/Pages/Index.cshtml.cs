using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WA30.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Models.Product> Products { get; set; }

        [BindProperty()]
        public string Filter { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string filter)
        {
            using (var db = new Data.NWContext())
            {
                Products = db.Products.Where(
                    p => p.ProductName.Contains(filter ?? "")).ToList();
            }
        }

        public void OnPost()
        {
            using (var db = new Data.NWContext())
            {
                Products = db.Products.Where(
                    p => p.ProductName.Contains(Filter ?? "")).ToList();
            }
        }
    }
}
