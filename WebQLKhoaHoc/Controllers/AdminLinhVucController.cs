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
    public class AdminLinhVucController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminLinhVuc
        public async Task<ActionResult> Index()
        {
            var linhVucs = db.LinhVucs.Include(l => l.NhomLinhVuc);
            return View(await linhVucs.ToListAsync());
        }

        // GET: AdminLinhVuc/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinhVuc linhVuc = await db.LinhVucs.FindAsync(id);
            if (linhVuc == null)
            {
                return HttpNotFound();
            }
            return View(linhVuc);
        }

        // GET: AdminLinhVuc/Create
        public ActionResult Create()
        {
            ViewBag.MaNhomLinhVuc = new SelectList(db.NhomLinhVucs, "MaNhomLinhVuc", "TenNhomLinhVuc");
            return View();
        }

        // POST: AdminLinhVuc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaLinhVuc,TenLinhVuc,MaNhomLinhVuc")] LinhVuc linhVuc)
        {
            if (ModelState.IsValid)
            {
                db.LinhVucs.Add(linhVuc);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaNhomLinhVuc = new SelectList(db.NhomLinhVucs, "MaNhomLinhVuc", "TenNhomLinhVuc", linhVuc.MaNhomLinhVuc);
            return View(linhVuc);
        }

        // GET: AdminLinhVuc/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinhVuc linhVuc = await db.LinhVucs.FindAsync(id);
            if (linhVuc == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNhomLinhVuc = new SelectList(db.NhomLinhVucs, "MaNhomLinhVuc", "TenNhomLinhVuc", linhVuc.MaNhomLinhVuc);
            return View(linhVuc);
        }

        // POST: AdminLinhVuc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaLinhVuc,TenLinhVuc,MaNhomLinhVuc")] LinhVuc linhVuc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(linhVuc).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaNhomLinhVuc = new SelectList(db.NhomLinhVucs, "MaNhomLinhVuc", "TenNhomLinhVuc", linhVuc.MaNhomLinhVuc);
            return View(linhVuc);
        }

        // GET: AdminLinhVuc/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinhVuc linhVuc = await db.LinhVucs.FindAsync(id);
            if (linhVuc == null)
            {
                return HttpNotFound();
            }
            return View(linhVuc);
        }

        // POST: AdminLinhVuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LinhVuc linhVuc = await db.LinhVucs.FindAsync(id);
            db.LinhVucs.Remove(linhVuc);
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
