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
    public class AdminChuyenMonNKHController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminChuyenMonNKH
        public async Task<ActionResult> Index()
        {
            var chuyenMonNKHs = db.ChuyenMonNKHs.Include(c => c.ChuyenMon).Include(c => c.NhaKhoaHoc);
            return View(await chuyenMonNKHs.ToListAsync());
        }

        // GET: AdminChuyenMonNKH/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMonNKH chuyenMonNKH = await db.ChuyenMonNKHs.FindAsync(id);
            if (chuyenMonNKH == null)
            {
                return HttpNotFound();
            }
            return View(chuyenMonNKH);
        }

        // GET: AdminChuyenMonNKH/Create
        public ActionResult Create(int manhakhoahoc)
        {
            ViewBag.manhakhoahoc = manhakhoahoc;
            var chuyenmon = db.ChuyenMonNKHs.Where(x => x.MaNKH == manhakhoahoc).Select(p=>p.MaChuyenMon);
            ViewBag.ChuyenMon = new SelectList(db.ChuyenMons.Where(p=> !chuyenmon.Contains(p.MaChuyenMon)),"MaChuyenMon","TenChuyenMon");
            return View();
        }

        // POST: AdminChuyenMonNKH/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaNKH,MaChuyenMon,NgayCapNhat")] ChuyenMonNKH chuyenMonNKH,int manhakhoahoc)
        {
            if (ModelState.IsValid)
            {
                db.ChuyenMonNKHs.Add(chuyenMonNKH);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "AdminNhaKhoahoc", new { id = manhakhoahoc });
            }

            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(chuyenMonNKH);
        }

        // GET: AdminChuyenMonNKH/Edit/5
        public async Task<ActionResult> Edit(int? id, int manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMonNKH chuyenMonNKH = await db.ChuyenMonNKHs.FindAsync(id);
            if (chuyenMonNKH == null)
            {
                return HttpNotFound();
            }
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(chuyenMonNKH);
        }

        // POST: AdminChuyenMonNKH/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaNKH,MaChuyenMon,NgayCapNhat")] ChuyenMonNKH chuyenMonNKH,int manhakhoahoc,int MaChuyenMonMoi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chuyenMonNKH).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Edit","AdminNhaKhoahoc",new { id = manhakhoahoc});
            }
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(chuyenMonNKH);
        }

        // GET: AdminChuyenMonNKH/Delete/5
        public async Task<ActionResult> Delete(int? id,int manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMonNKH chuyenMonNKH = await db.ChuyenMonNKHs.Where(p=>p.MaNKH==manhakhoahoc && p.MaChuyenMon==id).FirstOrDefaultAsync();
            if (chuyenMonNKH == null)
            {
                return HttpNotFound();
            }
            db.ChuyenMonNKHs.Remove(chuyenMonNKH);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "AdminNhaKhoahoc", new { id = manhakhoahoc });
        }

        // POST: AdminChuyenMonNKH/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ChuyenMonNKH chuyenMonNKH = await db.ChuyenMonNKHs.FindAsync(id);
            db.ChuyenMonNKHs.Remove(chuyenMonNKH);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
