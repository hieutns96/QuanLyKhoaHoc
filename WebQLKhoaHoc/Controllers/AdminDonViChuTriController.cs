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
    public class AdminDonViChuTriController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminDonViChuTri
        public async Task<ActionResult> Index()
        {
            return View(await db.DonViChuTris.ToListAsync());
        }

        // GET: AdminDonViChuTri/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonViChuTri donViChuTri = await db.DonViChuTris.FindAsync(id);
            if (donViChuTri == null)
            {
                return HttpNotFound();
            }
            return View(donViChuTri);
        }

        // GET: AdminDonViChuTri/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminDonViChuTri/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaDVChuTri,TenDVChuTri,DiaChi")] DonViChuTri donViChuTri)
        {
            if (ModelState.IsValid)
            {
                db.DonViChuTris.Add(donViChuTri);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(donViChuTri);
        }

        // GET: AdminDonViChuTri/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonViChuTri donViChuTri = await db.DonViChuTris.FindAsync(id);
            if (donViChuTri == null)
            {
                return HttpNotFound();
            }
            return View(donViChuTri);
        }

        // POST: AdminDonViChuTri/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaDVChuTri,TenDVChuTri,DiaChi")] DonViChuTri donViChuTri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donViChuTri).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(donViChuTri);
        }

        // GET: AdminDonViChuTri/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonViChuTri donViChuTri = await db.DonViChuTris.FindAsync(id);
            if (donViChuTri == null)
            {
                return HttpNotFound();
            }
            return View(donViChuTri);
        }

        // POST: AdminDonViChuTri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DonViChuTri donViChuTri = await db.DonViChuTris.FindAsync(id);
            db.DonViChuTris.Remove(donViChuTri);
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
