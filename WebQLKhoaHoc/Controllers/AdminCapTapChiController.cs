using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebQLKhoaHoc;

namespace WebQLKhoaHoc.Controllers
{
    public class AdminCapTapChiController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminCapTapChi
        public async Task<ActionResult> Index()
        {
            return View(await db.CapTapChis.ToListAsync());
        }

        // GET: AdminCapTapChi/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CapTapChi capTapChi = await db.CapTapChis.FindAsync(id);
            if (capTapChi == null)
            {
                return HttpNotFound();
            }
            return View(capTapChi);
        }

        // GET: AdminCapTapChi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCapTapChi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaCapTapChi,TenCapTapChi,ChiChu")] CapTapChi capTapChi)
        {
            if (ModelState.IsValid)
            {
                db.CapTapChis.Add(capTapChi);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(capTapChi);
        }

        // GET: AdminCapTapChi/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CapTapChi capTapChi = await db.CapTapChis.FindAsync(id);
            if (capTapChi == null)
            {
                return HttpNotFound();
            }
            return View(capTapChi);
        }

        // POST: AdminCapTapChi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaCapTapChi,TenCapTapChi,ChiChu")] CapTapChi capTapChi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(capTapChi).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(capTapChi);
        }

        // GET: AdminCapTapChi/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CapTapChi capTapChi = await db.CapTapChis.FindAsync(id);
            if (capTapChi == null)
            {
                return HttpNotFound();
            }
            return View(capTapChi);
        }

        // POST: AdminCapTapChi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CapTapChi capTapChi = await db.CapTapChis.FindAsync(id);
            db.CapTapChis.Remove(capTapChi);
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
