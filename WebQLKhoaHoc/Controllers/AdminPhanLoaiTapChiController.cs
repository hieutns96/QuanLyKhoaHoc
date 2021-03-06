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
using WebQLKhoaHoc.Models;

namespace WebQLKhoaHoc.Controllers
{
    [CustomizeAuthorize(Roles = "1")]
    public class AdminPhanLoaiTapChiController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminPhanLoaiTapChi
        public async Task<ActionResult> Index()
        {
            return View(await db.PhanLoaiTapChis.ToListAsync());
        }

      
        // GET: AdminPhanLoaiTapChi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPhanLoaiTapChi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaLoaiTapChi,TenLoaiTapChi")] PhanLoaiTapChi phanLoaiTapChi)
        {
            if (ModelState.IsValid)
            {
                db.PhanLoaiTapChis.Add(phanLoaiTapChi);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(phanLoaiTapChi);
        }

        // GET: AdminPhanLoaiTapChi/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoaiTapChi phanLoaiTapChi = await db.PhanLoaiTapChis.FindAsync(id);
            if (phanLoaiTapChi == null)
            {
                return HttpNotFound();
            }
            return View(phanLoaiTapChi);
        }

        // POST: AdminPhanLoaiTapChi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaLoaiTapChi,TenLoaiTapChi")] PhanLoaiTapChi phanLoaiTapChi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phanLoaiTapChi).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(phanLoaiTapChi);
        }

        // GET: AdminPhanLoaiTapChi/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanLoaiTapChi phanLoaiTapChi = await db.PhanLoaiTapChis.FindAsync(id);
            if (phanLoaiTapChi == null)
            {
                return HttpNotFound();
            }
            return View(phanLoaiTapChi);
        }

        // POST: AdminPhanLoaiTapChi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PhanLoaiTapChi phanLoaiTapChi = await db.PhanLoaiTapChis.FindAsync(id);
            List<BaiBao> baiBaos = await db.BaiBaos.Where(p => p.MaLoaiTapChi == phanLoaiTapChi.MaLoaiTapChi).ToListAsync();
            foreach (var baiBao in baiBaos)
            {
                baiBao.PhanLoaiTapChi = null;
            }          
            db.PhanLoaiTapChis.Remove(phanLoaiTapChi);
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
