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
using System.IO;
using System.Data.Entity.Migrations;

namespace WebQLKhoaHoc.Controllers
{
    public class AdminBaiBaoController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminBaiBao
        public async Task<ActionResult> Index()
        {
            var baiBaos = db.BaiBaos.Include(b => b.CapTapChi).Include(b => b.PhanLoaiTapChi);
            return View(await baiBaos.ToListAsync());
        }

        // GET: AdminBaiBao/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baiBao = await db.BaiBaos.FindAsync(id);
            if (baiBao == null)
            {
                return HttpNotFound();
            }
            return View(baiBao);
        }

        // GET: AdminBaiBao/Create
        public ActionResult Create()
        {
            ViewBag.MaCapTapChi = new SelectList(db.CapTapChis, "MaCapTapChi", "TenCapTapChi");
            ViewBag.MaLoaiTapChi = new SelectList(db.PhanLoaiTapChis, "MaLoaiTapChi", "TenLoaiTapChi");
            return View();
        }

        // POST: AdminBaiBao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HttpPostedFileBase linkUpload, [Bind(Include = "MaBaiBao,MaISSN,TenBaiBao,LaTrongNuoc,CQXuatBan,MaLoaiTapChi,MaCapTapChi,NamDangBao,TapPhatHanh,SoPhatHanh,TrangBaiBao,LienKetWeb")] BaiBao baiBao)
        {
            if (ModelState.IsValid)
            {
                db.BaiBaos.Add(baiBao);
                await db.SaveChangesAsync();

                db.BaiBaos.Attach(baiBao);
                if (linkUpload != null && linkUpload.ContentLength > 0)
                {
                    string filename = Path.GetFileNameWithoutExtension(linkUpload.FileName) + "_" + baiBao.MaBaiBao.ToString() + Path.GetExtension(linkUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/App_Data/uploads/baibao_file"), filename);
                    linkUpload.SaveAs(path);
                    baiBao.LinkFileUpLoad = filename;

                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaCapTapChi = new SelectList(db.CapTapChis, "MaCapTapChi", "TenCapTapChi", baiBao.MaCapTapChi);
            ViewBag.MaLoaiTapChi = new SelectList(db.PhanLoaiTapChis, "MaLoaiTapChi", "TenLoaiTapChi", baiBao.MaLoaiTapChi);
            return View(baiBao);
        }

        // GET: AdminBaiBao/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baiBao = await db.BaiBaos.FindAsync(id);
            if (baiBao == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCapTapChi = new SelectList(db.CapTapChis, "MaCapTapChi", "TenCapTapChi", baiBao.MaCapTapChi);
            ViewBag.MaLoaiTapChi = new SelectList(db.PhanLoaiTapChis, "MaLoaiTapChi", "TenLoaiTapChi", baiBao.MaLoaiTapChi);
            return View(baiBao);
        }

        // POST: AdminBaiBao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HttpPostedFileBase linkUpload, [Bind(Include = "MaBaiBao,MaISSN,TenBaiBao,LaTrongNuoc,CQXuatBan,MaLoaiTapChi,MaCapTapChi,NamDangBao,TapPhatHanh,SoPhatHanh,TrangBaiBao,LienKetWeb,LinkFileUpLoad")] BaiBao baiBao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(baiBao).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (linkUpload != null && linkUpload.ContentLength > 0)
                {
                    string filename = Path.GetFileNameWithoutExtension(linkUpload.FileName) + "_" + baiBao.MaBaiBao.ToString() + Path.GetExtension(linkUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/App_Data/uploads/baibao_file"), filename);
                    if (!String.IsNullOrEmpty(baiBao.LinkFileUpLoad))
                    {
                        string oldpath = Path.Combine(Server.MapPath("~/App_Data/uploads/baibao_file"), baiBao.LinkFileUpLoad);
                        if (System.IO.File.Exists(oldpath))
                        {
                            System.IO.File.Delete(oldpath);
                        }
                    }
                    linkUpload.SaveAs(path);
                    baiBao.LinkFileUpLoad = filename;
                }
                return RedirectToAction("Index");
            }
            ViewBag.MaCapTapChi = new SelectList(db.CapTapChis, "MaCapTapChi", "TenCapTapChi", baiBao.MaCapTapChi);
            ViewBag.MaLoaiTapChi = new SelectList(db.PhanLoaiTapChis, "MaLoaiTapChi", "TenLoaiTapChi", baiBao.MaLoaiTapChi);
            return View(baiBao);
        }

        // GET: AdminBaiBao/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baiBao = await db.BaiBaos.FindAsync(id);
            if (baiBao == null)
            {
                return HttpNotFound();
            }


            var baibaodetai = db.DSBaiBaoDeTais.Where(p => p.MaBaiBao == id).ToList();
            foreach (var item in baibaodetai)
            {
                db.DSBaiBaoDeTais.Remove(item);
                db.SaveChanges();
            }

            var nguoithamgia = db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == id).ToList();
            foreach (var item in nguoithamgia)
            {
                db.DSNguoiThamGiaBaiBaos.Remove(item);
            }

            var linhvuc = baiBao.LinhVucs.ToList();
            foreach (var item in linhvuc)
            {
                baiBao.LinhVucs.Remove(item);
            }


            db.BaiBaos.Remove(baiBao);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: AdminBaiBao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BaiBao baiBao = await db.BaiBaos.FindAsync(id);
            db.BaiBaos.Remove(baiBao);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<ActionResult> DanhSachNguoiThamGiaBaiBao()
        {
            var dSNguoiThamGiaBaiBaos = db.DSNguoiThamGiaBaiBaos.Include(d => d.BaiBao).Include(d => d.NhaKhoaHoc);
            return View(await dSNguoiThamGiaBaiBaos.ToListAsync());
        }

        /* create DS NGuoi tham gia bai bao*/
        public ActionResult CreateDanhSachNguoiThamGiaBaiBao(int? id)
        {

            var dsnguoidathamgia = db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == id).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.tenbaibao = db.BaiBaos.Find(id).TenBaiBao;
            ViewBag.mabaibao = id;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDanhSachNguoiThamGiaBaiBao([Bind(Include = "MaBaiBao,MaNKH,LaTacGiaChinh")] DSNguoiThamGiaBaiBao dSNguoiThamGiaBaiBao)
        {
            if (ModelState.IsValid)
            {
                db.DSNguoiThamGiaBaiBaos.Add(dSNguoiThamGiaBaiBao);
                await db.SaveChangesAsync();
                return RedirectToAction("DanhSachNguoiThamGiaBaiBao");
            }
            var dsnguoidathamgia = db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == dSNguoiThamGiaBaiBao.MaBaiBao).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.tenbaibao = db.BaiBaos.Find(dSNguoiThamGiaBaiBao.MaBaiBao).TenBaiBao;
            ViewBag.mabaibao = dSNguoiThamGiaBaiBao.MaBaiBao;
            return View();
        }

        /* EDit nguoi tham gia bai bao*/
        public async Task<ActionResult> EditDanhSachNguoiThamGiaBaiBao(int? id, int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSNguoiThamGiaBaiBao dSNguoiThamGiaBaiBao = await db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == id && p.MaNKH == manhakhoahoc).FirstOrDefaultAsync();
            if (dSNguoiThamGiaBaiBao == null)
            {
                return HttpNotFound();
            }
            return View(dSNguoiThamGiaBaiBao);
        }

        // POST: AdminDSNguoiThamGiaBaiBao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDanhSachNguoiThamGiaBaiBao([Bind(Include = "MaBaiBao,MaNKH,LaTacGiaChinh")] DSNguoiThamGiaBaiBao dSNguoiThamGiaBaiBao)
        {
            if (ModelState.IsValid)
            {

                db.DSNguoiThamGiaBaiBaos.AddOrUpdate(dSNguoiThamGiaBaiBao);

                await db.SaveChangesAsync();
                return RedirectToAction("DanhSachNguoiThamGiaBaiBao");
            }
            return View(dSNguoiThamGiaBaiBao);
        }

        /* DElete guoi tha gia bai bao*/
        public async Task<ActionResult> DeleteDanhSachNguoiThamGiaBaiBao(int? id, int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSNguoiThamGiaBaiBao dSNguoiThamGiaBaiBao = await db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == id && p.MaNKH == manhakhoahoc).FirstOrDefaultAsync();
            if (dSNguoiThamGiaBaiBao == null)
            {
                return HttpNotFound();
            }
            db.DSNguoiThamGiaBaiBaos.Remove(dSNguoiThamGiaBaiBao);
            await db.SaveChangesAsync();
            return RedirectToAction("DanhSachNguoiThamGiaBaiBao");
        }

        /* Linhvuc baibao con Create chu hoan thanh*/
        public async Task<ActionResult> LinhVucBaiBao()
        {

            List<BaiBao> baiBao = await db.BaiBaos.ToListAsync();
            if (baiBao == null)
            {
                return HttpNotFound();
            }
            return View(baiBao);
        }

        public async Task<ActionResult> EditLinhVucBaiBao(int? id, int? idBaiBao)
        {
            if (id == null || idBaiBao == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BaiBao baiBao = await db.BaiBaos.FindAsync(idBaiBao);
            if (baiBao == null)
            {
                return HttpNotFound();
            }

            var lv = baiBao.LinhVucs.Select(p=>p.MaLinhVuc).ToList();
            var linhvuc = await db.LinhVucs.Where(p=> !lv.Contains(p.MaLinhVuc)).ToListAsync();
            ViewBag.MaLinhVucCu = id;
            ViewBag.MaLinhVuc = new SelectList(linhvuc, "MaLinhVuc", "TenLinhVuc");
            return View(baiBao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditLinhVucBaiBao(int MaLinhVuc, int MaLinhVucCu, int MaBaiBao)
        {
            if (ModelState.IsValid)
            {
                BaiBao baiBao = await db.BaiBaos.FindAsync(MaBaiBao);
                LinhVuc lvcu = await db.LinhVucs.FindAsync(MaLinhVucCu);
                LinhVuc lv = await db.LinhVucs.FindAsync(MaLinhVuc);


                baiBao.LinhVucs.Remove(lvcu);
                baiBao.LinhVucs.Add(lv);
                await db.SaveChangesAsync();
                return RedirectToAction("LinhVucBaiBao");
            }

            BaiBao baibao = await db.BaiBaos.FindAsync(MaBaiBao);
            var linhvuc = await db.LinhVucs.Except(baibao.LinhVucs).ToListAsync();
            ViewBag.MaLinhVuc = new SelectList(linhvuc, "MaLinhVuc", "TenLinhVuc");
            return View(baibao);

        }    

        
        public async Task<ActionResult> CreateLinhVucBaiBaoPost(int? id, int? idBaiBao)
        {

            if (id == null || idBaiBao == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baiBao = await db.BaiBaos.FindAsync(idBaiBao);
            LinhVuc linhVuc = await db.LinhVucs.FindAsync(id);
            if (baiBao == null || linhVuc == null)
            {
                return HttpNotFound();
            }
            var linhvuc = await db.LinhVucs.Except(baiBao.LinhVucs).ToListAsync();
            ViewBag.MaLinhVuc = new SelectList(linhvuc, "MaLinhVuc", "TenLinhVuc");
            return View(baiBao);
        }

        public async Task<ActionResult> DeleteLinhVucBaiBao(int? id, int? idBaiBao )
        {
            if (id == null || idBaiBao == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baibao = await db.BaiBaos.Where(p => p.MaBaiBao == idBaiBao).FirstOrDefaultAsync();
            LinhVuc linhvuc = await db.LinhVucs.Where(p => p.MaLinhVuc == id).FirstOrDefaultAsync();
            if (baibao == null || linhvuc == null)
            {
                return HttpNotFound();
            }
            baibao.LinhVucs.Remove(linhvuc);
            await db.SaveChangesAsync();
            return RedirectToAction("LinhVucBaiBao");
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
