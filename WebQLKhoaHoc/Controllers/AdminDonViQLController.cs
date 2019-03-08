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
    public class AdminDonViQLController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminDonViQL
        public async Task<ActionResult> Index()
        {
            return View(await db.DonViQLs.ToListAsync());
        }

        // GET: AdminDonViQL/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonViQL donViQL = await db.DonViQLs.FindAsync(id);
            if (donViQL == null)
            {
                return HttpNotFound();
            }
            return View(donViQL);
        }

        // GET: AdminDonViQL/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminDonViQL/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaDonVi,TenDonVI,DiaChiCQ,DienThoaiCQ,EmailCQ")] DonViQL donViQL)
        {
            if (ModelState.IsValid)
            {
                db.DonViQLs.Add(donViQL);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(donViQL);
        }

        // GET: AdminDonViQL/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonViQL donViQL = await db.DonViQLs.FindAsync(id);
            if (donViQL == null)
            {
                return HttpNotFound();
            }
            return View(donViQL);
        }

        // POST: AdminDonViQL/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaDonVi,TenDonVI,DiaChiCQ,DienThoaiCQ,EmailCQ")] DonViQL donViQL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donViQL).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(donViQL);
        }

        // GET: AdminDonViQL/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonViQL donViQL = await db.DonViQLs.FindAsync(id);
            if (donViQL == null)
            {
                return HttpNotFound();
            }
            return View(donViQL);
        }

        // POST: AdminDonViQL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DonViQL donViQL = await db.DonViQLs.FindAsync(id);
            db.DonViQLs.Remove(donViQL);
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
