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
    public class AdminCapDeTaiController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminCapDeTai
        public async Task<ActionResult> Index()
        {
            return View(await db.CapDeTais.ToListAsync());
        }

        // GET: AdminCapDeTai/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CapDeTai capDeTai = await db.CapDeTais.FindAsync(id);
            if (capDeTai == null)
            {
                return HttpNotFound();
            }
            return View(capDeTai);
        }

        // GET: AdminCapDeTai/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCapDeTai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaCapDeTai,TenCapDeTai")] CapDeTai capDeTai)
        {
            if (ModelState.IsValid)
            {
                db.CapDeTais.Add(capDeTai);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(capDeTai);
        }

        // GET: AdminCapDeTai/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CapDeTai capDeTai = await db.CapDeTais.FindAsync(id);
            if (capDeTai == null)
            {
                return HttpNotFound();
            }
            return View(capDeTai);
        }

        // POST: AdminCapDeTai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaCapDeTai,TenCapDeTai")] CapDeTai capDeTai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(capDeTai).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(capDeTai);
        }

        // GET: AdminCapDeTai/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CapDeTai capDeTai = await db.CapDeTais.FindAsync(id);
            if (capDeTai == null)
            {
                return HttpNotFound();
            }
            return View(capDeTai);
        }

        // POST: AdminCapDeTai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CapDeTai capDeTai = await db.CapDeTais.FindAsync(id);
            db.CapDeTais.Remove(capDeTai);
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
