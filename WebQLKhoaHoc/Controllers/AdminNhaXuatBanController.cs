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
    public class AdminNhaXuatBanController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminNhaXuatBan
        public async Task<ActionResult> Index()
        {
            return View(await db.NhaXuatBans.ToListAsync());
        }

        // GET: AdminNhaXuatBan/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaXuatBan nhaXuatBan = await db.NhaXuatBans.FindAsync(id);
            if (nhaXuatBan == null)
            {
                return HttpNotFound();
            }
            return View(nhaXuatBan);
        }

        // GET: AdminNhaXuatBan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminNhaXuatBan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaNXB,TenNXB,DiaChiNXB,DienThoaiNXB,DiaChiWeb")] NhaXuatBan nhaXuatBan)
        {
            if (ModelState.IsValid)
            {
                db.NhaXuatBans.Add(nhaXuatBan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(nhaXuatBan);
        }

        // GET: AdminNhaXuatBan/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaXuatBan nhaXuatBan = await db.NhaXuatBans.FindAsync(id);
            if (nhaXuatBan == null)
            {
                return HttpNotFound();
            }
            return View(nhaXuatBan);
        }

        // POST: AdminNhaXuatBan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaNXB,TenNXB,DiaChiNXB,DienThoaiNXB,DiaChiWeb")] NhaXuatBan nhaXuatBan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaXuatBan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nhaXuatBan);
        }

        // GET: AdminNhaXuatBan/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaXuatBan nhaXuatBan = await db.NhaXuatBans.FindAsync(id);
            if (nhaXuatBan == null)
            {
                return HttpNotFound();
            }
            return View(nhaXuatBan);
        }

        // POST: AdminNhaXuatBan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NhaXuatBan nhaXuatBan = await db.NhaXuatBans.FindAsync(id);
            db.NhaXuatBans.Remove(nhaXuatBan);
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
