using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.Store.Data;
using Northwind.Store.Model;
using Northwind.Store.UI.Web.Internet.Settings;
using Northwind.Store.UI.Web.Internet.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Store.UI.Web.Internet.Controllers
{
    public class CartController : Controller
    {
        private readonly NWContext _db;
        private readonly SessionSettings sessionSettings;
        private readonly RequestSettings requestSettings;

        public CartController(NWContext db, SessionSettings sesSettings)
        {
            _db = db;
            sessionSettings = sesSettings;
            requestSettings = new RequestSettings(this);
        }

        public IActionResult Index()
        {
            var productId = TempData[nameof(Product.ProductId)];
            //var productName = TempData[nameof(Product.ProductName)];
            //TempData.Keep(nameof(Product.ProductName));
            //var productName = TempData.Peek(nameof(Product.ProductName));

            //var productAdded = requestSettings.ProductAdded;

            ViewBag.productAdded = requestSettings.ProductAdded;
            //TempData.Keep();

            return View(sessionSettings.Cart);
        }

        public ActionResult Add(int? id)
        {
            if (id.HasValue)
            {
                var p = _db.Products.Find(id);

                #region Session
                var cart = sessionSettings.Cart;

                cart.Items.Add(p);

                sessionSettings.Cart = cart;
                #endregion

                #region TempData
                //TempData["ProductId"] = p.ProductId;
                TempData[nameof(Product.ProductId)] = p.ProductId;
                TempData[nameof(Product.ProductName)] = p.ProductName;

                requestSettings.ProductAdded = p;
                #endregion
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Buy(CartViewModel cartViewModels)
        {
            List<Product> items = sessionSettings.Cart.Items;
            Order order = new Order
            {
                CustomerId = "QUEDE", 
                OrderDate = System.DateTime.Now
            };

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            _db.Add(order);
            await _db.SaveChangesAsync();

            // get order create and add items
            var orderCreated = _db.Orders.Where(p => p.CustomerId == "QUEDE").OrderByDescending(p => p.OrderId).First();

            //add products to order created 
            foreach (var item in items)
            {
                var productDetails = new OrderDetail
                {
                    OrderId = orderCreated.OrderId,
                    ProductId = item.ProductId,
                    UnitPrice = (decimal) item.UnitPrice,
                    Quantity = 0,
                    Discount = 0
                };

                _db.Add(productDetails);
                    
            }
            await _db.SaveChangesAsync();
            sessionSettings.Cart = null;

            return RedirectToAction(nameof(Index));
        }
    }
}
