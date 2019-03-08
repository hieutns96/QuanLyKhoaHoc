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
    public class AdminPhanLoaiSachesController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminPhanLoaiSaches
        public async Task<ActionResult> Index()
        {
            return View(await db.PhanLoaiSaches.ToListAsync());
        }

        // GET: AdminPhanLoaiSaches/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoaiSach phanLoaiSach = await db.PhanLoaiSaches.FindAsync(id);
            if (phanLoaiSach == null)
            {
                return HttpNotFound();
            }
            return View(phanLoaiSach);
        }

        // GET: AdminPhanLoaiSaches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPhanLoaiSaches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaLoai,TenLoai")] PhanLoaiSach phanLoaiSach)
        {
            if (ModelState.IsValid)
            {
                db.PhanLoaiSaches.Add(phanLoaiSach);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(phanLoaiSach);
        }

        // GET: AdminPhanLoaiSaches/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoaiSach phanLoaiSach = await db.PhanLoaiSaches.FindAsync(id);
            if (phanLoaiSach == null)
            {
                return HttpNotFound();
            }
            return View(phanLoaiSach);
        }

        // POST: AdminPhanLoaiSaches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaLoai,TenLoai")] PhanLoaiSach phanLoaiSach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phanLoaiSach).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(phanLoaiSach);
        }

        // GET: AdminPhanLoaiSaches/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoaiSach phanLoaiSach = await db.PhanLoaiSaches.FindAsync(id);
            if (phanLoaiSach == null)
            {
                return HttpNotFound();
            }
            return View(phanLoaiSach);
        }

        // POST: AdminPhanLoaiSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PhanLoaiSach phanLoaiSach = await db.PhanLoaiSaches.FindAsync(id);
            db.PhanLoaiSaches.Remove(phanLoaiSach);
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
