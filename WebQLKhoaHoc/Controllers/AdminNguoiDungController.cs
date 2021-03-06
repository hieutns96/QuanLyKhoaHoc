﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
    public class AdminNguoiDungController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminNguoiDung
        public async Task<ActionResult> Index()
        {
            var nguoiDungs = db.NguoiDungs.Include(n => n.ChucNang).Include(n => n.NhaKhoaHoc);
            return View(await nguoiDungs.ToListAsync());
        }

      
        // GET: AdminNguoiDung/Create
        public ActionResult Create()
        {
            ViewBag.MaChucNang = new SelectList(db.ChucNangs, "MaChucNang", "TenChucNang");

            return View();
        }

        // POST: AdminNguoiDung/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Username,Password,MaChucNang")] NguoiDungViewModel nguoiDung)
        {
            if (ModelState.IsValid)
            {
                string salt = "".GenRandomKey(); //update by Khiet
                NguoiDung newNguoiDung = new NguoiDung();
                newNguoiDung.Usernames = nguoiDung.Username;
                newNguoiDung.Passwords = Encryptor.MD5Hash(nguoiDung.Password + salt); //update by Khiet
                newNguoiDung.RandomKey = salt;
                newNguoiDung.IsActive = true;
                db.NguoiDungs.Add(newNguoiDung);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaChucNang = new SelectList(db.ChucNangs, "MaChucNang", "TenChucNang", nguoiDung.MaChucNang);
           
            return View(nguoiDung);
        }

        // GET: AdminNguoiDung/Edit/5
        public async Task<ActionResult> UnLock(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung =  db.NguoiDungs.Where(p => p.ID == id).FirstOrDefault();
            if (nguoiDung != null)
            {
                nguoiDung.IsActive = !nguoiDung.IsActive;
            }
            db.NguoiDungs.AddOrUpdate(nguoiDung);
        
            await db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        // GET: AdminNguoiDung/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = await db.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaChucNang = new SelectList(db.ChucNangs, "MaChucNang", "TenChucNang", nguoiDung.MaChucNang);
            ViewBag.MaNKH = new SelectList(db.NhaKhoaHocs, "MaNKH", "MaNKHHoSo", nguoiDung.MaNKH);
            return View(nguoiDung);
        }

        // POST: AdminNguoiDung/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Usernames,MaChucNang")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                NguoiDung user = db.NguoiDungs.Where( p => p.ID == nguoiDung.ID ).FirstOrDefault();
                if (user != null)
                {
                    user.MaChucNang = nguoiDung.MaChucNang;
                }
                db.NguoiDungs.AddOrUpdate(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaChucNang = new SelectList(db.ChucNangs, "MaChucNang", "TenChucNang", nguoiDung.MaChucNang);
            ViewBag.MaNKH = new SelectList(db.NhaKhoaHocs, "MaNKH", "MaNKHHoSo", nguoiDung.MaNKH);
            return View(nguoiDung);
        }

        // GET: AdminNguoiDung/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = await db.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // POST: AdminNguoiDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NguoiDung nguoiDung = await db.NguoiDungs.FindAsync(id);
            db.NguoiDungs.Remove(nguoiDung);
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
