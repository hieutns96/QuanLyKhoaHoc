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
using LinqKit;
using PagedList;
using System.IO;
using System.Data.Entity.Migrations;

namespace WebQLKhoaHoc.Controllers
{
    public class PhatMinhGiaiPhapsController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();
        private QLKHRepository QLKHrepo = new QLKHRepository();
        // GET: PhatMinhGiaiPhaps
        public async Task<ActionResult> Index(int? Page_No, [Bind(Include = "SearchValue, QuocGiaCap, NamCongBo")] PhatMinhGiaiPhapSearchViewModel phatMinh, int? nkhId)
        {
            var pre = PredicateBuilder.True<PhatMinhGiaiPhap>();

            if (nkhId == null)
            {
                if (!String.IsNullOrEmpty(phatMinh.SearchValue))
                {
                    pre = pre.And(p => p.TenPM.ToLower().Contains(phatMinh.SearchValue.ToLower()));
                    
                }
                if (!String.IsNullOrEmpty(phatMinh.QuocGiaCap))
                {
                    pre = pre.And(p => p.QuocGiaCap.ToLower().Contains(phatMinh.QuocGiaCap.ToLower()));
                    
                }
            }
            else
            {
                var maphatminhs = db.DSPhatMinhNKHs.Where(p => p.MaNKH == nkhId).Select(p => p.MaPM).ToList();
                pre = pre.And(p => maphatminhs.Contains(p.MaPM));
                
            }
            
            if (phatMinh.NamCongBo > DateTime.MinValue)
            {
                pre = pre.And(p => p.NamCongBo >= phatMinh.NamCongBo);
                
            }

            ViewBag.SearchValue = phatMinh.SearchValue;
            ViewBag.QuocGiaCap = phatMinh.QuocGiaCap;
            ViewBag.NamCongBo = phatMinh.NamCongBo;

            int No_Of_Page = (Page_No ?? 1);
            var phatminhs = db.PhatMinhGiaiPhaps.Include(d => d.DSPhatMinhNKHs).AsExpandable().Where(pre).OrderBy(p => p.MaPM).Skip((No_Of_Page - 1) * 6).Take(6).ToList();
            decimal totalItem = (decimal)db.PhatMinhGiaiPhaps.Where(pre).OrderBy(p => p.MaPM).Count();
            int totalPage = (int)Math.Ceiling(totalItem / 6);
            ViewBag.TotalItem = totalItem;
            IPagedList<PhatMinhGiaiPhap> pageOrders = new StaticPagedList<PhatMinhGiaiPhap>(phatminhs, No_Of_Page, 1, totalPage);
            return View(pageOrders);
        }

