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
    public class AdminNhomLinhVucController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminNhomLinhVuc
        public async Task<ActionResult> Index()
        {
            return View(await db.NhomLinhVucs.ToListAsync());
        }

        // GET: AdminNhomLinhVuc/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomLinhVuc nhomLinhVuc = await db.NhomLinhVucs.FindAsync(id);
            if (nhomLinhVuc == null)
            {
                return HttpNotFound();
            }
            return View(nhomLinhVuc);
        }

        // GET: AdminNhomLinhVuc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminNhomLinhVuc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaNhomLinhVuc,TenNhomLinhVuc")] NhomLinhVuc nhomLinhVuc)
        {
            if (ModelState.IsValid)
            {
                db.NhomLinhVucs.Add(nhomLinhVuc);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(nhomLinhVuc);
        }

        // GET: AdminNhomLinhVuc/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomLinhVuc nhomLinhVuc = await db.NhomLinhVucs.FindAsync(id);
            if (nhomLinhVuc == null)
            {
                return HttpNotFound();
            }
            return View(nhomLinhVuc);
        }

        // POST: AdminNhomLinhVuc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaNhomLinhVuc,TenNhomLinhVuc")] NhomLinhVuc nhomLinhVuc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhomLinhVuc).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nhomLinhVuc);
        }

        // GET: AdminNhomLinhVuc/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhomLinhVuc nhomLinhVuc = await db.NhomLinhVucs.FindAsync(id);
            if (nhomLinhVuc == null)
            {
                return HttpNotFound();
            }
            return View(nhomLinhVuc);
        }

        // POST: AdminNhomLinhVuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NhomLinhVuc nhomLinhVuc = await db.NhomLinhVucs.FindAsync(id);
            db.NhomLinhVucs.Remove(nhomLinhVuc);
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
