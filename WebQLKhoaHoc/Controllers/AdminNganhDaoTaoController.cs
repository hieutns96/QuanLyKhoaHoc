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
    public class AdminNganhDaoTaoController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminNganhDaoTao
        public async Task<ActionResult> Index()
        {
            return View(await db.NganhDaoTaos.ToListAsync());
        }

        // GET: AdminNganhDaoTao/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhDaoTao nganhDaoTao = await db.NganhDaoTaos.FindAsync(id);
            if (nganhDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(nganhDaoTao);
        }

        // GET: AdminNganhDaoTao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminNganhDaoTao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaNganhDaoTao,TenNganhDaoTao")] NganhDaoTao nganhDaoTao)
        {
            if (ModelState.IsValid)
            {
                db.NganhDaoTaos.Add(nganhDaoTao);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(nganhDaoTao);
        }

        // GET: AdminNganhDaoTao/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhDaoTao nganhDaoTao = await db.NganhDaoTaos.FindAsync(id);
            if (nganhDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(nganhDaoTao);
        }

        // POST: AdminNganhDaoTao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaNganhDaoTao,TenNganhDaoTao")] NganhDaoTao nganhDaoTao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nganhDaoTao).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nganhDaoTao);
        }

        // GET: AdminNganhDaoTao/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NganhDaoTao nganhDaoTao = await db.NganhDaoTaos.FindAsync(id);
            if (nganhDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(nganhDaoTao);
        }

        // POST: AdminNganhDaoTao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NganhDaoTao nganhDaoTao = await db.NganhDaoTaos.FindAsync(id);
            db.NganhDaoTaos.Remove(nganhDaoTao);
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
