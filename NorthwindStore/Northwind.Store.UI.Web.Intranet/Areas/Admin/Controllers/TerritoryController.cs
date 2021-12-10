using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Store.Data;
using Northwind.Store.Model;
using Northwind.Store.Notification;

namespace Northwind.Store.UI.Web.Intranet.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class TerritoryController : Controller
    {
        private readonly IRepository<Territory> repository;
        private readonly IRepository<Region> regionRepository;
        private readonly Notifications notifications = new();
        

        public TerritoryController( IRepository<Territory> repository, IRepository<Region> regionRepository)
        {
            this.repository = repository;
            this.regionRepository = regionRepository;
        }

        // GET: Admin/Territory
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            return View(await repository.GetList(pageNumber, 5));
        }

        // GET: Admin/Territory/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await repository.Get(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Admin/Territory/Create
        public IActionResult Create()
        {
            ViewData["RegionId"] = new SelectList(regionRepository.GetAll(), "RegionId", "RegionDescription");
            return View();
        }

        // POST: Admin/Territory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TerritoryId,TerritoryDescription,RegionId")] Territory model)
        {
            if (ModelState.IsValid)
            {
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
            ViewData["RegionId"] = new SelectList(regionRepository.GetAll(), "RegionId", "RegionDescription", model.RegionId);
            return View(model);
        }

        // GET: Admin/Territory/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await repository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewData["RegionId"] = new SelectList(regionRepository.GetAll(), "RegionId", "RegionDescription", model.RegionId);
            return View(model);
        }

        // POST: Admin/Territory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TerritoryId,TerritoryDescription,RegionId")] Territory model)
        {
            if (id != model.TerritoryId)
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
            ViewData["RegionId"] = new SelectList(regionRepository.GetAll(), "RegionId", "RegionDescription", model.RegionId);
            return View(model);
        }

        // GET: Admin/Territory/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await repository.Get(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Admin/Territory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id != null)
            {
                var model = await repository.Get(id);
                model.State = Model.ModelState.Deleted;

                await repository.Delete(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
