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
using System.Data.Entity.Migrations;

namespace WebQLKhoaHoc.Controllers
{
    public class AdminDSNguoiThamGiaBaiBaoController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminDSNguoiThamGiaBaiBao
        public async Task<ActionResult> Index()
        {
            var dSNguoiThamGiaBaiBaos = db.DSNguoiThamGiaBaiBaos.Include(d => d.BaiBao).Include(d => d.NhaKhoaHoc);
            return View(await dSNguoiThamGiaBaiBaos.ToListAsync());
        }

        // GET: AdminDSNguoiThamGiaBaiBao/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSNguoiThamGiaBaiBao dSNguoiThamGiaBaiBao = await db.DSNguoiThamGiaBaiBaos.FindAsync(id);
            if (dSNguoiThamGiaBaiBao == null)
            {
                return HttpNotFound();
            }
            return View(dSNguoiThamGiaBaiBao);
        }

        // GET: AdminDSNguoiThamGiaBaiBao/Create
        public ActionResult Create(int? id, int manhakhoahoc)
        {

            var dsnguoidathamgia = db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == id).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.tenbaibao = db.BaiBaos.Find(id).TenBaiBao;
            ViewBag.mabaibao = id;
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View();
        }

        // POST: AdminDSNguoiThamGiaBaiBao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaBaiBao,MaNKH,LaTacGiaChinh")] DSNguoiThamGiaBaiBao dSNguoiThamGiaBaiBao,int? manhakhoahoc)
        {
            if (ModelState.IsValid)
            {
                db.DSNguoiThamGiaBaiBaos.Add(dSNguoiThamGiaBaiBao);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = dSNguoiThamGiaBaiBao.MaBaiBao, manhakhoahoc = manhakhoahoc });
            }
            var dsnguoidathamgia = db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == dSNguoiThamGiaBaiBao.MaBaiBao).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.tenbaibao = db.BaiBaos.Find(dSNguoiThamGiaBaiBao.MaBaiBao).TenBaiBao;
            ViewBag.mabaibao = dSNguoiThamGiaBaiBao.MaBaiBao;
            ViewBag.manhakhoahoc = manhakhoahoc;
            return View();
        }

        // GET: AdminDSNguoiThamGiaBaiBao/Edit/5
        public async Task<ActionResult> Edit(int? id,int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<DSNguoiThamGiaBaiBao> dSNguoiThamGiaBaiBao = await db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == id).ToListAsync();
            if (dSNguoiThamGiaBaiBao == null)
            {
                return HttpNotFound();
            }
            ViewBag.manhakhoahoc = manhakhoahoc;
            ViewBag.mabaibao = id;
            return View(dSNguoiThamGiaBaiBao);
        }

        // POST: AdminDSNguoiThamGiaBaiBao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaBaiBao,MaNKH,LaTacGiaChinh")] List<DSNguoiThamGiaBaiBao> dSNguoiThamGiaBaiBao,int? manhakhoahoc,int? mabaibao)
        {
            if (ModelState.IsValid)
            {
                foreach(var x in dSNguoiThamGiaBaiBao)
                {
                    db.DSNguoiThamGiaBaiBaos.AddOrUpdate(x);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Edit",new { id = mabaibao, manhakhoahoc = manhakhoahoc });
            }
            ViewBag.manhakhoahoc = manhakhoahoc;
            ViewBag.mabaibao = mabaibao;
            return View(dSNguoiThamGiaBaiBao);
        }

        // GET: AdminDSNguoiThamGiaBaiBao/Delete/5
        public async Task<ActionResult> Delete(int? id,int manhakhoahoc,int mankh)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSNguoiThamGiaBaiBao dSNguoiThamGiaBaiBao = await db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == id && p.MaNKH == manhakhoahoc).FirstOrDefaultAsync();
            if (dSNguoiThamGiaBaiBao == null)
            {
                return HttpNotFound();
            }
            ViewBag.manhakhoahoc = manhakhoahoc;
            ViewBag.mabaibao = id;
            ViewBag.mankh = mankh;
            return View(dSNguoiThamGiaBaiBao);
        }

        // POST: AdminDSNguoiThamGiaBaiBao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id,int mankh,int manhakhoahoc)
        {
            DSNguoiThamGiaBaiBao dSNguoiThamGiaBaiBao = await db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == id && p.MaNKH == mankh).FirstOrDefaultAsync();
            db.DSNguoiThamGiaBaiBaos.Remove(dSNguoiThamGiaBaiBao);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = id, manhakhoahoc = manhakhoahoc });
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
