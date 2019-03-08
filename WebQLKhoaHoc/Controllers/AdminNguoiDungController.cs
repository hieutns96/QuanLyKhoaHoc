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
    public class AdminNguoiDungController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminNguoiDung
        public async Task<ActionResult> Index()
        {
            var nguoiDungs = db.NguoiDungs.Include(n => n.ChucNang).Include(n => n.NhaKhoaHoc);
            return View(await nguoiDungs.ToListAsync());
        }

        // GET: AdminNguoiDung/Details/5
        public async Task<ActionResult> Details(string id)
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

        // GET: AdminNguoiDung/Create
        public ActionResult Create()
        {
            ViewBag.MaChucNang = new SelectList(db.ChucNangs, "MaChucNang", "TenChucNang");
            ViewBag.MaNKH = new SelectList(db.NhaKhoaHocs, "MaNKH", "MaNKHHoSo");
            return View();
        }

        // POST: AdminNguoiDung/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Usernames,Passwords,MaNKH,MaChucNang")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                db.NguoiDungs.Add(nguoiDung);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaChucNang = new SelectList(db.ChucNangs, "MaChucNang", "TenChucNang", nguoiDung.MaChucNang);
            ViewBag.MaNKH = new SelectList(db.NhaKhoaHocs, "MaNKH", "MaNKHHoSo", nguoiDung.MaNKH);
            return View(nguoiDung);
        }

        // GET: AdminNguoiDung/Edit/5
        public async Task<ActionResult> Edit(string id)
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
        public async Task<ActionResult> Edit([Bind(Include = "Usernames,Passwords,MaNKH,MaChucNang")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nguoiDung).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaChucNang = new SelectList(db.ChucNangs, "MaChucNang", "TenChucNang", nguoiDung.MaChucNang);
            ViewBag.MaNKH = new SelectList(db.NhaKhoaHocs, "MaNKH", "MaNKHHoSo", nguoiDung.MaNKH);
            return View(nguoiDung);
        }

        // GET: AdminNguoiDung/Delete/5
        public async Task<ActionResult> Delete(string id)
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
        public async Task<ActionResult> DeleteConfirmed(string id)
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