        // GET: PhatMinhGiaiPhaps/Details/5
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
        public async Task<ActionResult> GetAllImage(int? id)
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
            List<ImageViewModel> lstImage = new List<ImageViewModel>
            {
                new ImageViewModel(phatMinhGiaiPhap.AnhChupSanPham1),
                new ImageViewModel(phatMinhGiaiPhap.AnhChupSanPham2),
                new ImageViewModel(phatMinhGiaiPhap.AnhScanGiayChungNhan)
            };
            return View(lstImage);
        }

        // GET: PhatMinhGiaiPhaps/Create
        public ActionResult Create()
        {
            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.DSPhatMinhNKHs = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH");
            return View();
        }

        // POST: PhatMinhGiaiPhaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HttpPostedFileBase anhChup1, HttpPostedFileBase anhChup2, HttpPostedFileBase anhGiayChungNhan, List<string> DSPhatMinhNKH, [Bind(Include = "MaPM,TenPM,SoHieuPM,MotaPM,DoiTuongSuDung,QuocGiaCap,LinkLienKet,NamCongBo,AnhScanGiayChungNhan,AnhChupSanPham1,AnhChupSanPham2")] PhatMinhGiaiPhap phatMinhGiaiPhap)
        {
            if (ModelState.IsValid)
            {

                if (QLKHrepo.HasFile(anhChup1))
                {
                    string mimeType = anhChup1.ContentType;
                    Stream fileStream = anhChup1.InputStream;
                    string fileName = Path.GetFileName(anhChup1.FileName);
                    int fileLength = anhChup1.ContentLength;
                    byte[] fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);
                    phatMinhGiaiPhap.AnhChupSanPham1 = fileData;
                }
                if (QLKHrepo.HasFile(anhChup2))
                {
                    string mimeType = anhChup2.ContentType;
                    Stream fileStream = anhChup2.InputStream;
                    string fileName = Path.GetFileName(anhChup2.FileName);
                    int fileLength = anhChup2.ContentLength;
                    byte[] fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);
                    phatMinhGiaiPhap.AnhChupSanPham2 = fileData;
                }
                if (QLKHrepo.HasFile(anhGiayChungNhan))
                {
                    string mimeType = anhGiayChungNhan.ContentType;
                    Stream fileStream = anhGiayChungNhan.InputStream;
                    string fileName = Path.GetFileName(anhGiayChungNhan.FileName);
                    int fileLength = anhGiayChungNhan.ContentLength;
                    byte[] fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);
                    phatMinhGiaiPhap.AnhScanGiayChungNhan = fileData;
                }
                db.PhatMinhGiaiPhaps.Add(phatMinhGiaiPhap);
                await db.SaveChangesAsync();

                var id = phatMinhGiaiPhap.MaPM;
                UserLoginViewModel user = (UserLoginViewModel)Session["user"];
                db.DSPhatMinhNKHs.Add(new DSPhatMinhNKH
                {
                    LaChuSoHuu = true,
                    MaPM = id,
                    MaNKH = user.MaNKH
                });

                if (DSPhatMinhNKH != null)
                {
                    List<DSPhatMinhNKH> ds = new List<DSPhatMinhNKH>();
                    foreach (var mankh in DSPhatMinhNKH)
                    {
                        ds.Add(new DSPhatMinhNKH
                        {
                            LaChuSoHuu = false,
                            MaPM = id,
                            MaNKH = Int32.Parse(mankh)
                        });
                    }
                    db.DSPhatMinhNKHs.AddRange(ds);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();
            ViewBag.DSPhatMinhNKHs = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH");

            return View(phatMinhGiaiPhap);
        }

        // GET: PhatMinhGiaiPhaps/Edit/5
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
            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            var lstNKH = db.NhaKhoaHocs.Where(p => p.DSPhatMinhNKHs.Any(d => d.MaPM == phatMinhGiaiPhap.MaPM && d.LaChuSoHuu == false)).Select(p => p.MaNKH).ToList();
            
            ViewBag.DSPhatMinhNKH = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH", lstNKH);

            return View(phatMinhGiaiPhap);
        }

        // POST: PhatMinhGiaiPhaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HttpPostedFileBase anhChup1, HttpPostedFileBase anhChup2, HttpPostedFileBase anhGiayChungNhan, List<string> DSPhatMinhNKH, [Bind(Include = "MaPM,TenPM,SoHieuPM,MotaPM,DoiTuongSuDung,QuocGiaCap,LinkLienKet,NamCongBo,AnhScanGiayChungNhan,AnhChupSanPham1,AnhChupSanPham2")] PhatMinhGiaiPhap phatMinhGiaiPhap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phatMinhGiaiPhap).State = EntityState.Modified;
                if (QLKHrepo.HasFile(anhChup1))
                {
                    string mimeType = anhChup1.ContentType;
                    Stream fileStream = anhChup1.InputStream;
                    string fileName = Path.GetFileName(anhChup1.FileName);
                    int fileLength = anhChup1.ContentLength;
                    byte[] fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);
                    phatMinhGiaiPhap.AnhChupSanPham1 = fileData;
                }
                if (QLKHrepo.HasFile(anhChup2))
                {
                    string mimeType = anhChup2.ContentType;
                    Stream fileStream = anhChup2.InputStream;
                    string fileName = Path.GetFileName(anhChup2.FileName);
                    int fileLength = anhChup2.ContentLength;
                    byte[] fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);
                    phatMinhGiaiPhap.AnhChupSanPham2 = fileData;
                }
                if (QLKHrepo.HasFile(anhGiayChungNhan))
                {
                    string mimeType = anhGiayChungNhan.ContentType;
                    Stream fileStream = anhGiayChungNhan.InputStream;
                    string fileName = Path.GetFileName(anhGiayChungNhan.FileName);
                    int fileLength = anhGiayChungNhan.ContentLength;
                    byte[] fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);
                    phatMinhGiaiPhap.AnhScanGiayChungNhan = fileData;
                }
                if (DSPhatMinhNKH != null)
                {
                    foreach (var mankh in DSPhatMinhNKH)
                    {

                        DSPhatMinhNKH dSPhatMinhNKH = new DSPhatMinhNKH
                        {
                            LaChuSoHuu = false,
                            MaPM = phatMinhGiaiPhap.MaPM,
                            MaNKH = Int32.Parse(mankh)
                        };
                        db.DSPhatMinhNKHs.AddOrUpdate(dSPhatMinhNKH);
                    }
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            var lstNKH = db.NhaKhoaHocs.Where(p => p.DSPhatMinhNKHs.Any(d => d.MaPM == phatMinhGiaiPhap.MaPM && d.LaChuSoHuu == false)).Select(p => p.MaNKH).ToList();

            ViewBag.DSPhatMinhNKH = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH", lstNKH);
            return View(phatMinhGiaiPhap);
        }

      
        // POST: PhatMinhGiaiPhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PhatMinhGiaiPhap phatMinhGiaiPhap = await db.PhatMinhGiaiPhaps.FindAsync(id);
            db.PhatMinhGiaiPhaps.Remove(phatMinhGiaiPhap);
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
