using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Northwind.Store.Data;
using Northwind.Store.Model;

namespace Northwind.Store.UI.Demo1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Customer> Customers { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Customers = new CustomerRepository().Get();
        }
    }
}
