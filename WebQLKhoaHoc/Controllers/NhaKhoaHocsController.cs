using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebQLKhoaHoc;
using WebQLKhoaHoc.Models;
using PagedList.Mvc;
using PagedList;
using System.Text;
using System.IO;
using System.Data.Entity.Migrations;
using WebGrease.Css.Extensions;
using Newtonsoft.Json;
using LinqKit;

namespace WebQLKhoaHoc.Controllers
{
    public class NhaKhoaHocsController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();
        private QLKHRepository repo = new QLKHRepository();

        // GET: NhaKhoaHocs
        public async Task<ActionResult> Index(int? Page_No, [Bind(Include = "MaDonVi,MaNgach,MaHocHam,MaHocVi,MaCNDaoTao,SearchValue")] NhaKhoaHocSearchViewModel nhaKhoaHoc)
        {

            ViewBag.MaCNDaoTao = new SelectList(db.ChuyenNganhs.ToList(), "MaChuyenNganh", "TenChuyenNganh");
            ViewBag.MaDonViQL = new SelectList(db.DonViQLs.ToList(), "MaDonVi", "TenDonVI");
            ViewBag.MaHocHam = new SelectList(db.HocHams.ToList(), "MaHocHam", "TenHocHam");
            ViewBag.MaHocVi = new SelectList(db.HocVis.ToList(), "MaHocVi", "TenHocVi");
            ViewBag.MaNgachVienChuc = new SelectList(db.NgachVienChucs.ToList(), "MaNgach", "TenNgach");

            var pre = PredicateBuilder.True<NhaKhoaHoc>();

            //var nhaKhoaHocs = db.NhaKhoaHocs.Include(n => n.ChuyenNganh).Include(n => n.DonViQL).Include(n => n.HocHam).Include(n => n.HocVi).Include(n => n.NgachVienChuc).ToList();

            if (!String.IsNullOrEmpty(nhaKhoaHoc.MaCNDaoTao))
            {
                pre = pre.And(p => p.MaCNDaoTao.ToString() == nhaKhoaHoc.MaCNDaoTao);
                //nhaKhoaHocs = nhaKhoaHocs.Where(p => p.MaCNDaoTao.ToString() == nhaKhoaHoc.MaCNDaoTao).ToList();
            }
            if (!String.IsNullOrEmpty(nhaKhoaHoc.MaDonVi))
            {
                pre = pre.And(p => p.MaDonViQL.ToString() == nhaKhoaHoc.MaDonVi);
                //nhaKhoaHocs = nhaKhoaHocs.Where(p => p.MaDonViQL.ToString() == nhaKhoaHoc.MaDonVi).ToList();
            }
            if (!String.IsNullOrEmpty(nhaKhoaHoc.MaHocHam))
            {
                pre = pre.And(p => p.MaHocHam.ToString() == nhaKhoaHoc.MaHocHam);
                //nhaKhoaHocs = nhaKhoaHocs.Where(p => p.MaHocHam.ToString() == nhaKhoaHoc.MaHocHam).ToList();
            }
            if (!String.IsNullOrEmpty(nhaKhoaHoc.MaHocVi))
            {
                pre = pre.And(p => p.MaHocVi.ToString() == nhaKhoaHoc.MaHocVi);
                //nhaKhoaHocs = nhaKhoaHocs.Where(p => p.MaHocVi.ToString() == nhaKhoaHoc.MaHocVi).ToList();
            }
            if (!String.IsNullOrEmpty(nhaKhoaHoc.MaNgach))
            {
                pre = pre.And(p => p.MaNgachVienChuc.ToString() == nhaKhoaHoc.MaNgach);
                //nhaKhoaHocs = nhaKhoaHocs.Where(p => p.MaNgachVienChuc.ToString() == nhaKhoaHoc.MaNgach).ToList();
            }
            if (!String.IsNullOrEmpty(nhaKhoaHoc.SearchValue))
            {
                pre = pre.And(p => (p.HoNKH + " " + p.TenNKH).ToLower().Contains(nhaKhoaHoc.SearchValue.ToLower()));
                //nhaKhoaHocs = nhaKhoaHocs.Where(p => (p.HoNKH+ " " +p.TenNKH).ToLower().Contains(nhaKhoaHoc.SearchValue.ToLower())).ToList();
            }
            ViewBag.SearchValue = nhaKhoaHoc.SearchValue;
            
            int No_Of_Page = (Page_No ?? 1);
            var nhaKhoaHocs = db.NhaKhoaHocs.Include(n => n.ChuyenNganh).Include(n => n.DonViQL).Include(n => n.HocHam).Include(n => n.HocVi).Include(n => n.NgachVienChuc).AsExpandable().Where(pre).OrderBy(p => p.MaNKH).Skip(No_Of_Page-1).Take(6).ToList(); 

            var lstNKH = new List<NhaKhoaHocViewModel>();
            for (int i = 0; i < nhaKhoaHocs.Count; i++)
            {
                NhaKhoaHocViewModel nkh = NhaKhoaHocViewModel.Mapping(nhaKhoaHocs[i]);
               lstNKH.Add(nkh);
            }

            
            int totalPage = (int)Math.Ceiling( (decimal)db.NhaKhoaHocs.Count() / 6);
            IPagedList<NhaKhoaHocViewModel> pageOrders = new StaticPagedList<NhaKhoaHocViewModel>(lstNKH, No_Of_Page, 1, totalPage);

            return View(pageOrders);
        }

