using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AgencyAgile.Models;
using AgencyAgile.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AgencyAgile.Web.Controllers
{
    public class TenantController : Controller
    {
        private TenantDbContext db = new TenantDbContext();

        // GET: /Tenant/
        public async Task<ActionResult> Index()
        {
            return View(await db.Tenants.ToListAsync());
        }

        // GET: /Tenant/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = await db.Tenants.FindAsync(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // GET: /Tenant/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Tenant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TenantId,Name,Slug")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                tenant.TenantId = Guid.NewGuid();
                tenant.Created = AuditedAction.Create("system", "System");
                db.Tenants.Add(tenant);
                var uctx = AgencyAgile.DAL.IdentityDbContext.Create(tenant.Slug);
                var i = uctx.Users.Count();
                var actx = AgencyDbContext.Create(tenant.Slug);
                var c = actx.Clients.Count();
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tenant);
        }

        // GET: /Tenant/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = await db.Tenants.FindAsync(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: /Tenant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TenantId,Name,Slug")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tenant);
        }

        // GET: /Tenant/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = await db.Tenants.FindAsync(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: /Tenant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Tenant tenant = await db.Tenants.FindAsync(id);
            db.Tenants.Remove(tenant);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
