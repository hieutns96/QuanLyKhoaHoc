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
    public class AdminQuaTrinhDaoTaoController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminQuaTrinhDaoTao
        public async Task<ActionResult> Index()
        {
            var quaTrinhDaoTaos = db.QuaTrinhDaoTaos.Include(q => q.BacDaoTao).Include(q => q.NganhDaoTao).Include(q => q.NhaKhoaHoc);
            return View(await quaTrinhDaoTaos.ToListAsync());
        }

        // GET: AdminQuaTrinhDaoTao/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaTrinhDaoTao quaTrinhDaoTao = await db.QuaTrinhDaoTaos.FindAsync(id);
            if (quaTrinhDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(quaTrinhDaoTao);
        }

        // GET: AdminQuaTrinhDaoTao/Create
        public ActionResult Create(int? manhakhoahoc)
        {   
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View();
        }

        // POST: AdminQuaTrinhDaoTao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaQT,MaNKH,ThoiGianBD,ThoiGianKT,MaBacDT,CoSoDaoTao,MaNganh,NamTotNghiep")] QuaTrinhDaoTao quaTrinhDaoTao,int? manhakhoahoc)
        {
            if (ModelState.IsValid)
            {
                db.QuaTrinhDaoTaos.Add(quaTrinhDaoTao);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit","AdminNhaKhoaHoc",new { id = manhakhoahoc });
            }

            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(quaTrinhDaoTao);
        }

        // GET: AdminQuaTrinhDaoTao/Edit/5
        public async Task<ActionResult> Edit(int? id,int manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaTrinhDaoTao quaTrinhDaoTao = await db.QuaTrinhDaoTaos.FindAsync(id);
            if (quaTrinhDaoTao == null)
            {
                return HttpNotFound();
            }

            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(quaTrinhDaoTao);
        }

        // POST: AdminQuaTrinhDaoTao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaQT,MaNKH,ThoiGianBD,ThoiGianKT,MaBacDT,CoSoDaoTao,MaNganh,NamTotNghiep")] QuaTrinhDaoTao quaTrinhDaoTao,int manhakhoahoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quaTrinhDaoTao).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "AdminNhaKhoaHoc", new { id = manhakhoahoc });
            }
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View(quaTrinhDaoTao);
        }

        // GET: AdminQuaTrinhDaoTao/Delete/5
        public async Task<ActionResult> Delete(int? id,int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaTrinhDaoTao quaTrinhDaoTao = await db.QuaTrinhDaoTaos.FindAsync(id);
            if (quaTrinhDaoTao == null)
            {
                return HttpNotFound();
            }
            db.QuaTrinhDaoTaos.Remove(quaTrinhDaoTao);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "AdminNhaKhoaHoc", new { id = manhakhoahoc });          
        }

        // POST: AdminQuaTrinhDaoTao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            QuaTrinhDaoTao quaTrinhDaoTao = await db.QuaTrinhDaoTaos.FindAsync(id);
            db.QuaTrinhDaoTaos.Remove(quaTrinhDaoTao);
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
