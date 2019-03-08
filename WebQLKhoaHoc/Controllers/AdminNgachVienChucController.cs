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
    public class AdminNgachVienChucController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminNgachVienChuc
        public async Task<ActionResult> Index()
        {
            return View(await db.NgachVienChucs.ToListAsync());
        }

        // GET: AdminNgachVienChuc/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NgachVienChuc ngachVienChuc = await db.NgachVienChucs.FindAsync(id);
            if (ngachVienChuc == null)
            {
                return HttpNotFound();
            }
            return View(ngachVienChuc);
        }

        // GET: AdminNgachVienChuc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminNgachVienChuc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaNgach,TenNgach")] NgachVienChuc ngachVienChuc)
        {
            if (ModelState.IsValid)
            {
                db.NgachVienChucs.Add(ngachVienChuc);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ngachVienChuc);
        }

        // GET: AdminNgachVienChuc/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NgachVienChuc ngachVienChuc = await db.NgachVienChucs.FindAsync(id);
            if (ngachVienChuc == null)
            {
                return HttpNotFound();
            }
            return View(ngachVienChuc);
        }

        // POST: AdminNgachVienChuc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaNgach,TenNgach")] NgachVienChuc ngachVienChuc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ngachVienChuc).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ngachVienChuc);
        }

        // GET: AdminNgachVienChuc/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NgachVienChuc ngachVienChuc = await db.NgachVienChucs.FindAsync(id);
            if (ngachVienChuc == null)
            {
                return HttpNotFound();
            }
            return View(ngachVienChuc);
        }

        // POST: AdminNgachVienChuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NgachVienChuc ngachVienChuc = await db.NgachVienChucs.FindAsync(id);
            db.NgachVienChucs.Remove(ngachVienChuc);
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
