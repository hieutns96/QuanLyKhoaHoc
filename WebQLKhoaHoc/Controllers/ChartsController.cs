using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebQLKhoaHoc.Models;

namespace WebQLKhoaHoc.Controllers
{
    public class ChartsController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();
        private QLKHRepository QLKHrepo = new QLKHRepository();
        // GET: Charts
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThongKe(string unit)
        {
            if (unit == "total")
            {
                return RedirectToAction("DonViTables");
            }
            else if (unit == "degree")
            {
                return RedirectToAction("TrinhDoCharts");
            }
            else
            {
                return RedirectToAction("NgachCharts");
            }
        }


        public ActionResult TrinhDoCharts()
        {
            List<TrinhDoChartViewModel> res = new List<TrinhDoChartViewModel>();
            List<HocVi> listHV = db.HocVis.ToList();
            foreach (var item in listHV)
            {

                int soLuong = db.NhaKhoaHocs.Where(p => p.MaHocVi == item.MaHocVi).ToList().Count();

                TrinhDoChartViewModel trinhdovm = TrinhDoChartViewModel.Mapping(item.TenHocVi, soLuong);
                res.Add(trinhdovm);
            }
            return View(res);
        }
        public ActionResult NgachCharts()
        {
            List<NgachVienChucChartViewModel> res = new List<NgachVienChucChartViewModel>();
            List<NgachVienChuc> listNVC = db.NgachVienChucs.ToList();
            foreach (var item in listNVC)
            {

                int soLuong = db.NhaKhoaHocs.Where(p => p.MaNgachVienChuc == item.MaNgach).ToList().Count();

                NgachVienChucChartViewModel ngachvm = NgachVienChucChartViewModel.Mapping(item.TenNgach, soLuong);
                res.Add(ngachvm);
            }
            return View(res);
        }

        public ActionResult DonViTables()
        {
            DonViTablesViewModel resModel = new DonViTablesViewModel();
            List<DonViQL> listDVQL = db.DonViQLs.ToList();
            List<HocVi> listHV = db.HocVis.ToList();
            List<HocHam> listHH = db.HocHams.ToList();

            resModel.Header.Add("Tên đơn vị");
            foreach (var hocVi in listHV)
            {
                resModel.Header.Add(hocVi.TenHocVi);
            }
            foreach (var hocHam in listHH)
            {
                resModel.Header.Add(hocHam.TenHocHam);
            }
            resModel.Header.Add("Tổng số bài");
            resModel.Header.Add("Trong nước");
            resModel.Header.Add("Ngoài nước");
            foreach (var item_donVi in listDVQL)
            {
                List<Object> row = new List<Object>() { item_donVi.TenDonVI };
                foreach (var hocVi in listHV)
                {
                    int soLuong = db.NhaKhoaHocs.Where(p => p.MaHocVi == hocVi.MaHocVi && p.MaDonViQL == item_donVi.MaDonVi).Count();
                    row.Add(soLuong);
                }
                foreach (var hocHam in listHH)
                {
                    int soLuong = db.NhaKhoaHocs.Where(p => p.MaHocHam == hocHam.MaHocHam && p.MaDonViQL == item_donVi.MaDonVi).Count();
                    row.Add(soLuong);
                }
                var baiBaoTheoDonVi = db.BaiBaos.Join(
                                        db.DSNguoiThamGiaBaiBaos,
                                        baiBao => baiBao.MaBaiBao,
                                        dsThamGiaBB => dsThamGiaBB.MaBaiBao,
                                        (baiBao, dsThamGiaBB) => new { baiBao, dsThamGiaBB.MaNKH })
                                     .Join(
                                        db.NhaKhoaHocs.Where(nkh => nkh.MaDonViQL == item_donVi.MaDonVi),
                                        baiBao_dsThamGiaBB => baiBao_dsThamGiaBB.MaNKH,
                                        nhaKhoaHoc => nhaKhoaHoc.MaNKH,
                                        (baiBao_dsThamGiaBB, nhaKhoaHoc) => new { baiBao_dsThamGiaBB.baiBao })
                                     .Select(bb_ds_nkh_dv => new { bb_ds_nkh_dv.baiBao });
                var soBB_theoDonVi = baiBaoTheoDonVi.Count();
                var soBB_theoDonVi_TrongNuoc = baiBaoTheoDonVi.Where(p => (bool)(p.baiBao.LaTrongNuoc == true)).Count();
                var soBB_theoDonVi_NgoaiNuoc = baiBaoTheoDonVi.Where(p => (bool)(p.baiBao.LaTrongNuoc == false)).Count();
                row.Add(soBB_theoDonVi);
                row.Add(soBB_theoDonVi_TrongNuoc);
                row.Add(soBB_theoDonVi_NgoaiNuoc);
                resModel.Rows.Add(row);
            }
            return View(resModel);
        }

