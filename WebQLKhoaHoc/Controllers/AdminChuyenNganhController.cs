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
    public class AdminChuyenNganhController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminChuyenNganh
        public async Task<ActionResult> Index()
        {
            var chuyenNganhs = db.ChuyenNganhs.Include(c => c.NganhDaoTao);
            return View(await chuyenNganhs.ToListAsync());
        }

        // GET: AdminChuyenNganh/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenNganh chuyenNganh = await db.ChuyenNganhs.FindAsync(id);
            if (chuyenNganh == null)
            {
                return HttpNotFound();
            }
            return View(chuyenNganh);
        }

        // GET: AdminChuyenNganh/Create
        public ActionResult Create()
        {
            ViewBag.MaNganhDaoTao = new SelectList(db.NganhDaoTaos, "MaNganhDaoTao", "TenNganhDaoTao");
            return View();
        }

        // POST: AdminChuyenNganh/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaChuyenNganh,TenChuyenNganh,MaNganhDaoTao")] ChuyenNganh chuyenNganh)
        {
            if (ModelState.IsValid)
            {
                db.ChuyenNganhs.Add(chuyenNganh);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaNganhDaoTao = new SelectList(db.NganhDaoTaos, "MaNganhDaoTao", "TenNganhDaoTao", chuyenNganh.MaNganhDaoTao);
            return View(chuyenNganh);
        }

        // GET: AdminChuyenNganh/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenNganh chuyenNganh = await db.ChuyenNganhs.FindAsync(id);
            if (chuyenNganh == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNganhDaoTao = new SelectList(db.NganhDaoTaos, "MaNganhDaoTao", "TenNganhDaoTao", chuyenNganh.MaNganhDaoTao);
            return View(chuyenNganh);
        }

        // POST: AdminChuyenNganh/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaChuyenNganh,TenChuyenNganh,MaNganhDaoTao")] ChuyenNganh chuyenNganh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chuyenNganh).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaNganhDaoTao = new SelectList(db.NganhDaoTaos, "MaNganhDaoTao", "TenNganhDaoTao", chuyenNganh.MaNganhDaoTao);
            return View(chuyenNganh);
        }

        // GET: AdminChuyenNganh/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenNganh chuyenNganh = await db.ChuyenNganhs.FindAsync(id);
            if (chuyenNganh == null)
            {
                return HttpNotFound();
            }
            return View(chuyenNganh);
        }

        // POST: AdminChuyenNganh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ChuyenNganh chuyenNganh = await db.ChuyenNganhs.FindAsync(id);
            db.ChuyenNganhs.Remove(chuyenNganh);
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
