using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebQLKhoaHoc.Models;

namespace WebQLKhoaHoc.Controllers
{
    public class HomeController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();
        private QLKHRepository QLKHrepo = new QLKHRepository();
        public ActionResult Index()
        {
            ViewBag.MaDonViQLThucHien = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI");
            ViewBag.MaDanhMuc = new List<SelectListItem>
            {
                new SelectListItem { Text = "Nhà Khoa Học", Value = "0",Selected = true},
                new SelectListItem { Text = "Đề Tài Khoa Học", Value ="1"},
                new SelectListItem { Text = "Bài Báo Khoa Học", Value ="2"},
                new SelectListItem { Text = "Sách Và Giáo Trình", Value ="3"},
            };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search([Bind(Include = "MaDanhMuc,MaDonViQLThucHien,SearchValue")] HomeSearchViewModel home)
        {

           
            ViewBag.SearchValue = home.SearchValue;
            int Size_Of_Page = 6;
            int No_Of_Page = 1;
            switch (home.MaDanhMuc)
            {
                case "1":
                    ViewBag.MaCapDeTai = new SelectList(db.CapDeTais, "MaCapDeTai", "TenCapDeTai");
                    ViewBag.MaDonViQLThucHien = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI");
                    ViewBag.MaLinhVuc = new SelectList(QLKHrepo.GetListMenuLinhVuc(), "Id", "TenLinhVuc");
                    var detais = db.DeTais.Include(d => d.CapDeTai).Include(d => d.LoaiHinhDeTai).Include(d => d.DonViChuTri).Include(d => d.DonViQL).Include(d => d.LinhVuc).Include(d => d.XepLoai).Include(d => d.TinhTrangDeTai).Include(d => d.PhanLoaiSP).ToList();
                    if (!String.IsNullOrEmpty(home.MaDonViQLThucHien) && home.MaDonViQLThucHien != "0")
                    {
                        detais = detais.Where(p => p.MaDonViQLThucHien.ToString() == home.MaDonViQLThucHien).ToList();
                    }
                    if (!String.IsNullOrEmpty(home.SearchValue))
                    {
                        detais = detais.Where(p => p.TenDeTai.ToLower().Contains(home.SearchValue.ToLower())).ToList();
                    }
                    int totalPage1 = (int)Math.Ceiling((decimal)detais.Count() / 6);
                    ViewBag.TotalItem = detais.Count();
                    IPagedList<DeTai> pageOrders1 = new StaticPagedList<DeTai>(detais, No_Of_Page, 1, totalPage1);

                    return View("~/Views/DeTais/Index.cshtml", pageOrders1);
                case "2":
                    ViewBag.MaLinhVuc = new SelectList(QLKHrepo.GetListMenuLinhVuc(), "Id", "TenLinhVuc");
                    ViewBag.MaCapTapChi = new SelectList(db.CapTapChis, "MaCapTapChi", "TenCapTapChi");
                    ViewBag.MaPhanLoaiTapChi = new SelectList(db.PhanLoaiTapChis, "MaLoaiTapChi", "TenLoaiTapChi");
                    ViewBag.MaLoaiTapChi = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Trong Nước", Value = "1"},
                        new SelectListItem { Text = "Ngoài Nước", Value ="0"},
                    };
                    var baibaos = db.BaiBaos.Include(b => b.CapTapChi).Include(b => b.PhanLoaiTapChi).ToList();
                    if (!String.IsNullOrEmpty(home.SearchValue))
                    {
                        baibaos = baibaos.Where(p => p.TenBaiBao.ToLower().Contains(home.SearchValue.ToLower())).ToList();
                    }
                    int totalPage2 = (int)Math.Ceiling((decimal)baibaos.Count() / 6);
                    ViewBag.TotalItem = baibaos.Count();
                    IPagedList<BaiBao> pageOrders2 = new StaticPagedList<BaiBao>(baibaos, No_Of_Page, 1, totalPage2);

                    return View("~/Views/BaiBaos/Index.cshtml", pageOrders2);
                case "3":
                    ViewBag.MaLinhVuc = new SelectList(QLKHrepo.GetListMenuLinhVuc(), "Id", "TenLinhVuc");
                    ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
                    ViewBag.MaLoai = new SelectList(db.PhanLoaiSaches, "MaLoai", "TenLoai");
                    var sachGiaoTrinhs = db.SachGiaoTrinhs.Include(s => s.LinhVuc).Include(s => s.NhaXuatBan).Include(s => s.PhanLoaiSach).ToList();
                    
                    if (!String.IsNullOrEmpty(home.SearchValue))
                    {
                        sachGiaoTrinhs = sachGiaoTrinhs.Where(p => p.TenSach.ToLower().Contains(home.SearchValue.ToLower())).ToList();
                    }
                    int totalPage3 = (int)Math.Ceiling((decimal)sachGiaoTrinhs.Count() / 6);
                    ViewBag.TotalItem = sachGiaoTrinhs.Count();
                    IPagedList<SachGiaoTrinh> pageOrders3 = new StaticPagedList<SachGiaoTrinh>(sachGiaoTrinhs, No_Of_Page, 1, totalPage3);

                    return View("~/Views/SachGiaoTrinhs/Index.cshtml", pageOrders3);
                default:
                    ViewBag.MaCNDaoTao = new SelectList(db.ChuyenNganhs.ToList(), "MaChuyenNganh", "TenChuyenNganh");
                    ViewBag.MaHocHam = new SelectList(db.HocHams.ToList(), "MaHocHam", "TenHocHam");
                    ViewBag.MaHocVi = new SelectList(db.HocVis.ToList(), "MaHocVi", "TenHocVi");
                    ViewBag.MaDonViQL = new SelectList(db.DonViQLs.ToList(), "MaDonVi", "TenDonVI");
                    ViewBag.MaNgachVienChuc = new SelectList(db.NgachVienChucs.ToList(), "MaNgach", "TenNgach");
                    var nhaKhoaHocs = db.NhaKhoaHocs.Include(n => n.ChuyenNganh).Include(n => n.DonViQL).Include(n => n.HocHam).Include(n => n.HocVi).Include(n => n.NgachVienChuc).ToList();
                    if (!String.IsNullOrEmpty(home.MaDonViQLThucHien) && home.MaDonViQLThucHien != "0")
                    {
                        nhaKhoaHocs = nhaKhoaHocs.Where(p => p.MaDonViQL.ToString() == home.MaDonViQLThucHien).ToList();
                    }
                    if (!String.IsNullOrEmpty(home.SearchValue))
                    {
                        nhaKhoaHocs = nhaKhoaHocs.Where(p => (p.HoNKH + " " + p.TenNKH).ToLower().Contains(home.SearchValue.ToLower())).ToList();
                    }
                    var lstNKH = new List<NhaKhoaHocViewModel>();
                    for (int i = 0; i < nhaKhoaHocs.Count; i++)
                    {
                        NhaKhoaHocViewModel nkh = NhaKhoaHocViewModel.Mapping(nhaKhoaHocs[i]);
                        lstNKH.Add(nkh);
                    }
                    int totalPage = (int)Math.Ceiling((decimal)lstNKH.Count() / 6);
                    ViewBag.TotalItem = lstNKH.Count();
                    IPagedList<NhaKhoaHocViewModel> pageOrders = new StaticPagedList<NhaKhoaHocViewModel>(lstNKH, No_Of_Page, 1, totalPage);

                    return View("~/Views/NhaKhoaHocs/Index.cshtml", pageOrders);

            }
        }

        public ActionResult Admin()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

    }
}