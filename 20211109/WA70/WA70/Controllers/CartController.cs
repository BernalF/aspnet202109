using Microsoft.AspNetCore.Mvc;

namespace WA70.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
