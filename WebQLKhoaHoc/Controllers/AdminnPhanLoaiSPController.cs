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
    public class AdminnPhanLoaiSPController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminnPhanLoaiSP
        public async Task<ActionResult> Index()
        {
            return View(await db.PhanLoaiSPs.ToListAsync());
        }

        // GET: AdminnPhanLoaiSP/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoaiSP phanLoaiSP = await db.PhanLoaiSPs.FindAsync(id);
            if (phanLoaiSP == null)
            {
                return HttpNotFound();
            }
            return View(phanLoaiSP);
        }

        // GET: AdminnPhanLoaiSP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminnPhanLoaiSP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaPhanLoai,TenPhanLoai")] PhanLoaiSP phanLoaiSP)
        {
            if (ModelState.IsValid)
            {
                db.PhanLoaiSPs.Add(phanLoaiSP);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(phanLoaiSP);
        }

        // GET: AdminnPhanLoaiSP/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoaiSP phanLoaiSP = await db.PhanLoaiSPs.FindAsync(id);
            if (phanLoaiSP == null)
            {
                return HttpNotFound();
            }
            return View(phanLoaiSP);
        }

        // POST: AdminnPhanLoaiSP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaPhanLoai,TenPhanLoai")] PhanLoaiSP phanLoaiSP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phanLoaiSP).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(phanLoaiSP);
        }

        // GET: AdminnPhanLoaiSP/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoaiSP phanLoaiSP = await db.PhanLoaiSPs.FindAsync(id);
            if (phanLoaiSP == null)
            {
                return HttpNotFound();
            }
            return View(phanLoaiSP);
        }

        // POST: AdminnPhanLoaiSP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PhanLoaiSP phanLoaiSP = await db.PhanLoaiSPs.FindAsync(id);
            db.PhanLoaiSPs.Remove(phanLoaiSP);
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
