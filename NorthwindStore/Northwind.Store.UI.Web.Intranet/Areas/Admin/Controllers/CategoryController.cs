using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Store.Data;
using Northwind.Store.Model;
using Northwind.Store.Notification;

namespace Northwind.Store.UI.Web.Intranet.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> repository;
        private readonly Notifications notifications = new();

        public CategoryController(IRepository<Category> repository)
        {
            this.repository = repository;
        }

        // GET: Admin/Category
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1; 
            return View(await repository.GetList(pageNumber, 5));
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await repository.Get(x => x.CategoryId == id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description,Picture")] Category model, IFormFile picture)
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
            return View(model);
        }

        // GET: Admin/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await repository.Get(x => x.CategoryId == id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description,Picture")] Category model)
        {
            if (id != model.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            return View(model);
        }

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await repository.Get(x => x.CategoryId == id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id != null)
            {
                var model = await repository.Get(x => x.CategoryId == id);
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
                var model = await repository.Get(x => x.CategoryId == id);

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
