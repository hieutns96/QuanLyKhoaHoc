using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Text;
using WebQLKhoaHoc.Models;

namespace WebQLKhoaHoc.Controllers
{

    [CustomizeAuthorize(Roles = "1,2")]
    public class AdminNhaKhoaHocController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();
        private QLKHRepository repo = new QLKHRepository();

        // GET: AdminNhaKhoaHoc
        public async Task<ActionResult> Index()
        {
            var nhaKhoaHocs = db.NhaKhoaHocs.Include(n => n.ChuyenNganh).Include(n => n.DonViQL).Include(n => n.HocHam).Include(n => n.HocVi).Include(n => n.NgachVienChuc).ToList();
            var lstNKH = new List<NhaKhoaHocViewModel>();
            for (int i = 0; i < nhaKhoaHocs.Count; i++)
            {
                NhaKhoaHocViewModel nkh = NhaKhoaHocViewModel.Mapping(nhaKhoaHocs[i]);
                lstNKH.Add(nkh);
            }
            return View(lstNKH);
        }

        // GET: AdminNhaKhoaHoc/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaKhoaHoc nhaKhoaHoc = await db.NhaKhoaHocs.FindAsync(id);
            if (nhaKhoaHoc == null)
            {
                return HttpNotFound();
            }
            return View(nhaKhoaHoc);
        }

        // GET: AdminNhaKhoaHoc/Create

        public ActionResult Create()
        {
            ViewBag.MaCNDaoTao = new SelectList(db.ChuyenNganhs, "MaChuyenNganh", "TenChuyenNganh");
            ViewBag.MaDonViQL = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI");
            ViewBag.MaHocHam = new SelectList(db.HocHams, "MaHocHam", "TenVietTat");
            ViewBag.MaHocVi = new SelectList(db.HocVis, "MaHocVi", "TenVietTat");
            ViewBag.MaNgachVienChuc = new SelectList(db.NgachVienChucs, "MaNgach", "TenNgach");

            return View();
        }

        // POST: AdminNhaKhoaHoc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HttpPostedFileBase fileUpload, [Bind(Include = "MaNKH,MaNKHHoSo,HoNKH,TenNKH,GioiTinhNKH,NgaySinh,DiaChiLienHe,DienThoai,EmailLienHe,MaHocHam,MaHocVi,MaCNDaoTao,MaDonViQL,AnhDaiDien,AnhCaNhan,MaNgachVienChuc")] NhaKhoaHoc nhaKhoaHoc)
        {

            if (ModelState.IsValid)
            {

                if (repo.HasFile(fileUpload))
                {
                    string mimeType = fileUpload.ContentType;
                    Stream fileStream = fileUpload.InputStream;
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    int fileLength = fileUpload.ContentLength;
                    byte[] fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);
                    nhaKhoaHoc.AnhCaNhan = fileData;

                }

                db.NhaKhoaHocs.Add(nhaKhoaHoc);
                await db.SaveChangesAsync();

                string salt = "".GenRandomKey(); //update by Khiet
                NguoiDung newuser = new NguoiDung
                {
                    MaNKH = nhaKhoaHoc.MaNKH,
                    Usernames = nhaKhoaHoc.MaNKHHoSo,
                    Passwords = Encryptor.MD5Hash("12345" + salt), //update by Khiet
                    MaChucNang = 2,
                    RandomKey = salt //update by Khiet
                };
                db.NguoiDungs.Add(newuser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            ViewBag.MaCNDaoTao = new SelectList(db.ChuyenNganhs, "MaChuyenNganh", "TenChuyenNganh", nhaKhoaHoc.MaCNDaoTao);
            ViewBag.MaDonViQL = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI", nhaKhoaHoc.MaDonViQL);
            ViewBag.MaHocHam = new SelectList(db.HocHams, "MaHocHam", "TenVietTat", nhaKhoaHoc.MaHocHam);
            ViewBag.MaHocVi = new SelectList(db.HocVis, "MaHocVi", "TenVietTat", nhaKhoaHoc.MaHocVi);
            ViewBag.MaNgachVienChuc = new SelectList(db.NgachVienChucs, "MaNgach", "TenNgach", nhaKhoaHoc.MaNgachVienChuc);
            return View(nhaKhoaHoc);
        }

        // GET: AdminNhaKhoaHoc/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaKhoaHoc nkh = await db.NhaKhoaHocs.Include(p => p.NganHangNKH).Where(p => p.MaNKH == id).FirstOrDefaultAsync();
            if (nkh == null)
            {
                return HttpNotFound();
            }
            //NhaKhoaHocViewModel nhaKhoaHoc = NhaKhoaHocViewModel.Mapping(nkh);
            ViewBag.MaCNDaoTao = new SelectList(db.ChuyenNganhs, "MaChuyenNganh", "TenChuyenNganh", nkh.MaCNDaoTao);
            ViewBag.MaDonViQL = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI", nkh.MaDonViQL);
            ViewBag.MaHocHam = new SelectList(db.HocHams, "MaHocHam", "TenHocHam", nkh.MaHocHam);
            ViewBag.MaHocVi = new SelectList(db.HocVis, "MaHocVi", "TenHocVi", nkh.MaHocVi);
            ViewBag.MaNgachVienChuc = new SelectList(db.NgachVienChucs, "MaNgach", "TenNgach", nkh.MaNgachVienChuc);
            return View(nkh);
        }

        // POST: AdminNhaKhoaHoc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HttpPostedFileBase fileUpload, [Bind(Include = "MaNKH,MaNKHHoSo,HoNKH,TenNKH,GioiTinhNKH,NgaySinh,DiaChiLienHe,DienThoai,EmailLienHe,MaHocHam,MaHocVi,MaCNDaoTao,MaDonViQL,MaNgachVienChuc")] NhaKhoaHoc nhaKhoaHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaKhoaHoc).State = EntityState.Modified;
                if (repo.HasFile(fileUpload))
                {
                    string mimeType = fileUpload.ContentType;
                    Stream fileStream = fileUpload.InputStream;
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    int fileLength = fileUpload.ContentLength;
                    byte[] fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);
                    nhaKhoaHoc.AnhCaNhan = fileData;

                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaCNDaoTao = new SelectList(db.ChuyenNganhs, "MaChuyenNganh", "TenChuyenNganh", nhaKhoaHoc.MaCNDaoTao);
            ViewBag.MaDonViQL = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI", nhaKhoaHoc.MaDonViQL);
            ViewBag.MaHocHam = new SelectList(db.HocHams, "MaHocHam", "TenVietTat", nhaKhoaHoc.MaHocHam);
            ViewBag.MaHocVi = new SelectList(db.HocVis, "MaHocVi", "TenVietTat", nhaKhoaHoc.MaHocVi);
            ViewBag.MaNgachVienChuc = new SelectList(db.NgachVienChucs, "MaNgach", "TenNgach", nhaKhoaHoc.MaNgachVienChuc);
            return View(nhaKhoaHoc);
        }

        // GET: AdminNhaKhoaHoc/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaKhoaHoc nhaKhoaHoc = await db.NhaKhoaHocs.FindAsync(id);
            if (nhaKhoaHoc == null)
            {
                return HttpNotFound();
            }
            return View(nhaKhoaHoc);
        }

        // POST: AdminNhaKhoaHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NhaKhoaHoc nhaKhoaHoc = await db.NhaKhoaHocs.FindAsync(id);
            NguoiDung nguoiDung = await db.NguoiDungs.Where(p => p.Usernames == nhaKhoaHoc.MaNKHHoSo).FirstOrDefaultAsync();
            nguoiDung.NhaKhoaHoc = null;
            nguoiDung.IsActive = false;
            db.NhaKhoaHocs.Remove(nhaKhoaHoc);
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
