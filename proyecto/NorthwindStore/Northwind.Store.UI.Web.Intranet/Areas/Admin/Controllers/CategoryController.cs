using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Northwind.Store.Data;
using Northwind.Store.Model;
using Northwind.Store.Notification;

namespace Northwind.Store.UI.Web.Intranet.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly Notifications ns = new();
        private readonly NWContext _context;
        private readonly IRepository<Category, int> _cR;
        private readonly CategoryRepository _cR2;

        public CategoryController(NWContext context, IRepository<Category, int> cR, CategoryRepository cR2)
        {
            _context = context;
            _cR = cR;
            _cR2 = cR2;
        }

        // GET: Admin/Category
        public async Task<IActionResult> Index()
        {
            var filtro = "";
            var categories = await _cR.Find(c => c.CategoryName.Contains(filtro));

            //return View(await _context.Categories.ToListAsync());
            return View(await _cR.GetList());
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _context.Categories
            //    .FirstOrDefaultAsync(m => m.CategoryId == id);
            var category = await _cR.Get(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
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
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description,Picture")] Category category, IFormFile picture)
        {
            if (ModelState.IsValid)
            {
                if (picture != null)
                {
                    // using System.IO;
                    using MemoryStream ms = new();
                    picture.CopyTo(ms);
                    category.Picture = ms.ToArray();
                }

                //_context.Add(category);
                //await _context.SaveChangesAsync();

                category.State = Model.ModelState.Added;
                await _cR.Save(category, ns);

                if (ns.Count > 0)
                {
                    var msg = ns[0];
                    ModelState.AddModelError("", $"{msg.Title} - {msg.Description}");

                    return View(category);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _context.Categories.FindAsync(id);
            var category = await _cR.Get(id.Value);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description,Picture,RowVersion,ModifiedProperties")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //try
                //{
                //    _context.Update(category);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!CategoryExists(category.CategoryId))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}

                category.State = Model.ModelState.Modified;
                //category.ModifiedProperties.Add("CategoryName");
                await _cR.Save(category, ns);

                if (ns.Count > 0)
                {
                    var msg = ns[0];
                    ModelState.AddModelError("", $"{msg.Title} - {msg.Description}");

                    return View(category);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _context.Categories
            //    .FirstOrDefaultAsync(m => m.CategoryId == id);
            var category = await _cR.Get(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var category = await _context.Categories.FindAsync(id);
            //_context.Categories.Remove(category);
            //await _context.SaveChangesAsync();

            //var category = await _cR.Get(id);
            //category.State = Model.ModelState.Deleted;
            //await _cR.Save(category);

            await _cR2.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }

        public async Task<FileStreamResult> ReadImage(int id)
        {
            //FileStreamResult result = null;

            //var category = await _context.Categories
            //    .FirstOrDefaultAsync(m => m.CategoryId == id);

            //if (category != null)
            //{
            //    var stream = new MemoryStream(category.Picture);

            //    if (stream != null)
            //    {
            //        result = File(stream, "image/png");
            //    }
            //}

            //return result;

            return File(await _cR2.GetFileStream(id), "image/png");
        }
    }
}