        public ActionResult DonViCharts(string viettat_hocham_hocvi)
        {
            List<List<Object>> data = new List<List<Object>>();
            List<DonViQL> listDVQL = db.DonViQLs.ToList();
            HocVi hvi = db.HocVis.SingleOrDefault(p => p.TenVietTat == viettat_hocham_hocvi);
            if (hvi != null)
            {
                foreach (var item_donVi in listDVQL)
                {
                    List<Object> row = new List<Object>() { item_donVi.TenDonVI };
                    var hocViTheoDonVi = db.NhaKhoaHocs.Where(nkh => nkh.MaDonViQL == item_donVi.MaDonVi)
                                     .Join(
                                        db.HocVis.Where(hv => hv.TenVietTat == viettat_hocham_hocvi),
                                        nkh => nkh.MaHocVi,
                                        hocvis => hocvis.MaHocVi,
                                        (nkh, hocvis) => new { nkh.MaNKH })
                                     .Select(nkh_hvis => new { nkh_hvis.MaNKH });
                    var soHV_theoDonVi = hocViTheoDonVi.Count();
                    row.Add(soHV_theoDonVi);
                    data.Add(row);
                }
            }
            else
            {
                foreach (var item_donVi in listDVQL)
                {
                    List<Object> row = new List<Object>() { item_donVi.TenDonVI };
                    var hocHamTheoDonVi = db.NhaKhoaHocs.Where(nkh => nkh.MaDonViQL == item_donVi.MaDonVi)
                                     .Join(
                                        db.HocHams.Where(hh => hh.TenVietTat == viettat_hocham_hocvi),
                                        nkh => nkh.MaHocVi,
                                        hochams => hochams.MaHocHam,
                                        (nkh, hocvis) => new { nkh.MaNKH })
                                     .Select(nkh_hvis => new { nkh_hvis.MaNKH });
                    var soHH_theoDonVi = hocHamTheoDonVi.Count();
                    row.Add(soHH_theoDonVi);
                    data.Add(row);
                }
            }
            return PartialView(data);
        }


        // chart about books/documents
        public ActionResult SachCharts()
        {
            return View();
        }

