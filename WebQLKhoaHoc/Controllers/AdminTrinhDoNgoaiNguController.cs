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
    public class AdminTrinhDoNgoaiNguController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminTrinhDoNgoaiNgu
        public async Task<ActionResult> Index()
        {
            return View(await db.TrinhDoNgoaiNgus.ToListAsync());
        }

        // GET: AdminTrinhDoNgoaiNgu/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoNgoaiNgu trinhDoNgoaiNgu = await db.TrinhDoNgoaiNgus.FindAsync(id);
            if (trinhDoNgoaiNgu == null)
            {
                return HttpNotFound();
            }
            return View(trinhDoNgoaiNgu);
        }

        // GET: AdminTrinhDoNgoaiNgu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminTrinhDoNgoaiNgu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaTrinhDoNN,TenTrinhDo,CapDo")] TrinhDoNgoaiNgu trinhDoNgoaiNgu)
        {
            if (ModelState.IsValid)
            {
                db.TrinhDoNgoaiNgus.Add(trinhDoNgoaiNgu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(trinhDoNgoaiNgu);
        }

        // GET: AdminTrinhDoNgoaiNgu/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoNgoaiNgu trinhDoNgoaiNgu = await db.TrinhDoNgoaiNgus.FindAsync(id);
            if (trinhDoNgoaiNgu == null)
            {
                return HttpNotFound();
            }
            return View(trinhDoNgoaiNgu);
        }

        // POST: AdminTrinhDoNgoaiNgu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaTrinhDoNN,TenTrinhDo,CapDo")] TrinhDoNgoaiNgu trinhDoNgoaiNgu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trinhDoNgoaiNgu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(trinhDoNgoaiNgu);
        }

        // GET: AdminTrinhDoNgoaiNgu/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoNgoaiNgu trinhDoNgoaiNgu = await db.TrinhDoNgoaiNgus.FindAsync(id);
            if (trinhDoNgoaiNgu == null)
            {
                return HttpNotFound();
            }
            return View(trinhDoNgoaiNgu);
        }

        // POST: AdminTrinhDoNgoaiNgu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TrinhDoNgoaiNgu trinhDoNgoaiNgu = await db.TrinhDoNgoaiNgus.FindAsync(id);
            db.TrinhDoNgoaiNgus.Remove(trinhDoNgoaiNgu);
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
