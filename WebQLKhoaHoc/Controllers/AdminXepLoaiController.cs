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
    public class AdminXepLoaiController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminXepLoai
        public async Task<ActionResult> Index()
        {
            return View(await db.XepLoais.ToListAsync());
        }

        // GET: AdminXepLoai/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            XepLoai xepLoai = await db.XepLoais.FindAsync(id);
            if (xepLoai == null)
            {
                return HttpNotFound();
            }
            return View(xepLoai);
        }

        // GET: AdminXepLoai/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminXepLoai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaXepLoai,TenXepLoai")] XepLoai xepLoai)
        {
            if (ModelState.IsValid)
            {
                db.XepLoais.Add(xepLoai);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(xepLoai);
        }

        // GET: AdminXepLoai/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            XepLoai xepLoai = await db.XepLoais.FindAsync(id);
            if (xepLoai == null)
            {
                return HttpNotFound();
            }
            return View(xepLoai);
        }

        // POST: AdminXepLoai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaXepLoai,TenXepLoai")] XepLoai xepLoai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(xepLoai).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(xepLoai);
        }

        // GET: AdminXepLoai/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            XepLoai xepLoai = await db.XepLoais.FindAsync(id);
            if (xepLoai == null)
            {
                return HttpNotFound();
            }
            return View(xepLoai);
        }

        // POST: AdminXepLoai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            XepLoai xepLoai = await db.XepLoais.FindAsync(id);
            db.XepLoais.Remove(xepLoai);
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
