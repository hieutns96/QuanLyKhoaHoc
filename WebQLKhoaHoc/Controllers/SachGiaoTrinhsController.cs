﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebQLKhoaHoc;
using WebQLKhoaHoc.Models;
using System.Data.Entity.Migrations;
using WebGrease.Css.Extensions;
using LinqKit;

namespace WebQLKhoaHoc.Controllers
{
    public class SachGiaoTrinhsController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();
        private QLKHRepository QLKHrepo = new QLKHRepository();

        // GET: SachGiaoTrinhs
        public async Task<ActionResult> Index(int? Page_No, [Bind(Include = "MaLoai,MaNXB,MaLinhVuc,SearchValue")] SachGiaoTrinhSearchViewModel sachGiaoTrinh, int? nkhId)
        {
            var pre = PredicateBuilder.True<SachGiaoTrinh>();


            if (nkhId == null)
            {
                if (!String.IsNullOrEmpty(sachGiaoTrinh.MaLinhVuc))
                {
                    if (sachGiaoTrinh.MaLinhVuc[0] == 'a')
                        pre = pre.And(p => p.MaLinhVuc.ToString() == sachGiaoTrinh.MaLinhVuc.Substring(1, sachGiaoTrinh.MaLinhVuc.Length - 1));

                    else
                        pre = pre.And(p => p.LinhVuc.MaNhomLinhVuc.ToString() == sachGiaoTrinh.MaLinhVuc);

                }
                if (!String.IsNullOrEmpty(sachGiaoTrinh.MaLoai))
                {
                    pre = pre.And(p => p.MaLoai.ToString() == sachGiaoTrinh.MaLoai);

                }
                if (!String.IsNullOrEmpty(sachGiaoTrinh.MaNXB))
                {
                    pre = pre.And(p => p.MaLoai.ToString() == sachGiaoTrinh.MaNXB);
                    ;
                }
                if (!String.IsNullOrEmpty(sachGiaoTrinh.SearchValue))
                {
                    pre = pre.And(p => p.TenSach.ToLower().Contains(sachGiaoTrinh.SearchValue.ToLower()));

                }


            }
            else
            {

                var maSachs = db.DSTacGias.Where(p => p.MaNKH == nkhId).Select(p => p.MaSach).ToList();
                pre = pre.And(p => maSachs.Contains(p.MaSach));

            }

            ViewBag.MaLinhVuc = new SelectList(QLKHrepo.GetListMenuLinhVuc(), "Id", "TenLinhVuc");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
            ViewBag.MaLoai = new SelectList(db.PhanLoaiSaches, "MaLoai", "TenLoai");
            ViewBag.SearchValue = sachGiaoTrinh.SearchValue;

            int No_Of_Page = (Page_No ?? 1);

            var sachGiaoTrinhs = db.SachGiaoTrinhs.Include(s => s.LinhVuc).Include(s => s.NhaXuatBan).Include(s => s.PhanLoaiSach).Include(s => s.DSTacGias).AsExpandable().Where(pre).OrderBy(p => p.MaSach).Skip((No_Of_Page - 1) * 6).Take(6).ToList();
            decimal totalItem = (decimal)db.SachGiaoTrinhs.Where(pre).OrderBy(p => p.MaSach).Count();
            int totalPage = (int)Math.Ceiling(totalItem / 6);
            ViewBag.TotalItem = totalItem;
            IPagedList<SachGiaoTrinh> pageOrders = new StaticPagedList<SachGiaoTrinh>(sachGiaoTrinhs, No_Of_Page, 1, totalPage);
            return View(pageOrders);
        }

