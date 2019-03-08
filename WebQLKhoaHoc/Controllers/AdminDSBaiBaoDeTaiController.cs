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
    public class AdminDSBaiBaoDeTaiController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminDSBaiBaoDeTai
        public async Task<ActionResult> Index()
        {
            var dSBaiBaoDeTais = db.DSBaiBaoDeTais.Include(d => d.BaiBao).Include(d => d.DeTai);
            return View(await dSBaiBaoDeTais.ToListAsync());
        }

        // GET: AdminDSBaiBaoDeTai/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSBaiBaoDeTai dSBaiBaoDeTai = await db.DSBaiBaoDeTais.FindAsync(id);
            if (dSBaiBaoDeTai == null)
            {
                return HttpNotFound();
            }
            return View(dSBaiBaoDeTai);
        }

        // GET: AdminDSBaiBaoDeTai/Create
        public ActionResult Create()
        {
            ViewBag.MaBaiBao = new SelectList(db.BaiBaos, "MaBaiBao", "MaISSN");
            ViewBag.MaDeTai = new SelectList(db.DeTais, "MaDeTai", "MaDeTaiHoSo");
            return View();
        }

        // POST: AdminDSBaiBaoDeTai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaDeTai,MaBaiBao,GhiChu")] DSBaiBaoDeTai dSBaiBaoDeTai)
        {
            if (ModelState.IsValid)
            {
                db.DSBaiBaoDeTais.Add(dSBaiBaoDeTai);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaBaiBao = new SelectList(db.BaiBaos, "MaBaiBao", "MaISSN", dSBaiBaoDeTai.MaBaiBao);
            ViewBag.MaDeTai = new SelectList(db.DeTais, "MaDeTai", "MaDeTaiHoSo", dSBaiBaoDeTai.MaDeTai);
            return View(dSBaiBaoDeTai);
        }

        // GET: AdminDSBaiBaoDeTai/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSBaiBaoDeTai dSBaiBaoDeTai = await db.DSBaiBaoDeTais.FindAsync(id);
            if (dSBaiBaoDeTai == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaBaiBao = new SelectList(db.BaiBaos, "MaBaiBao", "MaISSN", dSBaiBaoDeTai.MaBaiBao);
            ViewBag.MaDeTai = new SelectList(db.DeTais, "MaDeTai", "MaDeTaiHoSo", dSBaiBaoDeTai.MaDeTai);
            return View(dSBaiBaoDeTai);
        }

        // POST: AdminDSBaiBaoDeTai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaDeTai,MaBaiBao,GhiChu")] DSBaiBaoDeTai dSBaiBaoDeTai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dSBaiBaoDeTai).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaBaiBao = new SelectList(db.BaiBaos, "MaBaiBao", "MaISSN", dSBaiBaoDeTai.MaBaiBao);
            ViewBag.MaDeTai = new SelectList(db.DeTais, "MaDeTai", "MaDeTaiHoSo", dSBaiBaoDeTai.MaDeTai);
            return View(dSBaiBaoDeTai);
        }

        // GET: AdminDSBaiBaoDeTai/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSBaiBaoDeTai dSBaiBaoDeTai = await db.DSBaiBaoDeTais.FindAsync(id);
            if (dSBaiBaoDeTai == null)
            {
                return HttpNotFound();
            }
            return View(dSBaiBaoDeTai);
        }

        // POST: AdminDSBaiBaoDeTai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DSBaiBaoDeTai dSBaiBaoDeTai = await db.DSBaiBaoDeTais.FindAsync(id);
            db.DSBaiBaoDeTais.Remove(dSBaiBaoDeTai);
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
