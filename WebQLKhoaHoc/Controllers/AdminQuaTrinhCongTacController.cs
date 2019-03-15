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
    public class AdminQuaTrinhCongTacController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminQuaTrinhCongTac
        public async Task<ActionResult> Index()
        {
            var quaTrinhCongTacs = db.QuaTrinhCongTacs.Include(q => q.NhaKhoaHoc);
            return View(await quaTrinhCongTacs.ToListAsync());
        }

        // GET: AdminQuaTrinhCongTac/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaTrinhCongTac quaTrinhCongTac = await db.QuaTrinhCongTacs.FindAsync(id);
            if (quaTrinhCongTac == null)
            {
                return HttpNotFound();
            }
            return View(quaTrinhCongTac);
        }

        // GET: AdminQuaTrinhCongTac/Create
        public ActionResult Create(int? manhakhoahoc)
        {
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View();
        }

        // POST: AdminQuaTrinhCongTac/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaCT,MaNKH,ThoiGianBD,ThoiGIanKT,TenCoQuan,TenPhongBan,DiaChiCoQuan,TinhTP,ChucVuCT")] QuaTrinhCongTac quaTrinhCongTac,int manhakhoahoc)
        {
            if (ModelState.IsValid)
            {
                db.QuaTrinhCongTacs.Add(quaTrinhCongTac);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "AdminNhaKhoaHoc", new { id = manhakhoahoc });
            }

            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(quaTrinhCongTac);
        }

        // GET: AdminQuaTrinhCongTac/Edit/5
        public async Task<ActionResult> Edit(int? id,int manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaTrinhCongTac quaTrinhCongTac = await db.QuaTrinhCongTacs.FindAsync(id);
            if (quaTrinhCongTac == null)
            {
                return HttpNotFound();
            }

            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(quaTrinhCongTac);
        }

        // POST: AdminQuaTrinhCongTac/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaCT,MaNKH,ThoiGianBD,ThoiGIanKT,TenCoQuan,TenPhongBan,DiaChiCoQuan,TinhTP,ChucVuCT")] QuaTrinhCongTac quaTrinhCongTac,int manhakhoahoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quaTrinhCongTac).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "AdminNhaKhoaHoc", new { id = manhakhoahoc });
            }
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(quaTrinhCongTac);
        }

        // GET: AdminQuaTrinhCongTac/Delete/5
        public async Task<ActionResult> Delete(int? id,int manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaTrinhCongTac quaTrinhCongTac = await db.QuaTrinhCongTacs.FindAsync(id);
            if (quaTrinhCongTac == null)
            {
                return HttpNotFound();
            }
            db.QuaTrinhCongTacs.Remove(quaTrinhCongTac);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "AdminNhaKhoaHoc", new { id = manhakhoahoc });            
        }

        // POST: AdminQuaTrinhCongTac/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id,int manhakhoahoc)
        {
            QuaTrinhCongTac quaTrinhCongTac = await db.QuaTrinhCongTacs.FindAsync(id);
            db.QuaTrinhCongTacs.Remove(quaTrinhCongTac);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "AdminNhaKhoaHoc", new { id = manhakhoahoc });
        }*/

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
