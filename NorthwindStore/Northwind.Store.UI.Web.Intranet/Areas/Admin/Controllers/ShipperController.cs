﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Store.Data;
using Northwind.Store.Model;
using Northwind.Store.Notification;

namespace Northwind.Store.UI.Web.Intranet.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ShipperController : Controller
    {
        private readonly IRepository<Shipper> repository;
        private readonly Notifications notifications = new();

        public ShipperController(IRepository<Shipper> repository)
        {
            this.repository = repository;
        }

        // GET: Admin/Shipper
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            return View(await repository.GetList(pageNumber, 5));
        }

        // GET: Admin/Shipper/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Admin/Shipper/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Shipper/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShipperId,CompanyName,Phone")] Shipper model)
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
            return View(model);
        }

        // GET: Admin/Shipper/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Admin/Shipper/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShipperId,CompanyName,Phone")] Shipper model)
        {
            if (id != model.ShipperId)
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

        // GET: Admin/Shipper/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/Shipper/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
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