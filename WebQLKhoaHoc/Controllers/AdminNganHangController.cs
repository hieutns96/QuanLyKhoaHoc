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
    public class AdminNganHangController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: NganHang
        public async Task<ActionResult> Index()
        {
            return View(await db.NganHangs.ToListAsync());
        }

        // GET: NganHang/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganHang nganHang = await db.NganHangs.FindAsync(id);
            if (nganHang == null)
            {
                return HttpNotFound();
            }
            return View(nganHang);
        }

        // GET: NganHang/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NganHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaNH,TenNH,TenTiengAnh,TenVietTat,Website")] NganHang nganHang)
        {
            if (ModelState.IsValid)
            {
                db.NganHangs.Add(nganHang);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(nganHang);
        }

        // GET: NganHang/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganHang nganHang = await db.NganHangs.FindAsync(id);
            if (nganHang == null)
            {
                return HttpNotFound();
            }
            return View(nganHang);
        }

        // POST: NganHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaNH,TenNH,TenTiengAnh,TenVietTat,Website")] NganHang nganHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nganHang).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nganHang);
        }

        // GET: NganHang/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganHang nganHang = await db.NganHangs.FindAsync(id);
            if (nganHang == null)
            {
                return HttpNotFound();
            }
            return View(nganHang);
        }

        // POST: NganHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NganHang nganHang = await db.NganHangs.FindAsync(id);
            db.NganHangs.Remove(nganHang);
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
