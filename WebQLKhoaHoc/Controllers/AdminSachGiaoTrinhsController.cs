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
    public class AdminSachGiaoTrinhsController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminSachGiaoTrinhs
        public async Task<ActionResult> Index()
        {
            var sachGiaoTrinhs = db.SachGiaoTrinhs.Include(s => s.LinhVuc).Include(s => s.NhaXuatBan).Include(s => s.PhanLoaiSach);
            return View(await sachGiaoTrinhs.ToListAsync());
        }

        // GET: AdminSachGiaoTrinhs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SachGiaoTrinh sachGiaoTrinh = await db.SachGiaoTrinhs.FindAsync(id);
            if (sachGiaoTrinh == null)
            {
                return HttpNotFound();
            }
            return View(sachGiaoTrinh);
        }

        // GET: AdminSachGiaoTrinhs/Create
        public ActionResult Create()
        {
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
            ViewBag.MaLoai = new SelectList(db.PhanLoaiSaches, "MaLoai", "TenLoai");
            var lsnkh = db.NhaKhoaHocs.Select(p => new
            {
               MaNKH = p.MaNKH,
               TenNKH = p.HoNKH+" "+p.TenNKH
            });
            ViewBag.MaNKH = new SelectList(lsnkh,"MaNKH","TenNKH");
            return View();
        }

        // POST: AdminSachGiaoTrinhs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaSach,MaISBN,TenSach,MaLoai,MaLinhVuc,MaNXB,NamXuatBan")] SachGiaoTrinh sachGiaoTrinh,int MaChuBien)
        {
            if (ModelState.IsValid)
            {   
                db.SachGiaoTrinhs.Add(sachGiaoTrinh);                
                await db.SaveChangesAsync();
                DSTacGia dstacgia = new DSTacGia {
                    MaNKH = MaChuBien,
                    MaSach = sachGiaoTrinh.MaSach,
                    LaChuBien = true
                };
                sachGiaoTrinh.DSTacGias.Add(dstacgia);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc", sachGiaoTrinh.MaLinhVuc);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sachGiaoTrinh.MaNXB);
            ViewBag.MaLoai = new SelectList(db.PhanLoaiSaches, "MaLoai", "TenLoai", sachGiaoTrinh.MaLoai);
            return View(sachGiaoTrinh);
        }

        // GET: AdminSachGiaoTrinhs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SachGiaoTrinh sachGiaoTrinh = await db.SachGiaoTrinhs.FindAsync(id);
            if (sachGiaoTrinh == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc", sachGiaoTrinh.MaLinhVuc);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sachGiaoTrinh.MaNXB);
            ViewBag.MaLoai = new SelectList(db.PhanLoaiSaches, "MaLoai", "TenLoai", sachGiaoTrinh.MaLoai);
            return View(sachGiaoTrinh);
        }

        // POST: AdminSachGiaoTrinhs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaSach,MaISBN,TenSach,MaLoai,MaLinhVuc,MaNXB,NamXuatBan")] SachGiaoTrinh sachGiaoTrinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sachGiaoTrinh).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc", sachGiaoTrinh.MaLinhVuc);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sachGiaoTrinh.MaNXB);
            ViewBag.MaLoai = new SelectList(db.PhanLoaiSaches, "MaLoai", "TenLoai", sachGiaoTrinh.MaLoai);
            return View(sachGiaoTrinh);
        }

        // GET: AdminSachGiaoTrinhs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SachGiaoTrinh sachGiaoTrinh = await db.SachGiaoTrinhs.FindAsync(id);
            if (sachGiaoTrinh == null)
            {
                return HttpNotFound();
            }
            return View(sachGiaoTrinh);
        }

        // POST: AdminSachGiaoTrinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SachGiaoTrinh sachGiaoTrinh = await db.SachGiaoTrinhs.FindAsync(id);
            db.SachGiaoTrinhs.Remove(sachGiaoTrinh);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DanhSachTacGia()
        {
            var dSTacGias = db.DSTacGias.Include(d => d.NhaKhoaHoc).Include(d => d.SachGiaoTrinh);
            return View(await dSTacGias.ToListAsync());
        }


        /*create danh sach tac gia*/
        public ActionResult CreateDanhSachTacGia(int? id)
        {

            var dsnguoidathamgia = db.DSTacGias.Where(p => p.MaSach == id).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.masach = id;
            ViewBag.TenSach = db.SachGiaoTrinhs.Find(id).TenSach;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDanhSachTacGia([Bind(Include = "MaSach,MaNKH,LaChuBien")] DSTacGia dSTacGia)
        {
            if (ModelState.IsValid)
            {
                db.DSTacGias.Add(dSTacGia);
                await db.SaveChangesAsync();
                return RedirectToAction("DanhSachTacGia");
            }


            var dsnguoidathamgia = db.DSTacGias.Where(p => p.MaSach == dSTacGia.MaSach).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");            
            ViewBag.masach = dSTacGia.MaSach;
            ViewBag.TenSach = db.SachGiaoTrinhs.Find(dSTacGia.MaSach).TenSach;
            return View(dSTacGia);
        }


        /*edit danh sach tac gia*/
        public async Task<ActionResult> EditDanhSachTacGia(int? id,int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSTacGia dSTacGia = await db.DSTacGias.Where(p => p.MaSach == id && p.MaNKH == manhakhoahoc).FirstOrDefaultAsync();
            if (dSTacGia == null)
            {
                return HttpNotFound();
            }           
            ViewBag.tensach = db.SachGiaoTrinhs.Find(id).TenSach;
            return View(dSTacGia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDanhSachTacGia([Bind(Include = "MaSach,MaNKH,LaChuBien")] DSTacGia dSTacGia)
        {
            if (ModelState.IsValid)
            {
               db.DSTacGias.AddOrUpdate(dSTacGia);               
                await db.SaveChangesAsync();
                return RedirectToAction("DanhSachTacGia");
            }
           
            ViewBag.tensach = db.SachGiaoTrinhs.Find(dSTacGia.MaSach).TenSach;
            return View(dSTacGia);
        }



        /* delete danh sach tac gia*/
        public async Task<ActionResult> DeleteDanhSachTacGia(int? id, int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSTacGia dSTacGia = await db.DSTacGias.Where(p => p.MaSach == id && p.MaNKH == manhakhoahoc).FirstOrDefaultAsync();
            if (dSTacGia == null)
            {
                return HttpNotFound();
            }         
            db.DSTacGias.Remove(dSTacGia);
            await db.SaveChangesAsync();
           
            return RedirectToAction("DanhSachTacGia");
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
