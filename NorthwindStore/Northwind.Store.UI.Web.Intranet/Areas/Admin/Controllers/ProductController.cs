using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Store.Data;
using Northwind.Store.Model;
using Northwind.Store.Notification;

namespace Northwind.Store.UI.Web.Intranet.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IRepository<Product> repository;
        private readonly IRepository<Category> categoryRepository;
        private readonly IRepository<Supplier> supplierRepository;
        private readonly Notifications notifications = new();

        public ProductController(IRepository<Product> repository, IRepository<Category> categoryRepository, IRepository<Supplier> supplierRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
            this.supplierRepository = supplierRepository;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            return View(await repository.GetList(pageNumber, 5, "Category,Supplier"));
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await repository.Get(x => x.ProductId == id, "Category,Supplier");

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAll(), "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(supplierRepository.GetAll(), "SupplierId", "CompanyName");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,Picture")] Product model, IFormFile picture)
        {
            if (ModelState.IsValid)
            {
                if (picture != null)
                {
                    // using System.IO;
                    await using MemoryStream ms = new();
                    await picture.CopyToAsync(ms);
                    model.Picture = ms.ToArray();
                }

                model.State = Model.ModelState.Added;
                await repository.Save(model, notifications);

                if (notifications.Count > 0)
                {
                    var msg = notifications[0];
                    ModelState.AddModelError("", $"{msg.Title} - {msg.Description}");

                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAll(), "CategoryId", "CategoryName", model.CategoryId);
            ViewData["SupplierId"] = new SelectList(supplierRepository.GetAll(), "SupplierId", "CompanyName", model.SupplierId);
            return View(model);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await repository.Get(x => x.ProductId == id);
            if (model == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAll(), "CategoryId", "CategoryName", model.CategoryId);
            ViewData["SupplierId"] = new SelectList(supplierRepository.GetAll(), "SupplierId", "CompanyName", model.SupplierId);

            return View(model);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued, Picture")] Product model, IFormFile picture)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (picture != null)
                {
                    await using MemoryStream ms = new();
                    await picture.CopyToAsync(ms);
                    model.Picture = ms.ToArray();
                }

                model.State = Model.ModelState.Modified;
                await repository.Save(model, notifications);

                if (notifications.Count > 0)
                {
                    var msg = notifications[0];
                    ModelState.AddModelError("", $"{msg.Title} - {msg.Description}");

                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAll(), "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(supplierRepository.GetAll(), "SupplierId", "CompanyName");
            
            return View(model);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await repository.Get(x => x.ProductId == id, "Category,Supplier");

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id != null)
            {
                var model = await repository.Get(x => x.ProductId == id);
                model.State = Model.ModelState.Deleted;

                await repository.Delete(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<FileStreamResult> ReadImage(int? id)
        {
            FileStreamResult result = null;

            if (id != null)
            {
                var model = await repository.Get(x => x.ProductId == id);

                if (model != null)
                {
                    var stream = new MemoryStream(model.Picture);

                    result = File(stream, "image/png");
                }
            }

            return result;
        }
    }
}
