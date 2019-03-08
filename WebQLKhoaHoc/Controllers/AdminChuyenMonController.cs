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
    public class AdminChuyenMonController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminChuyenMon
        public async Task<ActionResult> Index()
        {
            return View(await db.ChuyenMons.ToListAsync());
        }

        // GET: AdminChuyenMon/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMon chuyenMon = await db.ChuyenMons.FindAsync(id);
            if (chuyenMon == null)
            {
                return HttpNotFound();
            }
            return View(chuyenMon);
        }

        // GET: AdminChuyenMon/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminChuyenMon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaChuyenMon,TenChuyenMon")] ChuyenMon chuyenMon)
        {
            if (ModelState.IsValid)
            {
                db.ChuyenMons.Add(chuyenMon);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(chuyenMon);
        }

        // GET: AdminChuyenMon/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMon chuyenMon = await db.ChuyenMons.FindAsync(id);
            if (chuyenMon == null)
            {
                return HttpNotFound();
            }
            return View(chuyenMon);
        }

        // POST: AdminChuyenMon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaChuyenMon,TenChuyenMon")] ChuyenMon chuyenMon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chuyenMon).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(chuyenMon);
        }

        // GET: AdminChuyenMon/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMon chuyenMon = await db.ChuyenMons.FindAsync(id);
            if (chuyenMon == null)
            {
                return HttpNotFound();
            }
            return View(chuyenMon);
        }

        // POST: AdminChuyenMon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ChuyenMon chuyenMon = await db.ChuyenMons.FindAsync(id);
            db.ChuyenMons.Remove(chuyenMon);
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
