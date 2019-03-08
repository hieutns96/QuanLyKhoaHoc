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
using System.Data.Entity.Migrations;

namespace WebQLKhoaHoc.Controllers
{
    public class AdminDSNguoiThamGiaDeTaiController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminDSNguoiThamGiaDeTai
        public async Task<ActionResult> Index()
        {
            var dSNguoiThamGiaDeTais = db.DSNguoiThamGiaDeTais.Include(d => d.DeTai).Include(d => d.NhaKhoaHoc);
            return View(await dSNguoiThamGiaDeTais.ToListAsync());
        }

        // GET: AdminDSNguoiThamGiaDeTai/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSNguoiThamGiaDeTai dSNguoiThamGiaDeTai = await db.DSNguoiThamGiaDeTais.FindAsync(id);
            if (dSNguoiThamGiaDeTai == null)
            {
                return HttpNotFound();
            }
            return View(dSNguoiThamGiaDeTai);
        }

        // GET: AdminDSNguoiThamGiaDeTai/Create
        public ActionResult Create(int? id,int? manhakhoahoc)
        {
            var dsnguoidathamgia = db.DSNguoiThamGiaDeTais.Where(p => p.MaDeTai == id).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.TenDeTai = db.DeTais.Find(id).TenDeTai;
            ViewBag.MaDeTai = id;
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View();
        }

        // POST: AdminDSNguoiThamGiaDeTai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaDeTai,MaNKH,LaChuNhiem")] DSNguoiThamGiaDeTai dSNguoiThamGiaDeTai,int ? manhakhoahoc)
        {
            if (ModelState.IsValid)
            {
                db.DSNguoiThamGiaDeTais.Add(dSNguoiThamGiaDeTai);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit",new { id=dSNguoiThamGiaDeTai.MaDeTai, manhakhoahoc = manhakhoahoc});
            }

            var dsnguoidathamgia = db.DSNguoiThamGiaDeTais.Where(p => p.MaDeTai == dSNguoiThamGiaDeTai.MaDeTai).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.TenDeTai = db.DeTais.Find(dSNguoiThamGiaDeTai.MaDeTai).TenDeTai;
            ViewBag.MaDeTai = dSNguoiThamGiaDeTai.MaDeTai;
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View();
        }

        // GET: AdminDSNguoiThamGiaDeTai/Edit/5
        public async Task<ActionResult> Edit(int? id,int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<DSNguoiThamGiaDeTai> dSNguoiThamGiaDeTai = await db.DSNguoiThamGiaDeTais.Where(p => p.MaDeTai == id).ToListAsync();
            if (dSNguoiThamGiaDeTai == null)
            {
                return HttpNotFound();
            }
            ViewBag.manhakhoahoc = manhakhoahoc;
            ViewBag.madetai = id;
            return View(dSNguoiThamGiaDeTai);
        }

        // POST: AdminDSNguoiThamGiaDeTai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaDeTai,MaNKH,LaChuNhiem")] List<DSNguoiThamGiaDeTai> dSNguoiThamGiaDeTai,int? manhakhoahoc,int? madetai)
        {
            if (ModelState.IsValid)
            {
                foreach (var x in dSNguoiThamGiaDeTai)
                {
                    db.DSNguoiThamGiaDeTais.AddOrUpdate(x);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Edit",new { id = madetai , manhakhoahoc = manhakhoahoc});
            }


            ViewBag.manhakhoahoc = manhakhoahoc;
            ViewBag.madetai = madetai;
            return View(dSNguoiThamGiaDeTai);
        }

        // GET: AdminDSNguoiThamGiaDeTai/Delete/5
        public async Task<ActionResult> Delete(int? id,int? mankh,int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSNguoiThamGiaDeTai dSNguoiThamGiaDeTai = await db.DSNguoiThamGiaDeTais.Where(p=>p.MaDeTai == id && p.MaNKH == mankh).FirstOrDefaultAsync();
            if (dSNguoiThamGiaDeTai == null)
            {
                return HttpNotFound();
            }
            ViewBag.madetai = id;
            ViewBag.mankh = mankh;
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(dSNguoiThamGiaDeTai);
        }

        // POST: AdminDSNguoiThamGiaDeTai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id,int? mankh, int? manhakhoahoc)
        {
            DSNguoiThamGiaDeTai dSNguoiThamGiaDeTai = await db.DSNguoiThamGiaDeTais.Where(p=>p.MaDeTai==id && p.MaNKH == mankh).FirstOrDefaultAsync();
            db.DSNguoiThamGiaDeTais.Remove(dSNguoiThamGiaDeTai);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit",new { id = id, manhakhoahoc = manhakhoahoc});
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
