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
    public class AdminLoaiHinhDeTaiController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminLoaiHinhDeTai
        public async Task<ActionResult> Index()
        {
            return View(await db.LoaiHinhDeTais.ToListAsync());
        }

        // GET: AdminLoaiHinhDeTai/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiHinhDeTai loaiHinhDeTai = await db.LoaiHinhDeTais.FindAsync(id);
            if (loaiHinhDeTai == null)
            {
                return HttpNotFound();
            }
            return View(loaiHinhDeTai);
        }

        // GET: AdminLoaiHinhDeTai/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminLoaiHinhDeTai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaLoaiDT,TenLoaiDT")] LoaiHinhDeTai loaiHinhDeTai)
        {
            if (ModelState.IsValid)
            {
                db.LoaiHinhDeTais.Add(loaiHinhDeTai);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(loaiHinhDeTai);
        }

        // GET: AdminLoaiHinhDeTai/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiHinhDeTai loaiHinhDeTai = await db.LoaiHinhDeTais.FindAsync(id);
            if (loaiHinhDeTai == null)
            {
                return HttpNotFound();
            }
            return View(loaiHinhDeTai);
        }

        // POST: AdminLoaiHinhDeTai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaLoaiDT,TenLoaiDT")] LoaiHinhDeTai loaiHinhDeTai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiHinhDeTai).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(loaiHinhDeTai);
        }

        // GET: AdminLoaiHinhDeTai/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiHinhDeTai loaiHinhDeTai = await db.LoaiHinhDeTais.FindAsync(id);
            if (loaiHinhDeTai == null)
            {
                return HttpNotFound();
            }
            return View(loaiHinhDeTai);
        }

        // POST: AdminLoaiHinhDeTai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LoaiHinhDeTai loaiHinhDeTai = await db.LoaiHinhDeTais.FindAsync(id);
            db.LoaiHinhDeTais.Remove(loaiHinhDeTai);
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