        // GET: SachGiaoTrinhs/sachgiaotrinhls/5
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
        public async Task<ActionResult> GetAllImage(int? id)
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
            List<ImageViewModel> lstImage = new List<ImageViewModel>
            {
                 new ImageViewModel(sachGiaoTrinh.AnhBia1),
                new ImageViewModel(sachGiaoTrinh.AnhBia4),
                new ImageViewModel(sachGiaoTrinh.AnhBiaISBN)
            };
            return View(lstImage);
        }

        // GET: SachGiaoTrinhs/Create
        public ActionResult Create()
        {
            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
            ViewBag.MaLoai = new SelectList(db.PhanLoaiSaches, "MaLoai", "TenLoai");
            ViewBag.DSTacGia = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH");
            return View();
        }

        // POST: SachGiaoTrinhs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more sachgiaotrinhls see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(List<string> DSTacGia, [Bind(Include = "MaSach,MaISBN,TenSach,MaLoai,MaLinhVuc,MaNXB,NamXuatBan")] SachGiaoTrinh sachGiaoTrinh)
        {
            if (ModelState.IsValid)
            {
                db.SachGiaoTrinhs.Add(sachGiaoTrinh);

                UserLoginViewModel user = (UserLoginViewModel)Session["user"];
                db.DSTacGias.Add(new DSTacGia
                {
                    LaChuBien = true,
                    MaSach = sachGiaoTrinh.MaSach,
                    MaNKH = user.MaNKH
                });

                if (DSTacGia != null)
                {
                    foreach (var mankh in DSTacGia)
                    {
                        DSTacGia tacGia = new DSTacGia
                        {
                            LaChuBien = false,
                            MaSach = sachGiaoTrinh.MaSach,
                            MaNKH = Int32.Parse(mankh)
                        };
                        db.DSTacGias.Add(tacGia);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();
            var tacgiaphu = db.NhaKhoaHocs.Where(p => p.DSTacGias.Any(d => d.MaSach == sachGiaoTrinh.MaSach && d.LaChuBien == false)).Select(p => p.MaNKH).ToList();

            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc", sachGiaoTrinh.MaLinhVuc);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sachGiaoTrinh.MaNXB);
            ViewBag.MaLoai = new SelectList(db.PhanLoaiSaches, "MaLoai", "TenLoai", sachGiaoTrinh.MaLoai);
            ViewBag.DSTacGia = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH", tacgiaphu);
            return View(sachGiaoTrinh);
        }

        // GET: SachGiaoTrinhs/Edit/5
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

            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();
            var tacgiaphu = db.NhaKhoaHocs.Where(p => p.DSTacGias.Any(d => d.MaSach == sachGiaoTrinh.MaSach && d.LaChuBien == false)).Select(p => p.MaNKH).ToList();

            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc", sachGiaoTrinh.MaLinhVuc);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sachGiaoTrinh.MaNXB);
            ViewBag.MaLoai = new SelectList(db.PhanLoaiSaches, "MaLoai", "TenLoai", sachGiaoTrinh.MaLoai);
            ViewBag.DSTacGia = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH", tacgiaphu);
            return View(sachGiaoTrinh);
        }

        // POST: SachGiaoTrinhs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more sachgiaotrinhls see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(List<string> DSTacGia, [Bind(Include = "MaSach,MaISBN,TenSach,MaLoai,MaLinhVuc,MaNXB,NamXuatBan")] SachGiaoTrinh sachGiaoTrinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sachGiaoTrinh).State = EntityState.Modified;
                if (DSTacGia != null)
                {
                    db.DSTacGias.Where(p => p.MaSach == sachGiaoTrinh.MaSach && p.LaChuBien == false).ForEach(z => db.DSTacGias.Remove(z));
                    foreach (var mankh in DSTacGia)
                    {
                        DSTacGia tacGia = new DSTacGia
                        {
                            LaChuBien = false,
                            MaSach = sachGiaoTrinh.MaSach,
                            MaNKH = Int32.Parse(mankh)
                        };
                        sachGiaoTrinh.DSTacGias.Add(tacGia);
                    }
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();
            var tacgiaphu = db.NhaKhoaHocs.Where(p => p.DSTacGias.Any(d => d.MaSach == sachGiaoTrinh.MaSach && d.LaChuBien == false)).Select(p => p.MaNKH).ToList();

            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc", sachGiaoTrinh.MaLinhVuc);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sachGiaoTrinh.MaNXB);
            ViewBag.MaLoai = new SelectList(db.PhanLoaiSaches, "MaLoai", "TenLoai", sachGiaoTrinh.MaLoai);
            ViewBag.DSTacGia = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH", tacgiaphu);
            return View(sachGiaoTrinh);
        }

        // POST: SachGiaoTrinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SachGiaoTrinh sachGiaoTrinh = await db.SachGiaoTrinhs.FindAsync(id);
            db.SachGiaoTrinhs.Remove(sachGiaoTrinh);
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
