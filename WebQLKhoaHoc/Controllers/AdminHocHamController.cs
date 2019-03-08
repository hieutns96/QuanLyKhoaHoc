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
    public class AdminHocHamController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminHocHam
        public async Task<ActionResult> Index()
        {
            return View(await db.HocHams.ToListAsync());
        }

        // GET: AdminHocHam/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocHam hocHam = await db.HocHams.FindAsync(id);
            if (hocHam == null)
            {
                return HttpNotFound();
            }
            return View(hocHam);
        }

        // GET: AdminHocHam/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminHocHam/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaHocHam,TenVietTat,TenHocHam")] HocHam hocHam)
        {
            if (ModelState.IsValid)
            {
                db.HocHams.Add(hocHam);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(hocHam);
        }

        // GET: AdminHocHam/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocHam hocHam = await db.HocHams.FindAsync(id);
            if (hocHam == null)
            {
                return HttpNotFound();
            }
            return View(hocHam);
        }

        // POST: AdminHocHam/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaHocHam,TenVietTat,TenHocHam")] HocHam hocHam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hocHam).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hocHam);
        }

        // GET: AdminHocHam/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HocHam hocHam = await db.HocHams.FindAsync(id);
            if (hocHam == null)
            {
                return HttpNotFound();
            }
            return View(hocHam);
        }

        // POST: AdminHocHam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HocHam hocHam = await db.HocHams.FindAsync(id);
            db.HocHams.Remove(hocHam);
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
