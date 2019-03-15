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
    public class AdminNganHangNKHsController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminNganHangNKHs
        public async Task<ActionResult> Index()
        {
            var nganHangNKHs = db.NganHangNKHs.Include(n => n.NganHang).Include(n => n.NhaKhoaHoc);
            return View(await nganHangNKHs.ToListAsync());
        }

        // GET: AdminNganHangNKHs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganHangNKH nganHangNKH = await db.NganHangNKHs.FindAsync(id);
            if (nganHangNKH == null)
            {
                return HttpNotFound();
            }
            return View(nganHangNKH);
        }

        // GET: AdminNganHangNKHs/Create
        public ActionResult Create(int manhakhoahoc)
        {

            ViewBag.MaNH = new SelectList(db.NganHangs, "MaNH", "TenNH");            
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View();
        }

        // POST: AdminNganHangNKHs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaNKH,STKNH,MaNH,ChiNhanhNH,GhiChu")] NganHangNKH nganHangNKH,int? manhakhoahoc)
        {
            if (ModelState.IsValid)
            {
                db.NganHangNKHs.Add(nganHangNKH);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit","AdminNhaKhoaHoc",new { id = manhakhoahoc});
            }

            ViewBag.MaNH = new SelectList(db.NganHangs, "MaNH", "TenNH");
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View();
        }

        // GET: AdminNganHangNKHs/Edit/5
        public async Task<ActionResult> Edit(int? id,int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganHangNKH nganHangNKH = await db.NganHangNKHs.Where(p => p.MaNH == id && p.MaNKH == manhakhoahoc).FirstOrDefaultAsync();
            if (nganHangNKH == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNH = new SelectList(db.NganHangs, "MaNH", "TenNH");
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(nganHangNKH);
        }

        // POST: AdminNganHangNKHs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaNKH,STKNH,MaNH,ChiNhanhNH,GhiChu")] NganHangNKH nganHangNKH,int? manhakhoahoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nganHangNKH).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "AdminNhaKhoaHoc", new { id = manhakhoahoc });
            }
            ViewBag.MaNH = new SelectList(db.NganHangs, "MaNH", "TenNH", nganHangNKH.MaNH);
            ViewBag.MaNKH = new SelectList(db.NhaKhoaHocs, "MaNKH", "MaNKHHoSo", nganHangNKH.MaNKH);
            return View(nganHangNKH);
        }

        // GET: AdminNganHangNKHs/Delete/5
        public async Task<ActionResult> Delete(int? id, int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganHangNKH nganHangNKH = await db.NganHangNKHs.Where(p => p.MaNH == id && p.MaNKH == manhakhoahoc).FirstOrDefaultAsync();
            if (nganHangNKH == null)
            {
                return HttpNotFound();
            }
            db.NganHangNKHs.Remove(nganHangNKH);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "AdminNhaKhoaHoc", new { id = manhakhoahoc });
        }

        // POST: AdminNganHangNKHs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NganHangNKH nganHangNKH = await db.NganHangNKHs.FindAsync(id);
            db.NganHangNKHs.Remove(nganHangNKH);
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
