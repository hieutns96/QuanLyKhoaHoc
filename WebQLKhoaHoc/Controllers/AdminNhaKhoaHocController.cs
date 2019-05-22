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
using System;
using System.Text.RegularExpressions;

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
            ViewBag.Error = TempData["ImportCSVFileError"] ?? null;
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
        public async Task<ActionResult> Create(HttpPostedFileBase fileUpload, [Bind(Include = "MaNKH,MaNKHHoSo,HoNKH,TenNKH,GioiTinhNKH,NgaySinh,DiaChiLienHe,DienThoai,EmailLienHe,MaHocHam,MaHocVi,MaCNDaoTao,MaDonViQL,AnhDaiDien,AnhCaNhan,MaNgachVienChuc,NoiSinh")] NhaKhoaHoc nhaKhoaHoc)
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
        public async Task<ActionResult> Edit(HttpPostedFileBase fileUpload, [Bind(Include = "MaNKH,MaNKHHoSo,HoNKH,TenNKH,GioiTinhNKH,NgaySinh,DiaChiLienHe,DienThoai,EmailLienHe,MaHocHam,MaHocVi,MaCNDaoTao,MaDonViQL,MaNgachVienChuc,NoiSinh")] NhaKhoaHoc nhaKhoaHoc)
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
            if (nguoiDung != null)
            {
                nguoiDung.MaNKH = null;
                nguoiDung.IsActive = false;
            }
            
           
            db.NhaKhoaHocs.Remove(nhaKhoaHoc);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<ActionResult> ImportCSVFile (HttpPostedFileBase fileCSV)
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
                        NhaKhoaHoc nkh = new NhaKhoaHoc();

                        Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        resultArray = r.Split(line);
                        string mankhhoso = resultArray[0];
                        NhaKhoaHoc checknkh = db.NhaKhoaHocs.Where(p => p.MaNKHHoSo == mankhhoso).FirstOrDefault();
                        if (checknkh == null)
                        {
                            //MaNKHHoSo,HoNKH,TenNKH,GioiTinh,NgaySinh,DiaChi,DienThoai,Email,SoCMND*/
                            nkh.MaNKHHoSo = resultArray[0];
                            nkh.HoNKH = resultArray[1];
                            nkh.TenNKH = resultArray[2];
                            nkh.GioiTinhNKH = resultArray[3];
                            nkh.NgaySinh = (resultArray[4] != "") ? Convert.ToDateTime(resultArray[4]) : DateTime.Now;
                            nkh.DiaChiLienHe = resultArray[5];
                            nkh.DienThoai = resultArray[6];
                            nkh.EmailLienHe = resultArray[7];
                            nkh.SoCMND = resultArray[8];
                            nkh.NoiSinh = resultArray[9];


                            db.NhaKhoaHocs.Add(nkh);
                            db.SaveChanges();

                            string salt = "".GenRandomKey(); //update by Khiet
                            NguoiDung newuser = new NguoiDung
                            {
                                MaNKH = nkh.MaNKH,
                                Usernames = nkh.MaNKHHoSo,
                                Passwords = Encryptor.MD5Hash("12345" + salt), //update by Khiet
                                MaChucNang = 2,
                                RandomKey = salt //update by Khiet
                            };
                            db.NguoiDungs.Add(newuser);
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
