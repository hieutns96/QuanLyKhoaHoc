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
    public class AdminTinhTrangDeTaiController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminTinhTrangDeTai
        public async Task<ActionResult> Index()
        {
            return View(await db.TinhTrangDeTais.ToListAsync());
        }

        // GET: AdminTinhTrangDeTai/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TinhTrangDeTai tinhTrangDeTai = await db.TinhTrangDeTais.FindAsync(id);
            if (tinhTrangDeTai == null)
            {
                return HttpNotFound();
            }
            return View(tinhTrangDeTai);
        }

        // GET: AdminTinhTrangDeTai/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminTinhTrangDeTai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaTinhTrang,TenTinhTrang")] TinhTrangDeTai tinhTrangDeTai)
        {
            if (ModelState.IsValid)
            {
                db.TinhTrangDeTais.Add(tinhTrangDeTai);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tinhTrangDeTai);
        }

        // GET: AdminTinhTrangDeTai/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TinhTrangDeTai tinhTrangDeTai = await db.TinhTrangDeTais.FindAsync(id);
            if (tinhTrangDeTai == null)
            {
                return HttpNotFound();
            }
            return View(tinhTrangDeTai);
        }

        // POST: AdminTinhTrangDeTai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaTinhTrang,TenTinhTrang")] TinhTrangDeTai tinhTrangDeTai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tinhTrangDeTai).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tinhTrangDeTai);
        }

        // GET: AdminTinhTrangDeTai/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TinhTrangDeTai tinhTrangDeTai = await db.TinhTrangDeTais.FindAsync(id);
            if (tinhTrangDeTai == null)
            {
                return HttpNotFound();
            }
            return View(tinhTrangDeTai);
        }

        // POST: AdminTinhTrangDeTai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TinhTrangDeTai tinhTrangDeTai = await db.TinhTrangDeTais.FindAsync(id);
            db.TinhTrangDeTais.Remove(tinhTrangDeTai);
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
