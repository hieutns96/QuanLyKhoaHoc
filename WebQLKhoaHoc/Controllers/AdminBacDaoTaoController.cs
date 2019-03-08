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
    public class AdminBacDaoTaoController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminBacDaoTao
        public async Task<ActionResult> Index()
        {
            return View(await db.BacDaoTaos.ToListAsync());
        }

        // GET: AdminBacDaoTao/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BacDaoTao bacDaoTao = await db.BacDaoTaos.FindAsync(id);
            if (bacDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(bacDaoTao);
        }

        // GET: AdminBacDaoTao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminBacDaoTao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaBacDT,TenBacDT")] BacDaoTao bacDaoTao)
        {
            if (ModelState.IsValid)
            {
                db.BacDaoTaos.Add(bacDaoTao);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bacDaoTao);
        }

        // GET: AdminBacDaoTao/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BacDaoTao bacDaoTao = await db.BacDaoTaos.FindAsync(id);
            if (bacDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(bacDaoTao);
        }

        // POST: AdminBacDaoTao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaBacDT,TenBacDT")] BacDaoTao bacDaoTao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bacDaoTao).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bacDaoTao);
        }

        // GET: AdminBacDaoTao/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BacDaoTao bacDaoTao = await db.BacDaoTaos.FindAsync(id);
            if (bacDaoTao == null)
            {
                return HttpNotFound();
            }
            return View(bacDaoTao);
        }

        // POST: AdminBacDaoTao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BacDaoTao bacDaoTao = await db.BacDaoTaos.FindAsync(id);
            db.BacDaoTaos.Remove(bacDaoTao);
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
