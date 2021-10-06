using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WA4.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        public ActionResult Detail()
        {
            return View();
        }
    }
}
