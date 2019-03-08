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
    public class AdminTrinhDoNgoaiNguNKHController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: TrinhDoNgoaiNguNKH
        public async Task<ActionResult> Index()
        {
            return View(await db.TrinhDoNgoaiNgus.ToListAsync());
        }

        // GET: TrinhDoNgoaiNguNKH/Details/5
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

        // GET: TrinhDoNgoaiNguNKH/Create
        public ActionResult Create(int? manhakhoahoc)
        {
            if (manhakhoahoc != null)
            {
                var ngoaingunkh = db.NhaKhoaHocs.Find(manhakhoahoc).TrinhDoNgoaiNgus;
                var trinhdongoaingu = db.TrinhDoNgoaiNgus.Where(p => !ngoaingunkh.Contains(p)).ToList();


                ViewBag.trinhdongoaingu = new SelectList(trinhdongoaingu, "MaTrinhDoNN", "TenTrinhDo");
                ViewBag.manhakhoahoc = manhakhoahoc;
               
            }
            return View();
        }

        // POST: TrinhDoNgoaiNguNKH/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int? manhakhoahoc,int? matrinhdo)
        {
            if (ModelState.IsValid)
            {
                var trinhdo = db.TrinhDoNgoaiNgus.Find(matrinhdo);
                db.TrinhDoNgoaiNgus.Add(trinhdo);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "AdminNhaKhoaHoc", new { id = manhakhoahoc });
            }


            var ngoaingu = db.NhaKhoaHocs.Find(manhakhoahoc).TrinhDoNgoaiNgus;
            var trinhdongoaingu = db.TrinhDoNgoaiNgus.Where(p => !ngoaingu.Contains(p)).ToList();


            ViewBag.trinhdongoaingu = new SelectList(trinhdongoaingu, "MaTrinhDoNN", "TenTrinhDo");
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View();
        }

        // GET: TrinhDoNgoaiNguNKH/Delete/5
        public async Task<ActionResult> Delete(int? id,int manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var nkh = await db.NhaKhoaHocs.FindAsync(manhakhoahoc);
            TrinhDoNgoaiNgu trinhDoNgoaiNgu = nkh.TrinhDoNgoaiNgus.Where(p => p.MaTrinhDoNN == id).FirstOrDefault();
            if (trinhDoNgoaiNgu == null)
            {
                return HttpNotFound();
            }
            nkh.TrinhDoNgoaiNgus.Remove(trinhDoNgoaiNgu);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "AdminNhaKhoaHoc", new { id = manhakhoahoc });
        }

        // POST: TrinhDoNgoaiNguNKH/Delete/5        
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
