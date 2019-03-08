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
    public class AdminKinhPhiDeTaiController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminKinhPhiDeTai
        public async Task<ActionResult> Index()
        {
            var kinhPhiDeTais = db.KinhPhiDeTais.Include(k => k.DeTai);
            return View(await kinhPhiDeTais.ToListAsync());
        }

        // GET: AdminKinhPhiDeTai/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KinhPhiDeTai kinhPhiDeTai = await db.KinhPhiDeTais.FindAsync(id);
            if (kinhPhiDeTai == null)
            {
                return HttpNotFound();
            }
            return View(kinhPhiDeTai);
        }

        // GET: AdminKinhPhiDeTai/Create
        public ActionResult Create()
        {
            ViewBag.MaDeTai = new SelectList(db.DeTais, "MaDeTai", "MaDeTaiHoSo");           
            return View();
        }

        // POST: AdminKinhPhiDeTai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaPhi,MaDeTai,LoaiKinhPhi,NamTiepNhan,SoTien,LoaiTien")] KinhPhiDeTai kinhPhiDeTai)
        {
            if (ModelState.IsValid)
            {
                db.KinhPhiDeTais.Add(kinhPhiDeTai);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaDeTai = new SelectList(db.DeTais, "MaDeTai", "MaDeTaiHoSo", kinhPhiDeTai.MaDeTai);
            return View(kinhPhiDeTai);
        }

        // GET: AdminKinhPhiDeTai/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KinhPhiDeTai kinhPhiDeTai = await db.KinhPhiDeTais.FindAsync(id);
            if (kinhPhiDeTai == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDeTai = new SelectList(db.DeTais, "MaDeTai", "MaDeTaiHoSo", kinhPhiDeTai.MaDeTai);
            return View(kinhPhiDeTai);
        }

        // POST: AdminKinhPhiDeTai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaPhi,MaDeTai,LoaiKinhPhi,NamTiepNhan,SoTien,LoaiTien")] KinhPhiDeTai kinhPhiDeTai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kinhPhiDeTai).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaDeTai = new SelectList(db.DeTais, "MaDeTai", "MaDeTaiHoSo", kinhPhiDeTai.MaDeTai);
            return View(kinhPhiDeTai);
        }

        // GET: AdminKinhPhiDeTai/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KinhPhiDeTai kinhPhiDeTai = await db.KinhPhiDeTais.FindAsync(id);
            if (kinhPhiDeTai == null)
            {
                return HttpNotFound();
            }
            db.KinhPhiDeTais.Remove(kinhPhiDeTai);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: AdminKinhPhiDeTai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            KinhPhiDeTai kinhPhiDeTai = await db.KinhPhiDeTais.FindAsync(id);
            db.KinhPhiDeTais.Remove(kinhPhiDeTai);
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