        public JsonResult DataSachCharts(string cachThongKe, DateTime? from_date, DateTime? to_date)
        {
            var pre = PredicateBuilder.New<SachGiaoTrinh>(true);

            if (from_date != null && to_date != null)
            {
                pre = pre.And(s => ((s.NamXuatBan >= from_date) && (s.NamXuatBan <= to_date)));
            }
            var sachs = db.SachGiaoTrinhs.AsExpandable().Where(pre);
            List<List<Object>> res = null;
            if (cachThongKe == "phanloai")
            {
                var loaiSachs = db.PhanLoaiSaches.ToList();
                var sach_phanloai = sachs.Join(
                                        db.PhanLoaiSaches,
                                        sach => sach.MaLoai,
                                        phanloai => phanloai.MaLoai,
                                        (sach, phanloai) => new { sach.MaSach, phanloai.MaLoai }
                                    );
                res = new List<List<object>>() { new List<object>() { "Loại", "Số lượng" } };
                foreach (var loai in loaiSachs)
                {
                    var soLuongSach = sach_phanloai.Where(p => p.MaLoai == loai.MaLoai).Count();
                    res.Add(new List<object>() { loai.TenLoai, soLuongSach });
                }
            }
            else
            {
                var nhaXuatBans = db.NhaXuatBans.ToList();
                var sach_nhaxuatban = sachs.Join(
                                        db.NhaXuatBans,
                                        sach => sach.MaNXB,
                                        nhaxuatban => nhaxuatban.MaNXB,
                                        (sach, nhaxuatban) => new { sach.MaSach, nhaxuatban.MaNXB }
                                    );
                res = new List<List<object>>() { new List<object>() { "Nhà xuất bản", "Số lượng" } };
                foreach (var nxb in nhaXuatBans)
                {
                    var soLuongSach = sach_nhaxuatban.Where(p => p.MaNXB == nxb.MaNXB).Count();
                    res.Add(new List<object>() { nxb.TenNXB, soLuongSach });
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }



        //// about topic
        public ActionResult topicChart()
        {
            DonViTablesViewModel resModel = new DonViTablesViewModel();
            List<string> capdetai = db.CapDeTais.Select(p => p.TenCapDeTai).ToList();
            List<string> donviql = db.DonViQLs.Select(p => p.TenDonVI).ToList();
            /*Table Header*/
            resModel.Header.Add("Tên đơn vị quản lý");
            foreach (string ten in capdetai)
            {
                resModel.Header.Add(ten);
            }
            resModel.Header.Add("Tổng số");


            /*table data*/
            foreach (string dvql in donviql)
            {
                int total = 0;
                List<object> row = new List<object>();
                foreach (string capdt in capdetai)
                {
                    var datarow = db.DeTais.Where(p => (p.CapDeTai.TenCapDeTai == capdt && p.DonViQL.TenDonVI == dvql)).Count();
                    row.Add(datarow);
                    total = (datarow > 0) ? total + datarow : total;
                }
                row.Add(total);
                resModel.Rows.Add(row);
            }
            ViewBag.TenDonVi = donviql;
            return View(resModel);
        }

        [HttpPost]
        public ActionResult topicChart(string unit, DateTime? from_date, DateTime? to_date)
        {
            DateTime fd = new DateTime();
            DateTime td = new DateTime();
            if (from_date != null)
            {
                fd = Convert.ToDateTime(from_date);
            }
            else
            {
                fd = DateTime.MinValue;
            }
            if (to_date != null)
            {
                td = Convert.ToDateTime(to_date);
            }
            else
            {
                td = DateTime.MaxValue;
            }

            if (unit == "total")
            {
                DonViTablesViewModel resModel = new DonViTablesViewModel();
                List<string> capdetai = db.CapDeTais.Select(p => p.TenCapDeTai).ToList();
                List<string> donviql = db.DonViQLs.Select(p => p.TenDonVI).ToList();
                /*Table Header*/
                resModel.Header.Add("Tên đơn vị quản lý");
                foreach (string ten in capdetai)
                {
                    resModel.Header.Add(ten);
                }
                resModel.Header.Add("Tổng số");


                /*table data*/
                foreach (string dvql in donviql)
                {
                    int total = 0;
                    List<object> row = new List<object>();
                    foreach (string capdt in capdetai)
                    {
                        var datarow = db.DeTais.Where(p => (p.CapDeTai.TenCapDeTai == capdt && p.DonViQL.TenDonVI == dvql
                            && p.NamBD.Value.Year > fd.Year && p.NamKT.Value.Year < td.Year)).Count();
                        row.Add(datarow);
                        total = (datarow > 0) ? total + datarow : total;
                    }
                    row.Add(total);
                    resModel.Rows.Add(row);
                }
                ViewBag.TenDonVi = donviql;
                return View(resModel);
            }
            else if (unit == "fieldSector")
            {
                return RedirectToAction("TopicFieldSector", new { from_date, to_date });
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Dữ liệu chỉ tiêu chưa chính xác");
            }
        }

        public ActionResult TopicFieldSector(DateTime? from_date, DateTime? to_date)
        {
            DateTime fd = new DateTime();
            DateTime td = new DateTime();
            if (from_date != null)
            {
                fd = Convert.ToDateTime(from_date);
            }
            else
            {
                fd = DateTime.MinValue;
            }
            if (to_date != null)
            {
                td = Convert.ToDateTime(to_date);
            }
            else
            {
                td = DateTime.MaxValue;
            }

            IDictionary<string, int> linhvuc = new Dictionary<string, int>();
            List<string> nhomlinhvuc = db.NhomLinhVucs.Select(p => p.TenNhomLinhVuc).ToList();
            //int tongso
            foreach (string ten in nhomlinhvuc)
            {
                int sodetai = db.DeTais.Where(p => (p.LinhVuc.NhomLinhVuc.TenNhomLinhVuc == ten
                        && p.NamBD.Value.Year > fd.Year && p.NamKT.Value.Year < td.Year)).Count();
                linhvuc.Add(ten, sodetai);
            }
            ViewBag.linhvuc = Json(linhvuc);
            return View(linhvuc);
        }

        //// about topic
        public ActionResult articleChart()
        {
            DonViTablesViewModel resModel = new DonViTablesViewModel();
            List<string> captapchi = db.CapTapChis.Select(p => p.TenCapTapChi).ToList();

            /*Table Header*/
            foreach (string ten in captapchi)
            {
                resModel.Header.Add(ten);
            }
            resModel.Header.Add("Tổng số");
            /*table data*/

            int total = 0;
            List<object> row = new List<object>();
            foreach (string captc in captapchi)
            {
                var datarow = db.BaiBaos.Where(p => (p.CapTapChi.TenCapTapChi == captc)).Count();
                row.Add(datarow);
                total = (datarow > 0) ? total + datarow : total;
            }
            row.Add(total);
            resModel.Rows.Add(row);


            return View(resModel);
        }
        [HttpPost]
        public ActionResult articleChart(string unit, DateTime? from_date, DateTime? to_date)
        {
            DateTime fd = DateTime.MinValue;
            DateTime td = DateTime.MaxValue;
            if (from_date != null)
            {
                fd = Convert.ToDateTime(from_date);
            }
            if (to_date != null)
            {
                td = Convert.ToDateTime(to_date);
            }

            if (unit == "total")
            {
                DonViTablesViewModel resModel = new DonViTablesViewModel();
                List<string> captapchi = db.CapTapChis.Select(p => p.TenCapTapChi).ToList();

                /*Table Header*/
                foreach (string ten in captapchi)
                {
                    resModel.Header.Add(ten);
                }
                resModel.Header.Add("Tổng số");
                /*table data*/

                int total = 0;
                List<object> row = new List<object>();
                foreach (string captc in captapchi)
                {
                    var datarow = db.BaiBaos.Where(p => (p.CapTapChi.TenCapTapChi == captc
                        && p.NamDangBao.Value.Year > fd.Year && p.NamDangBao.Value.Year < td.Year)).Count();
                    row.Add(datarow);
                    total = (datarow > 0) ? total + datarow : total;
                }
                row.Add(total);

                resModel.Rows.Add(row);


                return View(resModel);
            }
            else if (unit == "fieldSector")
            {
                return RedirectToAction("ArticleFieldSector", new { from_date, to_date });
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Dữ liệu chỉ tiêu chưa chính xác");
            }
        }
        public ActionResult ArticleFieldSector(DateTime? from_date, DateTime? to_date)
        {
            DateTime fd = DateTime.MinValue;
            DateTime td = DateTime.MaxValue;
            if (from_date != null)
            {
                fd = Convert.ToDateTime(from_date);
            }
            if (to_date != null)
            {
                td = Convert.ToDateTime(to_date);
            }


            IDictionary<string, int> linhvuc = new Dictionary<string, int>();
            List<string> nhomlinhvuc = db.NhomLinhVucs.Select(p => p.TenNhomLinhVuc).ToList();
            //int tongso
            foreach (string ten in nhomlinhvuc)
            {
                linhvuc.Add(ten, db.BaiBaos.Where(p => (p.LinhVucs.Any(t => t.NhomLinhVuc.TenNhomLinhVuc == ten)
                                                        && p.NamDangBao.Value.Year > fd.Year && p.NamDangBao.Value.Year < td.Year)).Count());
            }
            ViewBag.linhvuc = Json(linhvuc);
            return View(linhvuc);
        }

    }
}