        // GET: NhaKhoaHocs/Details/5
        public async Task<ActionResult> Details(int id)
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
            ViewBag.MaCNDaoTao = new SelectList(db.ChuyenNganhs, "MaChuyenNganh", "TenChuyenNganh");
            ViewBag.MaDonViQL = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI");
            ViewBag.MaHocHam = new SelectList(db.HocHams, "MaHocHam", "TenHocHam");
            ViewBag.MaHocVi = new SelectList(db.HocVis, "MaHocVi", "TenHocVi");
            ViewBag.MaNgachVienChuc = new SelectList(db.NgachVienChucs, "MaNgach", "TenNgach");
            NhaKhoaHocViewModel nkh = NhaKhoaHocViewModel.Mapping(nhaKhoaHoc);
            return View(nkh);
        }

        // GET: NhaKhoaHocs/Create
        public ActionResult Create()
        {
           
            ViewBag.MaCNDT = new SelectList(db.ChuyenNganhs, "MaChuyenNganh", "TenChuyenNganh");
            ViewBag.MaDVQL = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI");
            ViewBag.MaHH = new SelectList(db.HocHams, "MaHocHam", "TenHocHam");
            ViewBag.MaHV = new SelectList(db.HocVis, "MaHocVi", "TenHocVi");
            ViewBag.MaNgachVC = new SelectList(db.NgachVienChucs, "MaNgach", "TenNgach");

            var lstAllTrinhDoNN = db.TrinhDoNgoaiNgus.Select(p => new
            {
                p.MaTrinhDoNN,
                TenTD = p.TenTrinhDo
            }).ToList();

           
            ViewBag.DSTrinhDoNgoaiNgu = new MultiSelectList(lstAllTrinhDoNN, "MaTrinhDoNN", "TenTD");

            var lstAllLinhVucNC = db.LinhVucs.Select(p => new
            {
                p.MaLinhVuc,
                TenLV = p.TenLinhVuc
            });
           
            ViewBag.DSLinhVucNghienCuu = new MultiSelectList(lstAllLinhVucNC, "MaLinhVuc", "TenLV");

            var lstAllChuyenMonGD = db.ChuyenMons.Select(p => new
            {
                p.MaChuyenMon,
                TenCM = p.TenChuyenMon
            });
           
            ViewBag.DSChuyenMonGiangDay = new MultiSelectList(lstAllChuyenMonGD, "MaChuyenMon", "TenCM");


            return View();
        }
        
     
        // POST: NhaKhoaHocs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaNKH,MaNKHHoSo,HoNKH,TenNKH,GioiTinhNKH,NgaySinh,DiaChiLienHe,DienThoai,EmailLienHe,MaHocHam,MaHocVi,MaCNDaoTao,MaDonViQL,AnhDaiDien,MaNgachVienChuc")] NhaKhoaHoc nhaKhoaHoc)
        {
            if (ModelState.IsValid)
            {
                db.NhaKhoaHocs.Add(nhaKhoaHoc);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaCNDaoTao = new SelectList(db.ChuyenNganhs, "MaChuyenNganh", "TenChuyenNganh", nhaKhoaHoc.MaCNDaoTao);
            ViewBag.MaDonViQL = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI", nhaKhoaHoc.MaDonViQL);
            ViewBag.MaHocHam = new SelectList(db.HocHams, "MaHocHam", "TenHocHam", nhaKhoaHoc.MaHocHam);
            ViewBag.MaHocVi = new SelectList(db.HocVis, "MaHocVi", "TenHocVi", nhaKhoaHoc.MaHocVi);
            ViewBag.MaNgachVienChuc = new SelectList(db.NgachVienChucs, "MaNgach", "TenNgach", nhaKhoaHoc.MaNgachVienChuc);

            return View(nhaKhoaHoc);
        }

        // GET: NhaKhoaHocs/Edit/5
        public async Task<ActionResult> Edit(int? id)
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

            ViewBag.MaCNDT = new SelectList(db.ChuyenNganhs, "MaChuyenNganh", "TenChuyenNganh", nhaKhoaHoc.MaCNDaoTao);
            ViewBag.MaDVQL = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI", nhaKhoaHoc.MaDonViQL);
            ViewBag.MaHH = new SelectList(db.HocHams, "MaHocHam", "TenHocHam",nhaKhoaHoc.MaHocHam);
            ViewBag.MaHV = new SelectList(db.HocVis, "MaHocVi", "TenHocVi", nhaKhoaHoc.MaHocVi);
            ViewBag.MaNgachVC = new SelectList(db.NgachVienChucs, "MaNgach", "TenNgach", nhaKhoaHoc.MaNgachVienChuc);

            var lstAllTrinhDoNN = db.TrinhDoNgoaiNgus.Select(p => new
            {
                p.MaTrinhDoNN,
                TenTD = p.TenTrinhDo
            }).ToList();
          
            var lstTrinhDoNN = db.TrinhDoNgoaiNgus.Where(p => p.NhaKhoaHocs.Select(t => t.MaNKH).Contains(nhaKhoaHoc.MaNKH)).Select(p => p.MaTrinhDoNN).ToList();
            ViewBag.DSTrinhDoNgoaiNgu = new MultiSelectList(lstAllTrinhDoNN, "MaTrinhDoNN", "TenTD", lstTrinhDoNN);

            var lstAllLinhVucNC = db.LinhVucs.Select(p => new
            {
                p.MaLinhVuc,
                TenLV = p.TenLinhVuc
            });
            var lstLinhVucNC = db.LinhVucs.Where(p => p.NhaKhoaHocs.Select(t => t.MaNKH).Contains(nhaKhoaHoc.MaNKH)).Select(t => t.MaLinhVuc)
                .ToList();
            ViewBag.DSLinhVucNghienCuu = new MultiSelectList(lstAllLinhVucNC, "MaLinhVuc", "TenLV", lstLinhVucNC);

            var lstAllChuyenMonGD = db.ChuyenMons.Select(p => new
            {
                p.MaChuyenMon,
                TenCM = p.TenChuyenMon
            });
            var lstChuyenMonGD = db.ChuyenMonNKHs.Where(p => p.MaNKH == nhaKhoaHoc.MaNKH).Select(p => p.MaChuyenMon)
                .ToList();
            ViewBag.DSChuyenMonGiangDay = new MultiSelectList(lstAllChuyenMonGD, "MaChuyenMon", "TenCM", lstChuyenMonGD);
            NhaKhoaHocViewModel nkh = NhaKhoaHocViewModel.Mapping(nhaKhoaHoc);
            return View(nkh);
        }

        // POST: NhaKhoaHocs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HttpPostedFileBase fileUpload, List<int> DSTrinhDoNN,List<int> DSLinhVucNC, List<int> DSChuyenMonGD, [Bind(Include = "MaNKH,MaNKHHoSo,HoNKH,TenNKH,GioiTinhNKH,NgaySinh,DiaChiLienHe,DienThoai,EmailLienHe,MaHocHam,MaHocVi,MaCNDaoTao,MaDonViQL,AnhDaiDien,MaNgachVienChuc")] NhaKhoaHoc nhaKhoaHoc)
        {
           
            if (ModelState.IsValid)
            {
                // upload image
                var nhakh = db.NhaKhoaHocs.Where(p => p.MaNKH == nhaKhoaHoc.MaNKH).Include(p => p.LinhVucs).Include(p=>p.TrinhDoNgoaiNgus).FirstOrDefault();

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

                db.NhaKhoaHocs.AddOrUpdate(nhaKhoaHoc);
                

                if (DSLinhVucNC != null) {
                    
                    var deletedlinhvuc = nhakh.LinhVucs.Where(p => !DSLinhVucNC.Contains(p.MaLinhVuc)).ToList();
                    var addedlinhvuc = DSLinhVucNC.Except(nhakh.LinhVucs.Select(p => p.MaLinhVuc)).ToList();
                    var addlinhvuc = db.LinhVucs.Where(p => addedlinhvuc.Contains(p.MaLinhVuc)).ToList();
                   
                    foreach (var x in deletedlinhvuc)
                    {
                        nhakh.LinhVucs.Remove(x);
                    }
                   
                    foreach(var x in addlinhvuc)
                    {
                        nhakh.LinhVucs.Add(x);
                    }
                }
                else
                {
                    foreach(var x in nhakh.LinhVucs)
                    {
                        nhakh.LinhVucs.Remove(x);
                    }
                }

                if (DSTrinhDoNN != null) {
                    var deletednn = nhakh.TrinhDoNgoaiNgus.Where(p => !DSTrinhDoNN.Contains(p.MaTrinhDoNN)).ToList();
                    var addednn = DSTrinhDoNN.Except(nhakh.TrinhDoNgoaiNgus.Select(p => p.MaTrinhDoNN)).ToList();
                    var addnn = db.TrinhDoNgoaiNgus.Where(p => addednn.Contains(p.MaTrinhDoNN)).ToList();
                    foreach (var x in deletednn)
                    {
                        nhakh.TrinhDoNgoaiNgus.Remove(x);
                    }
                    foreach (var x in addnn)
                    {
                        nhakh.TrinhDoNgoaiNgus.Add(x);
                    }
                }
                else
                {
                    foreach(var x in nhakh.TrinhDoNgoaiNgus)
                    {
                        nhakh.TrinhDoNgoaiNgus.Remove(x);
                    }
                }                               

              
                if (DSChuyenMonGD != null) {
                    db.ChuyenMonNKHs.Where(p => p.MaNKH == nhaKhoaHoc.MaNKH).ForEach(x => db.ChuyenMonNKHs.Remove(x));
                    foreach (var item in DSChuyenMonGD)
                    {
                        ChuyenMonNKH chuyenmonNKH = new ChuyenMonNKH
                        {
                            MaNKH = nhaKhoaHoc.MaNKH,
                            MaChuyenMon = item,
                            NgayCapNhat = DateTime.Now
                        };
                        db.ChuyenMonNKHs.Add(chuyenmonNKH);
                        db.SaveChanges();
                    }
                }
               

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            ViewBag.MaCNDaoTao = new SelectList(db.ChuyenNganhs, "MaChuyenNganh", "TenChuyenNganh", nhaKhoaHoc.MaCNDaoTao);
            ViewBag.MaDonViQL = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI", nhaKhoaHoc.MaDonViQL);
            ViewBag.MaHocHam = new SelectList(db.HocHams, "MaHocHam", "TenHocHam", nhaKhoaHoc.MaHocHam);
            ViewBag.MaHocVi = new SelectList(db.HocVis, "MaHocVi", "TenHocVi", nhaKhoaHoc.MaHocVi);
            ViewBag.MaNgachVienChuc = new SelectList(db.NgachVienChucs, "MaNgach", "TenNgach", nhaKhoaHoc.MaNgachVienChuc);
            NhaKhoaHocViewModel nkh = NhaKhoaHocViewModel.Mapping(nhaKhoaHoc);
            return View(nkh);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditQuaTrinhDT(List<QuaTrinhDaoTao> quatrinhdaotao, string xoaquatrinhdaotao, string xoaquatrinhdaotaojs, int MaNKH)
        {
            //các MaQT cần xóa trong DB
            List<int> maxoa = JsonConvert.DeserializeObject<List<int>>(xoaquatrinhdaotao);
            //xử lý các dòng chưa có MaQT mà cần xóa
            List<int> maxoajs = JsonConvert.DeserializeObject<List<int>>(xoaquatrinhdaotaojs);
            if (maxoajs != null)
            {
                maxoajs.Sort();
                maxoajs.Reverse();
                foreach (var x in maxoajs)
                {
                    quatrinhdaotao.RemoveAt(x);
                }
            }

            /* track entity để thêm xóa sửa*/
            var quatrinhnhakhoahoc = db.NhaKhoaHocs.Where(p => p.MaNKH == MaNKH).Include(p=>p.QuaTrinhDaoTaos).FirstOrDefault();
            if (maxoa!= null)
            {                           
                foreach (var x in maxoa)
                {
                    var deleteddatao = db.QuaTrinhDaoTaos.Where(p => p.MaQT == x).FirstOrDefault();
                    db.QuaTrinhDaoTaos.Remove(deleteddatao);
                    quatrinhdaotao.RemoveAll(p=>p.MaQT == x);
                    db.SaveChanges();
                }
            }
            foreach(var x in quatrinhdaotao)
            {
                var daotao = db.QuaTrinhDaoTaos.Where(p => p.MaQT == x.MaQT).SingleOrDefault();
                if (daotao != null)
                {
                    db.QuaTrinhDaoTaos.AddOrUpdate(x);              
                }
                else                
                {
                    quatrinhnhakhoahoc.QuaTrinhDaoTaos.Add(x);
                }
                db.SaveChanges();
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = MaNKH });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditQuaTrinhCT(List<QuaTrinhCongTac> quatrinhcongtac, string xoaquatrinhcongtac, string xoaquatrinhcongtacjs, int MaNKH)
        {
            //các MaQT cần xóa trong DB
            List<int> maxoa = JsonConvert.DeserializeObject<List<int>>(xoaquatrinhcongtac);
            //xử lý các dòng chưa có MaQT mà cần xóa
            List<int> maxoajs = JsonConvert.DeserializeObject<List<int>>(xoaquatrinhcongtacjs);
            if (maxoajs != null)
            {
                maxoajs.Sort();
                maxoajs.Reverse();
                foreach (var x in maxoajs)
                {
                    quatrinhcongtac.RemoveAt(x);
                }
            }

            /* track entity để thêm xóa sửa*/
            var quatrinhnhakhoahoc = db.NhaKhoaHocs.Where(p => p.MaNKH == MaNKH).Include(p => p.QuaTrinhCongTacs).FirstOrDefault();
            if (maxoa != null)
            {
                foreach (var x in maxoa)
                {
                    var deletedcongtac = db.QuaTrinhCongTacs.Where(p => p.MaCT == x).FirstOrDefault();
                    db.QuaTrinhCongTacs.Remove(deletedcongtac);
                    quatrinhcongtac.RemoveAll(p => p.MaCT == x);
                    db.SaveChanges();
                }
            }
            foreach (var x in quatrinhcongtac)
            {
                var congtac = db.QuaTrinhCongTacs.Where(p => p.MaCT == x.MaCT).SingleOrDefault();
                if (congtac != null)
                {
                    db.QuaTrinhCongTacs.AddOrUpdate(x);
                }
                else
                {
                    quatrinhnhakhoahoc.QuaTrinhCongTacs.Add(x);
                }
                db.SaveChanges();
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = MaNKH });
        }
            // GET: NhaKhoaHocs/Delete/5
            public async Task<ActionResult> Delete(string id)
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




        // POST: NhaKhoaHocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            NhaKhoaHoc nhaKhoaHoc = await db.NhaKhoaHocs.FindAsync(id);
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