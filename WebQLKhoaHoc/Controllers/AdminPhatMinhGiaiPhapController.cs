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
using WebQLKhoaHoc.Models;

namespace WebQLKhoaHoc.Controllers
{
    [CustomizeAuthorize(Roles = "1")]
    public class AdminPhatMinhGiaiPhapController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminPhatMinhGiaiPhap
        public async Task<ActionResult> Index()
        {
            var phatMinhGiaiPhaps = db.PhatMinhGiaiPhaps;
            return View(await phatMinhGiaiPhaps.ToListAsync());
        }

        // GET: AdminPhatMinhGiaiPhap/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhatMinhGiaiPhap phatMinhGiaiPhap = await db.PhatMinhGiaiPhaps.FindAsync(id);
            if (phatMinhGiaiPhap == null)
            {
                return HttpNotFound();
            }
            return View(phatMinhGiaiPhap);
        }

        // GET: AdminPhatMinhGiaiPhap/Create
        public ActionResult Create()
        {
            var lsnkh = db.NhaKhoaHocs.Select(p => new
            {
                MaNKH = p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            });
            ViewBag.MaNKH = new SelectList(lsnkh, "MaNKH", "TenNKH");
            return View();
        }

        // POST: AdminPhatMinhGiaiPhap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaPM,TenPM,SoHieuPM,MotaPM,DoiTuongSuDung,QuocGiaCap,LinkLienKet,NamCongBo")] PhatMinhGiaiPhap phatMinhGiaiPhap, int MaChuSoHuu)
        {
            if (ModelState.IsValid)
            {
                db.PhatMinhGiaiPhaps.Add(phatMinhGiaiPhap);
                DSPhatMinhNKH dSPhatMinhNKH = new DSPhatMinhNKH
                {
                    MaNKH = MaChuSoHuu,
                    MaPM = phatMinhGiaiPhap.MaPM,
                    LaChuSoHuu = true
                };

                db.DSPhatMinhNKHs.Add(dSPhatMinhNKH);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(phatMinhGiaiPhap);
        }

        // GET: AdminPhatMinhGiaiPhap/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhatMinhGiaiPhap phatMinhGiaiPhap = await db.PhatMinhGiaiPhaps.FindAsync(id);
            if (phatMinhGiaiPhap == null)
            {
                return HttpNotFound();
            }
            return View(phatMinhGiaiPhap);
        }

        // POST: AdminPhatMinhGiaiPhap/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaPM,TenPM,SoHieuPM,MotaPM,DoiTuongSuDung,QuocGiaCap,LinkLienKet,AnhScanGiayChungNhan,AnhChupSanPham1,AnhChupSanPham2,NamCongBo")] PhatMinhGiaiPhap phatMinhGiaiPhap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phatMinhGiaiPhap).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(phatMinhGiaiPhap);
        }

        // GET: AdminPhatMinhGiaiPhap/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhatMinhGiaiPhap phatMinhGiaiPhap = await db.PhatMinhGiaiPhaps.FindAsync(id);
            if (phatMinhGiaiPhap == null)
            {
                return HttpNotFound();
            }
            return View(phatMinhGiaiPhap);
        }

        // POST: AdminPhatMinhGiaiPhap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PhatMinhGiaiPhap phatMinhGiaiPhap = await db.PhatMinhGiaiPhaps.FindAsync(id);
            db.PhatMinhGiaiPhaps.Remove(phatMinhGiaiPhap);
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
