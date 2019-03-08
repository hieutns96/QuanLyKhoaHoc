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
    public class AdminHocViController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminHocVi
        public async Task<ActionResult> Index()
        {
            return View(await db.HocVis.ToListAsync());
        }

        // GET: AdminHocVi/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocVi hocVi = await db.HocVis.FindAsync(id);
            if (hocVi == null)
            {
                return HttpNotFound();
            }
            return View(hocVi);
        }

        // GET: AdminHocVi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminHocVi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaHocVi,TenVietTat,TenHocVi")] HocVi hocVi)
        {
            if (ModelState.IsValid)
            {
                db.HocVis.Add(hocVi);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(hocVi);
        }

        // GET: AdminHocVi/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocVi hocVi = await db.HocVis.FindAsync(id);
            if (hocVi == null)
            {
                return HttpNotFound();
            }
            return View(hocVi);
        }

        // POST: AdminHocVi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaHocVi,TenVietTat,TenHocVi")] HocVi hocVi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hocVi).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hocVi);
        }

        // GET: AdminHocVi/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocVi hocVi = await db.HocVis.FindAsync(id);
            if (hocVi == null)
            {
                return HttpNotFound();
            }
            return View(hocVi);
        }

        // POST: AdminHocVi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HocVi hocVi = await db.HocVis.FindAsync(id);
            db.HocVis.Remove(hocVi);
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
