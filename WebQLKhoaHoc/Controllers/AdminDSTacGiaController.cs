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
    public class AdminDSTacGiaController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminDSTacGia
        public async Task<ActionResult> Index()
        {
            var dSTacGias = db.DSTacGias.Include(d => d.NhaKhoaHoc).Include(d => d.SachGiaoTrinh);
            return View(await dSTacGias.ToListAsync());
        }

        // GET: AdminDSTacGia/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSTacGia dSTacGia = await db.DSTacGias.FindAsync(id);
            if (dSTacGia == null)
            {
                return HttpNotFound();
            }
            return View(dSTacGia);
        }

        // GET: AdminDSTacGia/Create
        public ActionResult Create(int? id, int? manhakhoahoc )
        {

            var dsnguoidathamgia = db.DSTacGias.Where(p => p.MaSach == id).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");        
            ViewBag.manhakhoahoc = manhakhoahoc;
            ViewBag.masach = id;
            ViewBag.tensach = db.SachGiaoTrinhs.Find(id).TenSach;            
            return View();
        }

        // POST: AdminDSTacGia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaSach,MaNKH,LaChuBien")] DSTacGia dSTacGia,int? manhakhoahoc )
        {
            if (ModelState.IsValid)
            {
                db.DSTacGias.Add(dSTacGia);
                await db.SaveChangesAsync();                
                return RedirectToAction("Edit",new { id= dSTacGia.MaSach, manhakhoahoc = manhakhoahoc});
            }


            var dsnguoidathamgia = db.DSTacGias.Where(p => p.MaSach == dSTacGia.MaSach).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.manhakhoahoc = manhakhoahoc;
            ViewBag.masach = dSTacGia.MaSach;
            ViewBag.tensach = db.SachGiaoTrinhs.Find(dSTacGia.MaSach).TenSach;            
            return View(dSTacGia);
        }

        // GET: AdminDSTacGia/Edit/5
        public async Task<ActionResult> Edit(int? id, int? manhakhoahoc )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<DSTacGia> dSTacGia = await db.DSTacGias.Where(p => p.MaSach == id).ToListAsync();
            if (dSTacGia == null)
            {
                return HttpNotFound();
            }            

            ViewBag.manhakhoahoc = manhakhoahoc;
            ViewBag.masach = id;
            ViewBag.tensach = db.SachGiaoTrinhs.Find(id).TenSach;          
            return View(dSTacGia);
        }

        // POST: AdminDSTacGia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaSach,MaNKH,LaChuBien")] List<DSTacGia> dSTacGia,int? manhakhoahoc,int? masach)
        {
            if (ModelState.IsValid)
            {
                foreach (var x in dSTacGia) {
                    db.DSTacGias.AddOrUpdate(x);
                }
                await db.SaveChangesAsync();             
                return RedirectToAction("Edit",new { id = manhakhoahoc});
            }
            ViewBag.manhakhoahoc = manhakhoahoc;
            ViewBag.masach = masach;
            ViewBag.tensach = db.SachGiaoTrinhs.Find(masach).TenSach;           
            return View(dSTacGia);
        }

        // GET: AdminDSTacGia/Delete/5
        public async Task<ActionResult> Delete(int? id,int? mankh, int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSTacGia dSTacGia = await db.DSTacGias.Where(p => p.MaSach == id && p.MaNKH == mankh).FirstOrDefaultAsync();
            if (dSTacGia == null)
            {
                return HttpNotFound();
            }
            ViewBag.manhakhoahoc = manhakhoahoc;
            ViewBag.masach = id;
            ViewBag.mankh = mankh;
            return View(dSTacGia);
        }

        // POST: AdminDSTacGia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id,int mankh,int? manhakhoahoc)
        {
            DSTacGia dSTacGia = await db.DSTacGias.Where(p => p.MaSach == id && p.MaNKH == mankh).FirstOrDefaultAsync();
            db.DSTacGias.Remove(dSTacGia);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = id, manhakhoahoc = manhakhoahoc });
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
