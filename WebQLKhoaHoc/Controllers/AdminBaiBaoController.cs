﻿using System;
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
using WebQLKhoaHoc.Models;
using System.Text.RegularExpressions;

namespace WebQLKhoaHoc.Controllers
{
    [CustomizeAuthorize(Roles = "1")]
    public class AdminBaiBaoController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminBaiBao
        public async Task<ActionResult> Index()
        {
            var baiBaos = db.BaiBaos.Include(b => b.CapTapChi).Include(b => b.PhanLoaiTapChi);
            return View(await baiBaos.ToListAsync());
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
            await db.SaveChangesAsync();
            return View(baiBao);
        }

        // POST: AdminBaiBao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BaiBao baiBao = await db.BaiBaos.FindAsync(id);
            foreach (var item in baiBao.DSBaiBaoDeTais.ToList())
            {
                db.DSBaiBaoDeTais.Remove(item);
            }

            
            foreach (var item in baiBao.DSNguoiThamGiaBaiBaos.ToList())
            {
                db.DSNguoiThamGiaBaiBaos.Remove(item);
            }

            
            foreach (var item in baiBao.LinhVucs.ToList())
            {
                baiBao.LinhVucs.Remove(item);
            }


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

                DSNguoiThamGiaBaiBao tacGia = await db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == dSNguoiThamGiaBaiBao.MaBaiBao && p.MaNKH == dSNguoiThamGiaBaiBao.MaNKH).FirstOrDefaultAsync();
                if (tacGia != null)
                {
                    tacGia.LaTacGiaChinh = dSNguoiThamGiaBaiBao.LaTacGiaChinh;
                }
                else
                {
                    db.DSNguoiThamGiaBaiBaos.Remove(dSNguoiThamGiaBaiBao);
                }

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
            
            ViewBag.MaLinhVucCu = id;
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc", id);
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
            ViewBag.MaLinhVucCu = MaLinhVuc;
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc",MaLinhVucCu);
            return View(baibao);

        }


        public async Task<ActionResult> CreateLinhVucBaiBao(int? id)
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

           
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc");
            return View(baiBao);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateLinhVucBaiBao(int id,int MaLinhVuc)
        {
            if (ModelState.IsValid)
            {
                BaiBao baiBao = await db.BaiBaos.FindAsync(id);
                LinhVuc lv = await db.LinhVucs.FindAsync(MaLinhVuc);
                
                baiBao.LinhVucs.Add(lv);
                await db.SaveChangesAsync();
                return RedirectToAction("LinhVucBaiBao");
            }

            BaiBao baibao = await db.BaiBaos.FindAsync(id);
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc");
            return View(baibao);
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

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<ActionResult> ImportCSVFile(HttpPostedFileBase fileCSV)
        {

            if (fileCSV != null && fileCSV.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileCSV.FileName);
                if (extension == ".csv")
                {
                    string filename = Path.GetFileName(fileCSV.FileName);
                    string path = Path.Combine(Server.MapPath("~/App_Data/uploads/csv"), filename);

                    try
                    {
                        fileCSV.SaveAs(path);
                        AddRecordFromCSV(path);
                    }
                    catch (Exception exception)
                    {
                        TempData["ImportCSVFileError"] = exception.Message;
                    }
                }
                else
                {
                    TempData["ImportCSVFileError"] = "Xin vui lòng nhập file định dạng CSV";
                }
            }
            return RedirectToAction("Index");
        }

        public void AddRecordFromCSV(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                try
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {

                        string[] resultArray;
                        BaiBao baibao = new BaiBao();
                        
                        Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        resultArray = r.Split(line);
                        string maissn = resultArray[0];
                        BaiBao checkbaibao = db.BaiBaos.Where(p => p.MaISSN == maissn).FirstOrDefault();
                        if (checkbaibao == null)
                        {
                            //maissn,tenbaibao,cqxuatban,tapphathanh,sophathanh,trangbaibao,lienketweb
                            baibao.MaISSN = resultArray[0];
                            baibao.TenBaiBao = resultArray[1];
                            baibao.CQXuatBan = resultArray[2];
                            baibao.TapPhatHanh = resultArray[3];
                            baibao.SoPhatHanh = resultArray[4];
                            baibao.TrangBaiBao = resultArray[5];
                            baibao.LienKetWeb = resultArray[6];

                            db.BaiBaos.Add(baibao);
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

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
