using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        //public ActionResult totalCharts()
        //{
        //    List<totalChartViewModel> res = new List<totalChartViewModel>();
        //    List<DonViQL> listDVQL = db.DonViQLs.ToList();
        //    foreach (var item in listDVQL)
        //    {
        //        int nGSDD = db.NhaKhoaHocs.Where(p => p.MaDonViQL == item.MaDonVi && p.MaHocHam == 3).ToList().Count();
        //        int nGS = db.NhaKhoaHocs.Where(p => p.MaDonViQL == item.MaDonVi && p.MaHocHam == 2).ToList().Count();
        //        int nPGS = db.NhaKhoaHocs.Where(p => p.MaDonViQL == item.MaDonVi && p.MaHocHam == 1).ToList().Count();
        //        int nTSKH = db.NhaKhoaHocs.Where(p => p.MaDonViQL == item.MaDonVi && p.MaHocVi == 7).ToList().Count();
        //        int nTS = db.NhaKhoaHocs.Where(p => p.MaDonViQL == item.MaDonVi && p.MaHocVi == 6).ToList().Count();
        //        int nThS = db.NhaKhoaHocs.Where(p => p.MaDonViQL == item.MaDonVi && p.MaHocVi == 5).ToList().Count();
        //        int nGV = db.NhaKhoaHocs.Where(p => p.MaDonViQL == item.MaDonVi && (p.MaNgachVienChuc == 1 || p.MaNgachVienChuc == 2)).ToList().Count();
        //        int nNCV = db.NhaKhoaHocs.Where(p => p.MaDonViQL == item.MaDonVi && p.MaNgachVienChuc == 10).ToList().Count();

        //        totalChartViewModel totalchart = totalChartViewModel.Mapping(item.MaDonVi,nGSDD, nGS, nPGS, nTSKH, nTS, nThS, nGV, nNCV);
        //        res.Add(totalchart);
        //    }
        //    return View(res);
        //}

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
                                     .Join(db.NhaKhoaHocs,
                                        baiBao_dsThamGiaBB => baiBao_dsThamGiaBB.MaNKH,
                                        nhaKhoaHoc => nhaKhoaHoc.MaNKH,
                                        (baiBao_dsThamGiaBB, nhaKhoaHoc) => new { baiBao_dsThamGiaBB.baiBao, nhaKhoaHoc.MaDonViQL })
                                     .Join(db.DonViQLs.Where(p => p.MaDonVi == item_donVi.MaDonVi),
                                        bb_ds_nkh => bb_ds_nkh.MaDonViQL,
                                        donVi => donVi.MaDonVi,
                                        (bb_ds_nkh, donVi) => new { bb_ds_nkh.baiBao })
                                     .Select(bb_ds_nkh_dv => new { bb_ds_nkh_dv.baiBao } );
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
                    var hocViTheoDonVi = db.NhaKhoaHocs
                                     .Join(
                                        db.HocVis.Where(hv => hv.TenVietTat == viettat_hocham_hocvi),
                                        nkh => nkh.MaHocVi,
                                        hocvis => hocvis.MaHocVi,
                                        (nkh, hocvis) => new { nkh })
                                     .Join(
                                        db.DonViQLs.Where(dv => dv.MaDonVi == item_donVi.MaDonVi),
                                        nkh_hocvis => nkh_hocvis.nkh.MaDonViQL,
                                        dvis => dvis.MaDonVi,
                                        (nkh_hocvis, dvis) => new { nkh_hocvis.nkh.MaNKH })
                                     .Select(nkh_hvis_dvis => new { nkh_hvis_dvis.MaNKH });
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
                    var hocHamTheoDonVi = db.NhaKhoaHocs
                                     .Join(
                                        db.HocHams.Where(hh => hh.TenVietTat == viettat_hocham_hocvi),
                                        nkh => nkh.MaHocVi,
                                        hochams => hochams.MaHocHam,
                                        (nkh, hocvis) => new { nkh })
                                     .Join(
                                        db.DonViQLs.Where(dv => dv.MaDonVi == item_donVi.MaDonVi),
                                        nkh_hocvis => nkh_hocvis.nkh.MaDonViQL,
                                        dvis => dvis.MaDonVi,
                                        (nkh_hocvis, dvis) => new { nkh_hocvis.nkh.MaNKH })
                                     .Select(nkh_hvis_dvis => new { nkh_hvis_dvis.MaNKH });
                    var soHH_theoDonVi = hocHamTheoDonVi.Count();
                    row.Add(soHH_theoDonVi);
                    data.Add(row);
                }
            }
            return PartialView(data);
        }


        //// chart about books/documents
        //public ActionResult book()
        //{
        //    //ViewBag.DVQL = new SelectList(db.DonViQLs, "MaDonVi", "TenDonVI");
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult book(int unit,DateTime? from_date, DateTime? to_date)
        //{
        //    List<DSTacGia> dSTacGias = db.DSTacGias.Where(p => p.LaChuBien == true).ToList();
        //    List<NhaKhoaHoc> nhaKhoaHocs = new List<NhaKhoaHoc>();
        //    if (unit == 0)
        //    {
        //        nhaKhoaHocs = db.NhaKhoaHocs.ToList();
        //    }
        //    else
        //    {
        //        nhaKhoaHocs = db.NhaKhoaHocs.Where(p => p.MaDonViQL == unit).ToList();
        //    }
        //    List<SachGiaoTrinh> sachGiaoTrinhs = new List<SachGiaoTrinh>();
        //    if (from_date != null && to_date != null)
        //    {
        //        sachGiaoTrinhs = db.SachGiaoTrinhs.Where(p => p.NamXuatBan >= from_date && p.NamXuatBan <= to_date).ToList();
        //    }
        //    else if (from_date == null && to_date != null)
        //    {
        //        sachGiaoTrinhs = db.SachGiaoTrinhs.Where(p => p.NamXuatBan <= to_date).ToList();
        //    }
        //    else if (from_date != null && to_date == null)
        //    {
        //        sachGiaoTrinhs = db.SachGiaoTrinhs.Where(p => p.NamXuatBan >= from_date).ToList();
        //    }
        //    else
        //    {
        //        sachGiaoTrinhs = db.SachGiaoTrinhs.ToList();
        //    }

        //    List<DSTacGia> tacgias = new List<DSTacGia>();
        //    // find all books of scientists 
        //    for (int i = 0; i < dSTacGias.ToList().Count(); i++)
        //    {
        //        for (int j = 0; j < nhaKhoaHocs.ToList().Count(); j++)
        //        {
        //            if (dSTacGias[i].MaNKH== nhaKhoaHocs[j].MaNKH)
        //            {
        //                tacgias.Add(dSTacGias[i]);
        //                break;
        //            }
        //        }
        //    }
        //    //var tacgias = (from nkh in nhaKhoaHocs
        //    //               join tg  in dSTacGias
        //    //               on nkh.MaNKH equals tg.MaNKH
        //    //               select new
        //    //               {
        //    //                   tg.MaSach,
        //    //                   tg.MaNKH
        //    //               }).ToList();

        //    //var sachs = (from sach in sachGiaoTrinhs
        //    //             join tgs in tacgias
        //    //             on sach.MaSach equals tgs.MaSach
        //    //             select new
        //    //             {
        //    //                 sach.MaSach,
        //    //                 sach.MaLoai,
        //    //                 sach.TenSach
        //    //             }).ToList();
        //    List<SachGiaoTrinh> giaoTrinhs = new List<SachGiaoTrinh>();

        //    for (int i = 0; i < sachGiaoTrinhs.ToList().Count(); i++)
        //    {
        //        for (int j = 0; j < tacgias.ToList().Count(); j++)
        //        {
        //            if (sachGiaoTrinhs[i].MaSach == tacgias[j].MaSach)
        //            {
        //                giaoTrinhs.Add(sachGiaoTrinhs[i]);
        //            }
        //        }
        //    }
        //    // phân loại sách từng loại

        //    int ngiaoTrinh = giaoTrinhs.Where(p => p.MaLoai == 2).ToList().Count();
        //    int nChuyenKhao = giaoTrinhs.Where(p => p.MaLoai == 1).ToList().Count();
        //    int nThamKhao = giaoTrinhs.Where(p => p.MaLoai == 3).ToList().Count();
        //    int nKhac = giaoTrinhs.Where(p => p.MaLoai == 4).ToList().Count();

        //    ViewBag.giaoTrinh = ngiaoTrinh;
        //    ViewBag.chuyenKhao = nChuyenKhao;
        //    ViewBag.thamKhao = nThamKhao;
        //    ViewBag.Khac = nKhac;
        //    ViewBag.unit = unit;
        //    ViewBag.fromdate = from_date;
        //    ViewBag.todate = to_date;
        //    return View();
        //}


        //// about article
        //public ActionResult article()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult article(int unit, DateTime? from_date, DateTime? to_date)
        //{
        //    List<DSNguoiThamGiaBaiBao> dSNguoiThamGias = db.DSNguoiThamGiaBaiBaos.ToList();
        //    List<NhaKhoaHoc> nhaKhoaHocs = new List<NhaKhoaHoc>();
        //    if (unit == 0)
        //    {
        //        nhaKhoaHocs = db.NhaKhoaHocs.ToList();
        //    }
        //    else
        //    {
        //        nhaKhoaHocs = db.NhaKhoaHocs.Where(p => p.MaDonViQL == unit).ToList();
        //    }
        //    List<BaiBao> baiBaos = new List<BaiBao>();
        //    if (from_date != null && to_date != null)
        //    {
        //        baiBaos = db.BaiBaos.Where(p => p.NamDangBao >= from_date && p.NamDangBao <= to_date).ToList();
        //    }
        //    else if (from_date == null && to_date != null)
        //    {
        //        baiBaos = db.BaiBaos.Where(p => p.NamDangBao <= to_date).ToList();
        //    }
        //    else if (from_date != null && to_date == null)
        //    {
        //        baiBaos = db.BaiBaos.Where(p => p.NamDangBao >= from_date).ToList();
        //    }
        //    else
        //    {
        //        baiBaos = db.BaiBaos.ToList();
        //    }

        //    List<DSNguoiThamGiaBaiBao> dSNguoiThamGiaBaiBaos = new List<DSNguoiThamGiaBaiBao>();
        //    // find all article of scientists 
        //    for (int i = 0; i < dSNguoiThamGias.Count(); i++)
        //    {
        //        for (int j = 0; j < nhaKhoaHocs.Count(); j++)
        //        {
        //            if (dSNguoiThamGias[i].MaNKH == nhaKhoaHocs[j].MaNKH)
        //            {
        //                dSNguoiThamGiaBaiBaos.Add(dSNguoiThamGias[i]);
        //                break;
        //            }
        //        }
        //    }

        //    List<BaiBao> articles = new List<BaiBao>();

        //    for (int i = 0; i < baiBaos.Count(); i++)
        //    {
        //        for (int j = 0; j < dSNguoiThamGiaBaiBaos.Count(); j++)
        //        {
        //            if (baiBaos[i].MaBaiBao == dSNguoiThamGiaBaiBaos[j].MaBaiBao)
        //            {
        //                articles.Add(baiBaos[i]);
        //                break;
        //            }
        //        }
        //    }

        //    // phân loại trong và ngoai nước
        //    List<BaiBao> articles_in = articles.Where(p => p.LaTrongNuoc == true).ToList();
        //    List<BaiBao> articles_out = articles.Where(p => p.LaTrongNuoc == false).ToList();

        //    int narticle_in_HDCDGS = articles_in.Where(p => p.MaCapTapChi == 2).ToList().Count();
        //    int narticle_in_Khac = articles_in.Where(p => p.MaCapTapChi == 4).ToList().Count();

        //    int narticle_ISI = articles_out.Where(p => p.MaCapTapChi == 1).ToList().Count();
        //    int narticle_SCOPUS = articles_out.Where(p => p.MaCapTapChi == 3).ToList().Count();
        //    int narticle_ESCI = articles_out.Where(p => p.MaCapTapChi == 5).ToList().Count();
        //    int narticle_out_Khac = articles_out.Where(p => p.MaCapTapChi == 4).ToList().Count();

        //    ViewBag.nHDCDGS = narticle_in_HDCDGS;
        //    ViewBag.nKhac_in = narticle_in_Khac;
        //    ViewBag.nISI = narticle_ISI;
        //    ViewBag.nSCOPUS = narticle_SCOPUS;
        //    ViewBag.nESCI = narticle_ESCI;
        //    ViewBag.nKhac_out = narticle_out_Khac;

        //    ViewBag.unit = unit;
        //    ViewBag.fromdate = from_date;
        //    ViewBag.todate = to_date;
        //    return View();
        //}

        //// about topic
        //public ActionResult topicChart()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult topicChart(string unit,DateTime? from_date, DateTime? to_date)
        //{
        //    DateTime fd = new DateTime();
        //    DateTime td = new DateTime();
        //    if (from_date != null)
        //    {
        //        fd = Convert.ToDateTime(from_date);
        //    }
        //    else
        //    {
        //        fd = DateTime.MinValue;
        //    }
        //    if (to_date != null)
        //    {
        //        td = Convert.ToDateTime(to_date);
        //    }
        //    else
        //    {
        //        td = DateTime.MaxValue;
        //    }

        //    topicdata tpdata = topicdata.Mapping(Int32.Parse(unit), fd, td);

        //    if (unit == "0" || unit == null)
        //    {
        //        return RedirectToAction("topicsChart", tpdata);
        //    }
        //    else
        //    {
        //        return RedirectToAction("atopicChart", tpdata);
        //    }

        //}

        //public ActionResult topicsChart(topicdata tpdt)
        //{
        //    List<topicChartViewModel> res = new List<topicChartViewModel>();
        //    List<DonViQL> listDVQL = db.DonViQLs.ToList();
        //    List<DeTai> deTais = new List<DeTai>();
        //    deTais = db.DeTais.Where(p => p.NamBD >= tpdt.fromdate && p.NamBD <= tpdt.todate).ToList();
        //    foreach (var item in listDVQL)
        //    {

        //        int a = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 1).ToList().Count();
        //        int b = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 2).ToList().Count();
        //        int c = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 3).ToList().Count();
        //        int d = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 4).ToList().Count();
        //        int e = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 5).ToList().Count();
        //        int f = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 6).ToList().Count();
        //        int g = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 7).ToList().Count();
        //        int h = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 8).ToList().Count();
        //        int i = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 9).ToList().Count();
        //        int k = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 10).ToList().Count();
        //        int l = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 11).ToList().Count();
        //        int m = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 12).ToList().Count();
        //        int n = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 13).ToList().Count();
        //        int z = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 14).ToList().Count();
        //        int q = deTais.Where(p => p.MaDonViQLThucHien == item.MaDonVi && p.MaCapDeTai == 15).ToList().Count();

        //        topicChartViewModel topicChart = topicChartViewModel.Mapping(item.MaDonVi, a, b, c, d, e, f, g, h, i, k, l, m, n, z, q);
        //        res.Add(topicChart);
        //    }

        //    ViewBag.unit = tpdt.MaDVQL;
        //    ViewBag.fromdate = tpdt.fromdate;
        //    ViewBag.todate = tpdt.todate;
        //    return View(res);
        //}

        //public ActionResult atopicChart(topicdata tpdt)
        //{
        //    topicChartViewModel res = new topicChartViewModel();
        //    List<DeTai> deTais = db.DeTais.Where(p =>p.MaDonViQLThucHien == tpdt.MaDVQL && p.NamBD >= tpdt.fromdate && p.NamBD <= tpdt.todate).ToList();

        //    List<DeTai> a = deTais.Where(p => p.MaCapDeTai == 1).ToList();
        //    List<DeTai> b = deTais.Where(p => p.MaCapDeTai == 2).ToList();
        //    List<DeTai> c = deTais.Where(p => p.MaCapDeTai == 3).ToList();
        //    List<DeTai> d = deTais.Where(p => p.MaCapDeTai == 4).ToList();
        //    List<DeTai> e = deTais.Where(p => p.MaCapDeTai == 5).ToList();
        //    List<DeTai> f = deTais.Where(p => p.MaCapDeTai == 6).ToList();
        //    List<DeTai> g = deTais.Where(p => p.MaCapDeTai == 7).ToList();
        //    List<DeTai> h = deTais.Where(p => p.MaCapDeTai == 8).ToList();
        //    List<DeTai> i = deTais.Where(p => p.MaCapDeTai == 9).ToList();
        //    List<DeTai> k = deTais.Where(p => p.MaCapDeTai == 10).ToList();
        //    List<DeTai> l = deTais.Where(p => p.MaCapDeTai == 11).ToList();
        //    List<DeTai> m = deTais.Where(p => p.MaCapDeTai == 12).ToList();
        //    List<DeTai> n = deTais.Where(p => p.MaCapDeTai == 13).ToList();
        //    List<DeTai> z = deTais.Where(p => p.MaCapDeTai == 14).ToList();
        //    List<DeTai> q = deTais.Where(p => p.MaCapDeTai == 15).ToList();

        //    res = topicChartViewModel.Mapping(tpdt.MaDVQL, a.Count, b.Count, c.Count, d.Count, e.Count, f.Count, g.Count, h.Count, i.Count, k.Count, l.Count, m.Count, n.Count, z.Count, q.Count);
        //    // sum item
        //    ViewBag.a = a.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.b = b.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.c = c.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.d = d.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.e = e.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.f = f.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.g = g.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.h = h.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.i = i.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.k = k.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.l = l.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.m = m.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.n = n.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.z = z.Sum(p => Convert.ToInt32(p.KinhPhi));
        //    ViewBag.q = q.Sum(p => Convert.ToInt32(p.KinhPhi));

        //    ViewBag.sumAll = ViewBag.a + ViewBag.b + ViewBag.c + ViewBag.d + ViewBag.e + ViewBag.f + ViewBag.g + ViewBag.h + ViewBag.i + ViewBag.k + ViewBag.l + ViewBag.m + ViewBag.n + ViewBag.z + ViewBag.q;

        //    ViewBag.unit = tpdt.MaDVQL;
        //    ViewBag.fromdate = tpdt.fromdate;
        //    ViewBag.todate = tpdt.todate;


        //    return View(res);
        //}
    }
}