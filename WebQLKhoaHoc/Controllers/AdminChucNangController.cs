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
    public class AdminChucNangController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminChucNang
        public async Task<ActionResult> Index()
        {
            return View(await db.ChucNangs.ToListAsync());
        }

        // GET: AdminChucNang/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucNang chucNang = await db.ChucNangs.FindAsync(id);
            if (chucNang == null)
            {
                return HttpNotFound();
            }
            return View(chucNang);
        }

        // GET: AdminChucNang/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminChucNang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaChucNang,TenChucNang,GhiChu")] ChucNang chucNang)
        {
            if (ModelState.IsValid)
            {
                db.ChucNangs.Add(chucNang);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(chucNang);
        }

        // GET: AdminChucNang/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucNang chucNang = await db.ChucNangs.FindAsync(id);
            if (chucNang == null)
            {
                return HttpNotFound();
            }
            return View(chucNang);
        }

        // POST: AdminChucNang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaChucNang,TenChucNang,GhiChu")] ChucNang chucNang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chucNang).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(chucNang);
        }

        // GET: AdminChucNang/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChucNang chucNang = await db.ChucNangs.FindAsync(id);
            if (chucNang == null)
            {
                return HttpNotFound();
            }
            return View(chucNang);
        }

        // POST: AdminChucNang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ChucNang chucNang = await db.ChucNangs.FindAsync(id);
            db.ChucNangs.Remove(chucNang);
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
