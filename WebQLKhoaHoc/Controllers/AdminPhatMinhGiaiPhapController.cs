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
using WebQLKhoaHoc.Models;
using System.Data.Entity.Migrations;
using System.IO;
using System.Text.RegularExpressions;

namespace WebQLKhoaHoc.Controllers
{
    [CustomizeAuthorize(Roles = "1")]
    public class AdminPhatMinhGiaiPhapController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();

        // GET: AdminPhatMinhGiaiPhap
        public async Task<ActionResult> Index()
        {
            var phatMinhGiaiPhaps = db.PhatMinhGiaiPhaps;
            return View(await phatMinhGiaiPhaps.ToListAsync());
        }

        // GET: AdminPhatMinhGiaiPhap/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhatMinhGiaiPhap phatMinhGiaiPhap = await db.PhatMinhGiaiPhaps.FindAsync(id);
            if (phatMinhGiaiPhap == null)
            {
                return HttpNotFound();
            }
            return View(phatMinhGiaiPhap);
        }

        // GET: AdminPhatMinhGiaiPhap/Create
        public ActionResult Create()
        {
            var lsnkh = db.NhaKhoaHocs.Select(p => new
            {
                MaNKH = p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            });
            ViewBag.MaNKH = new SelectList(lsnkh, "MaNKH", "TenNKH");
            return View();
        }

        // POST: AdminPhatMinhGiaiPhap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaPM,TenPM,SoHieuPM,MotaPM,DoiTuongSuDung,QuocGiaCap,LinkLienKet,NamCongBo,MotaPM")] PhatMinhGiaiPhap phatMinhGiaiPhap, int MaChuSoHuu)
        {
            if (ModelState.IsValid)
            {
                db.PhatMinhGiaiPhaps.Add(phatMinhGiaiPhap);
                await db.SaveChangesAsync();

                DSPhatMinhNKH dSPhatMinhNKH = new DSPhatMinhNKH
                {
                    MaNKH = MaChuSoHuu,
                    MaPM = phatMinhGiaiPhap.MaPM,
                    LaChuSoHuu = true
                };

                db.DSPhatMinhNKHs.Add(dSPhatMinhNKH);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(phatMinhGiaiPhap);
        }

        // GET: AdminPhatMinhGiaiPhap/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhatMinhGiaiPhap phatMinhGiaiPhap = await db.PhatMinhGiaiPhaps.FindAsync(id);
            if (phatMinhGiaiPhap == null)
            {
                return HttpNotFound();
            }
            return View(phatMinhGiaiPhap);
        }

        // POST: AdminPhatMinhGiaiPhap/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaPM,TenPM,SoHieuPM,MotaPM,DoiTuongSuDung,QuocGiaCap,LinkLienKet,NamCongBo,MotaPM")] PhatMinhGiaiPhap phatMinhGiaiPhap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phatMinhGiaiPhap).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(phatMinhGiaiPhap);
        }

        // GET: AdminPhatMinhGiaiPhap/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhatMinhGiaiPhap phatMinhGiaiPhap = await db.PhatMinhGiaiPhaps.FindAsync(id);
            if (phatMinhGiaiPhap == null)
            {
                return HttpNotFound();
            }
            return View(phatMinhGiaiPhap);
        }

        // POST: AdminPhatMinhGiaiPhap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PhatMinhGiaiPhap phatMinhGiaiPhap = await db.PhatMinhGiaiPhaps.FindAsync(id);
            db.PhatMinhGiaiPhaps.Remove(phatMinhGiaiPhap);

            List<DSPhatMinhNKH> dsPM_NKH = await db.DSPhatMinhNKHs.Where(p => p.MaPM == id).ToListAsync();
            foreach (var tacgia in dsPM_NKH)
            {
                db.DSPhatMinhNKHs.Remove(tacgia);
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DanhSachNguoiThamGiaPhatMinh()
        {
            var dsPhatMinh_NKH = db.DSPhatMinhNKHs.Include(d => d.NhaKhoaHoc).Include(d => d.PhatMinhGiaiPhap);
            return View(await dsPhatMinh_NKH.ToListAsync());
        }

        /*create danh sach tac gia*/
        public ActionResult CreateDanhSachNguoiThamGiaPhatMinh(int? id)
        {

            var dsnguoidathamgia = db.DSPhatMinhNKHs.Where(p => p.MaPM == id).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.MaPM = id;
            ViewBag.TenPM = db.PhatMinhGiaiPhaps.Find(id).TenPM;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDanhSachNguoiThamGiaPhatMinh([Bind(Include = "MaPM,MaNKH,LaChuSoHuu")] DSPhatMinhNKH dSPhatMinhNKH)
        {
            if (ModelState.IsValid)
            {
                db.DSPhatMinhNKHs.Add(dSPhatMinhNKH);
                await db.SaveChangesAsync();
                return RedirectToAction("DanhSachNguoiThamGiaPhatMinh");
            }


            var dsnguoidathamgia = db.DSPhatMinhNKHs.Where(p => p.MaPM == dSPhatMinhNKH.MaPM).Select(p => p.MaNKH).ToList();
            var lstAllNKH = db.NhaKhoaHocs.Where(p => !dsnguoidathamgia.Contains(p.MaNKH)).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaNKH = new SelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.MaPM = dSPhatMinhNKH.MaPM;
            ViewBag.TenPM = db.PhatMinhGiaiPhaps.Find(dSPhatMinhNKH.MaPM).TenPM;
            return View(dSPhatMinhNKH);
        }

        /*edit danh sach tac gia*/
        public async Task<ActionResult> EditDanhSachNguoiThamGiaPhatMinh(int? id, int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSPhatMinhNKH dSPhatMinhNKH = await db.DSPhatMinhNKHs.Where(p => p.MaPM == id && p.MaNKH == manhakhoahoc).FirstOrDefaultAsync();
            if (dSPhatMinhNKH == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenPM = db.PhatMinhGiaiPhaps.Find(id).TenPM;
            return View(dSPhatMinhNKH);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDanhSachNguoiThamGiaPhatMinh([Bind(Include = "MaPM,MaNKH,LaChuSoHuu")] DSPhatMinhNKH dSPhatMinhNKH)
        {
            if (ModelState.IsValid)
            {
                DSPhatMinhNKH tacGia = db.DSPhatMinhNKHs.Where(p => p.MaPM == dSPhatMinhNKH.MaPM && p.MaNKH == dSPhatMinhNKH.MaNKH).FirstOrDefault();
                if (tacGia != null)
                {
                    tacGia.LaChuSoHuu = dSPhatMinhNKH.LaChuSoHuu;
                }
                else
                {
                    db.DSPhatMinhNKHs.Add(dSPhatMinhNKH);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("DanhSachNguoiThamGiaPhatMinh");
            }

            ViewBag.TenPM = db.PhatMinhGiaiPhaps.Find(dSPhatMinhNKH.MaPM).TenPM;
            return View(dSPhatMinhNKH);
        }

        /* delete danh sach tac gia*/
        public async Task<ActionResult> DeleteDanhSachNguoiThamGiaPhatMinh(int? id, int? manhakhoahoc)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DSPhatMinhNKH dSPhatMinhNKH = await db.DSPhatMinhNKHs.Where(p => p.MaPM == id && p.MaNKH == manhakhoahoc).FirstOrDefaultAsync();
            if (dSPhatMinhNKH == null)
            {
                return HttpNotFound();
            }
            db.DSPhatMinhNKHs.Remove(dSPhatMinhNKH);
            await db.SaveChangesAsync();

            return RedirectToAction("DanhSachNguoiThamGiaPhatMinh");
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<ActionResult> ImportCSVFile(HttpPostedFileBase fileCSV)
        {

            if (fileCSV != null && fileCSV.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileCSV.FileName);
                if (extension == ".csv")
                {
                    string filename = Path.GetFileName(fileCSV.FileName);
                    string path = Path.Combine(Server.MapPath("~/App_Data/uploads/csv"), filename);

                    try
                    {
                        fileCSV.SaveAs(path);
                        AddRecordFromCSV(path);
                    }
                    catch (Exception exception)
                    {
                        TempData["ImportCSVFileError"] = exception.Message;
                    }
                }
                else
                {
                    TempData["ImportCSVFileError"] = "Xin vui lòng nhập file định dạng CSV";
                }
            }
            return RedirectToAction("Index");
        }

        public void AddRecordFromCSV(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                try
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {

                        string[] resultArray;
                        PhatMinhGiaiPhap phatMinhGiaiPhap = new PhatMinhGiaiPhap();

                        Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        resultArray = r.Split(line);
                        string sohieupm = resultArray[0];
                        PhatMinhGiaiPhap checkpm = db.PhatMinhGiaiPhaps.Where(p => p.SoHieuPM == sohieupm).FirstOrDefault();
                        if (checkpm == null)
                        {
                            //sohieupm,tenpm,motapm
                            phatMinhGiaiPhap.SoHieuPM = resultArray[0];
                            phatMinhGiaiPhap.TenPM = resultArray[1];
                            phatMinhGiaiPhap.MotaPM = resultArray[2];
                            db.PhatMinhGiaiPhaps.Add(phatMinhGiaiPhap);
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

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
