using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebQLKhoaHoc;
using WebQLKhoaHoc.Models;
using System.IO;
using System.Data.Entity.Migrations;
using WebGrease.Css.Extensions;
using LinqKit;

namespace WebQLKhoaHoc.Controllers
{
    public class BaiBaosController : Controller
    {
        private QLKhoaHocEntities db = new QLKhoaHocEntities();
        private QLKHRepository QLKHrepo = new QLKHRepository();

        // GET: BaiBaos
        public async Task<ActionResult> Index(int? Page_No, [Bind(Include = "MaLinhVuc,MaPhanLoaiTapChi,MaLoaiTapChi,MaCapTapChi,CQXuatBan,Fromdate,Todate,SearchValue")] BaiBaoSearchViewModel baibao, int? nkhId)
        {
            var pre = PredicateBuilder.True<BaiBao>();
            //var baibaos = db.BaiBaos.Include(b => b.CapTapChi).Include(b => b.PhanLoaiTapChi).Include(b => b.LinhVucs).Include(b => b.DSNguoiThamGiaBaiBaos).ToList();

            if (nkhId == null)
            {
                if (!String.IsNullOrEmpty(baibao.MaLinhVuc))
                {
                    if (baibao.MaLinhVuc[0] == 'a')
                        pre = pre.And(p => p.LinhVucs.Any(t => t.MaLinhVuc.ToString() == baibao.MaLinhVuc.Substring(1, baibao.MaLinhVuc.Length - 1)));
                    //baibaos = baibaos.Where(p => p.LinhVucs.Any(t => t.MaLinhVuc.ToString() == baibao.MaLinhVuc.Substring(1, baibao.MaLinhVuc.Length - 1))).ToList();

                    else
                        pre = pre.And(p => p.LinhVucs.Any(t => t.MaNhomLinhVuc.ToString() == baibao.MaLinhVuc));
                    //baibaoss = baibaoss.Where(p => p.LinhVucs.Any(t => t.MaNhomLinhVuc.ToString() == baibao.MaLinhVuc)).ToList();
                }
                if (!String.IsNullOrEmpty(baibao.MaPhanLoaiTapChi))
                {
                    pre = pre.And(p => p.MaLoaiTapChi.ToString() == baibao.MaPhanLoaiTapChi);
                    //baibaos = baibaos.Where(p => p.MaLoaiTapChi.ToString() == baibao.MaPhanLoaiTapChi).ToList();
                }

                if (!String.IsNullOrEmpty(baibao.MaLoaiTapChi))
                {
                    if (baibao.MaLoaiTapChi == "0")
                        pre = pre.And(p => p.LaTrongNuoc == false);
                    //baibaos = baibaos.Where(p => p.LaTrongNuoc == false).ToList();
                    else
                        pre = pre.And(p => p.LaTrongNuoc == true);
                    //baibaos = baibaos.Where(p => p.LaTrongNuoc == true).ToList();
                }

                if (baibao.Fromdate > DateTime.MinValue)
                {
                    ViewBag.Fromdate = baibao.Fromdate.ToShortDateString();
                    pre = pre.And(p => p.NamDangBao.Value.Year >= baibao.Fromdate.Year);
                    //baibaos = baibaos.Where(p => p.NamDangBao.Value.Year >= baibao.Fromdate.Year).ToList();
                }
                if (baibao.Todate > DateTime.MinValue)
                {
                    ViewBag.Todate = baibao.Todate.ToShortDateString();

                    pre = pre.And(p => p.NamDangBao.Value.Year <= baibao.Todate.Year);
                    //baibaos = baibaos.Where(p => p.NamDangBao.Value.Year <= baibao.Todate.Year).ToList();
                }
                if (!String.IsNullOrEmpty(baibao.CQXuatBan))
                {
                    pre = pre.And(p => p.CQXuatBan.Contains(baibao.CQXuatBan));
                    //baibaos = baibaos.Where(p => p.CQXuatBan.Contains(baibao.CQXuatBan)).ToList();
                }
                if (!String.IsNullOrEmpty(baibao.SearchValue))
                {
                    pre = pre.And(p => p.TenBaiBao.Contains(baibao.SearchValue));
                    //baibaos = baibaos.Where(p => p.TenBaiBao.Contains(baibao.SearchValue)).ToList();
                }
            }
            else
            {
                var mabaibaos = db.DSNguoiThamGiaBaiBaos.Where(p => p.MaNKH == 1).Select(p => p.MaBaiBao).ToList();
                pre = pre.And(p => mabaibaos.Contains(p.MaBaiBao));
                //baibaos = baibaos.Where(p => mabaibaos.Contains(p.MaBaiBao)).ToList();
            }


            ViewBag.MaLinhVuc = new SelectList(QLKHrepo.GetListMenuLinhVuc(), "Id", "TenLinhVuc");


            ViewBag.MaCapTapChi = new SelectList(db.CapTapChis, "MaCapTapChi", "TenCapTapChi");
            ViewBag.MaPhanLoaiTapChi = new SelectList(db.PhanLoaiTapChis, "MaLoaiTapChi", "TenLoaiTapChi");
            ViewBag.MaLoaiTapChi = new List<SelectListItem>
            {
                new SelectListItem { Text = "Trong Nước", Value = "1"},
                new SelectListItem { Text = "Ngoài Nước", Value ="0"},
            };
            ViewBag.CQXuatBan = baibao.CQXuatBan;
            ViewBag.SearchValue = baibao.SearchValue;


            //int Size_Of_Page = 6;
            int No_Of_Page = (Page_No ?? 1);

            var baibaos = db.BaiBaos.Include(b => b.CapTapChi).Include(b => b.PhanLoaiTapChi).Include(b => b.LinhVucs).Include(b => b.DSNguoiThamGiaBaiBaos).AsExpandable().Where(pre).OrderBy(p => p.MaBaiBao).Skip((No_Of_Page - 1) * 6).Take(6).ToList();

            return View(baibaos.ToPagedList(No_Of_Page, 6));
        }


        // GET: BaiBaos/baibaols/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baiBao = await db.BaiBaos.FindAsync(id);
            if (baiBao == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdChuBien = db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == baiBao.MaBaiBao && p.LaTacGiaChinh == true).Select(p => p.MaNKH);
            return View(baiBao);
        }

        // GET: BaiBaos/Create
        public ActionResult Create()
        {

            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();

            ViewBag.MaCapTapChi = new SelectList(db.CapTapChis, "MaCapTapChi", "TenCapTapChi");
            ViewBag.MaLoaiTapChi = new SelectList(db.PhanLoaiTapChis, "MaLoaiTapChi", "TenLoaiTapChi");
            ViewBag.DSNguoiThamGiaBaiBao = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH");
            ViewBag.LinhVuc = new MultiSelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc");
            ViewBag.DeTai = new MultiSelectList(db.DeTais, "MaDeTai", "TenDeTai");
            ViewBag.NguonGoc = new List<SelectListItem> {
                new SelectListItem { Text = "Trong nước" , Value = "true"},
                new SelectListItem { Text = "Ngoài nước" , Value = "false"}
            };

            return View();
        }

        // POST: BaiBaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more baibaols see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(List<int> LinhVuc, List<string> DeTaiBaiBao, List<string> DSNguoiThamGiaBaiBao, [Bind(Include = "MaBaiBao,MaISSN,TenBaiBao,LaTrongNuoc,CQXuatBan,MaLoaiTapChi,MaCapTapChi,NamDangBao,TapPhatHanh,SoPhatHanh,TrangBaiBao,LienKetWeb")] BaiBao baiBao, HttpPostedFileBase linkUpload)
        {
            if (ModelState.IsValid)
            {
                if (linkUpload != null && linkUpload.ContentLength > 0)
                {
                    string filename = Path.GetFileNameWithoutExtension(linkUpload.FileName) + "_" + baiBao.MaBaiBao.ToString() + Path.GetExtension(linkUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/App_Data/uploads/baibao_file"), filename);
                    linkUpload.SaveAs(path);
                    baiBao.LinkFileUpLoad = filename;
                }
                if (LinhVuc != null)
                {
                    baiBao.LinhVucs = db.LinhVucs.Where(p => LinhVuc.Contains(p.MaLinhVuc)).ToList();
                }

                db.BaiBaos.Add(baiBao);
                db.SaveChanges();

                UserLoginViewModel user = (UserLoginViewModel)Session["user"];
                db.DSNguoiThamGiaBaiBaos.Add(new DSNguoiThamGiaBaiBao
                {
                    LaTacGiaChinh = true,
                    MaBaiBao = baiBao.MaBaiBao,
                    MaNKH = user.MaNKH
                });

                if (DSNguoiThamGiaBaiBao != null)
                {
                    foreach (var mankh in DSNguoiThamGiaBaiBao)
                    {
                        DSNguoiThamGiaBaiBao nguoiTGBB = new DSNguoiThamGiaBaiBao
                        {
                            LaTacGiaChinh = false,
                            MaBaiBao = baiBao.MaBaiBao,
                            MaNKH = Int32.Parse(mankh)
                        };
                        db.DSNguoiThamGiaBaiBaos.Add(nguoiTGBB);
                        db.SaveChanges();
                    }
                }
                if (DeTaiBaiBao != null)
                {
                    foreach (var madetai in DeTaiBaiBao)
                    {
                        DSBaiBaoDeTai detai = new DSBaiBaoDeTai
                        {
                            MaBaiBao = baiBao.MaBaiBao,
                            MaDeTai = Int32.Parse(madetai)
                        };
                        db.DSBaiBaoDeTais.Add(detai);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }


            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();
            var tacgiaphu = db.NhaKhoaHocs.Where(p => p.DSNguoiThamGiaBaiBaos.Any(d => d.MaBaiBao == baiBao.MaBaiBao && d.LaTacGiaChinh == false)).Select(p => p.MaNKH).ToList();
            var detaibaibao = db.DSBaiBaoDeTais.Where(p => p.MaBaiBao == baiBao.MaBaiBao).Select(p => p.MaDeTai).ToList();

            ViewBag.MaCapTapChi = new SelectList(db.CapTapChis, "MaCapTapChi", "TenCapTapChi", baiBao.MaCapTapChi);
            ViewBag.MaLoaiTapChi = new SelectList(db.PhanLoaiTapChis, "MaLoaiTapChi", "TenLoaiTapChi", baiBao.MaLoaiTapChi);
            ViewBag.DSNguoiThamGiaBaiBao = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH", tacgiaphu);
            ViewBag.LinhVuc = new MultiSelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc", baiBao.LinhVucs);
            ViewBag.DeTai = new MultiSelectList(db.DeTais, "MaDeTai", "TenDeTai", detaibaibao);
            ViewBag.NguonGoc = new List<SelectListItem> {
                new SelectListItem { Text = "Trong nước" , Value = "true"},
                new SelectListItem { Text = "Ngoài nước" , Value = "false"}
            };

            return View(baiBao);
        }

        // GET: BaiBaos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baiBao = await db.BaiBaos.FindAsync(id);

            if (baiBao == null)
            {
                return HttpNotFound();
            }

            var lstAllNKH = db.NhaKhoaHocs.Where(p => p.MaNKH != 1).Select(p => new
            {
                p.MaNKH,
                TenNKH = p.HoNKH + " " + p.TenNKH
            }).ToList();
            var tacgiaphu = db.NhaKhoaHocs.Where(p => p.DSNguoiThamGiaBaiBaos.Any(d => d.MaBaiBao == baiBao.MaBaiBao && d.LaTacGiaChinh == false)).Select(p => p.MaNKH).ToList();
            var detaibaibao = db.DSBaiBaoDeTais.Where(p => p.MaBaiBao == baiBao.MaBaiBao).Select(p => p.MaDeTai).ToList();
            var linhvucbaibao = db.BaiBaos.Where(p => p.MaBaiBao == baiBao.MaBaiBao).Include(p => p.LinhVucs).FirstOrDefault();
            var linhvuc = baiBao.LinhVucs.Select(p => p.MaLinhVuc).ToList();

            ViewBag.MaCapTapChi = new SelectList(db.CapTapChis, "MaCapTapChi", "TenCapTapChi", baiBao.MaCapTapChi);
            ViewBag.MaLoaiTapChi = new SelectList(db.PhanLoaiTapChis, "MaLoaiTapChi", "TenLoaiTapChi", baiBao.MaLoaiTapChi);
            ViewBag.DSNguoiThamGiaBaiBao = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH", tacgiaphu);
            ViewBag.DSLinhVuc = new MultiSelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc", linhvuc);
            ViewBag.DeTai = new MultiSelectList(db.DeTais, "MaDeTai", "TenDeTai", detaibaibao);

            ViewBag.NguonGoc = new List<SelectListItem> {
                new SelectListItem { Text = "Trong nước" , Value = "true"},
                new SelectListItem { Text = "Ngoài nước" , Value = "false"}
            };
            return View(baiBao);
        }

        // POST: BaiBaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more baibaols see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(List<int> LinhVuc, List<string> DeTaiBaiBao, List<string> DSNguoiThamGiaBaiBao, [Bind(Include = "MaBaiBao,MaISSN,TenBaiBao,LaTrongNuoc,CQXuatBan,MaLoaiTapChi,MaCapTapChi,NamDangBao,TapPhatHanh,SoPhatHanh,TrangBaiBao,LienKetWeb")] BaiBao baiBao, HttpPostedFileBase linkUpload)
        {
            if (ModelState.IsValid)
            {

                var baibao = db.BaiBaos.Where(p => p.MaBaiBao == baiBao.MaBaiBao).Include(p => p.LinhVucs).Include(p => p.DSBaiBaoDeTais).Include(p => p.DSNguoiThamGiaBaiBaos).FirstOrDefault();
                baiBao.LinhVucs = baibao.LinhVucs;
                baiBao.DSBaiBaoDeTais = baibao.DSBaiBaoDeTais;
                baiBao.DSNguoiThamGiaBaiBaos = baibao.DSNguoiThamGiaBaiBaos;
                /* xử lý ảnh upload*/
                if (linkUpload != null && linkUpload.ContentLength > 0)
                {
                    string filename = Path.GetFileNameWithoutExtension(linkUpload.FileName) + "_" + baiBao.MaBaiBao.ToString() + Path.GetExtension(linkUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/App_Data/uploads/baibao_file"), filename);
                    if (!String.IsNullOrEmpty(baibao.LinkFileUpLoad))
                    {
                        string oldpath = Path.Combine(Server.MapPath("~/App_Data/uploads/baibao_file"), baibao.LinkFileUpLoad);
                        if (System.IO.File.Exists(oldpath))
                        {
                            System.IO.File.Delete(oldpath);
                        }
                    }
                    linkUpload.SaveAs(path);
                    baiBao.LinkFileUpLoad = filename;
                }
                else
                {
                    baiBao.LinkFileUpLoad = baibao.LinkFileUpLoad;
                }
                db.BaiBaos.AddOrUpdate(baiBao);
                /* phần xửa lý lĩnh vực*/

                if (LinhVuc != null)
                {

                    var deletedlinhvuc = baibao.LinhVucs.Where(p => !LinhVuc.Contains(p.MaLinhVuc)).ToList();
                    var addedlinhvuc = LinhVuc.Except(baibao.LinhVucs.Select(p => p.MaLinhVuc).ToList<int>());
                    var addlinhvuc = db.LinhVucs.Where(p => addedlinhvuc.Contains(p.MaLinhVuc)).ToList();
                    foreach (var d in deletedlinhvuc)
                    {
                        baibao.LinhVucs.Remove(d);
                    }
                    foreach (var a in addlinhvuc)
                    {
                        baibao.LinhVucs.Add(a);
                    }
                }
                else
                {
                    foreach (var x in baibao.LinhVucs)
                    {
                        baibao.LinhVucs.Remove(x);
                    }
                }


                /* xừ lý người tham gia bài báo*/
                if (DSNguoiThamGiaBaiBao != null)
                {
                    db.DSNguoiThamGiaBaiBaos.Where(p => p.MaBaiBao == baibao.MaBaiBao && p.LaTacGiaChinh == false).ForEach(z => db.DSNguoiThamGiaBaiBaos.Remove(z));
                    foreach (var mankh in DSNguoiThamGiaBaiBao)
                    {
                        DSNguoiThamGiaBaiBao nguoiTGBB = new DSNguoiThamGiaBaiBao
                        {
                            LaTacGiaChinh = false,
                            MaBaiBao = baibao.MaBaiBao,
                            MaNKH = Int32.Parse(mankh)
                        };
                        baibao.DSNguoiThamGiaBaiBaos.Add(nguoiTGBB);
                    }
                }

                /* xử lý đề tài bài báo*/
                if (DeTaiBaiBao != null)
                {
                    db.DSBaiBaoDeTais.Where(p => p.MaBaiBao == baibao.MaBaiBao).ForEach(z => db.DSBaiBaoDeTais.Remove(z));
                    foreach (var madetai in DeTaiBaiBao)
                    {
                        DSBaiBaoDeTai detai = new DSBaiBaoDeTai
                        {
                            MaBaiBao = baiBao.MaBaiBao,
                            MaDeTai = Int32.Parse(madetai)
                        };
                        db.DSBaiBaoDeTais.Add(detai);
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
            var tacgiaphu = db.NhaKhoaHocs.Where(p => p.DSNguoiThamGiaBaiBaos.Any(d => d.MaBaiBao == baiBao.MaBaiBao && d.LaTacGiaChinh == false)).Select(p => p.MaNKH).ToList();
            var detaibaibao = db.DSBaiBaoDeTais.Where(p => p.MaBaiBao == baiBao.MaBaiBao).Select(p => p.MaDeTai).ToList();

            ViewBag.MaCapTapChi = new SelectList(db.CapTapChis, "MaCapTapChi", "TenCapTapChi", baiBao.MaCapTapChi);
            ViewBag.MaLoaiTapChi = new SelectList(db.PhanLoaiTapChis, "MaLoaiTapChi", "TenLoaiTapChi", baiBao.MaLoaiTapChi);
            ViewBag.DSNguoiThamGiaBaiBao = new MultiSelectList(lstAllNKH, "MaNKH", "TenNKH", tacgiaphu);
            ViewBag.LinhVuc = new MultiSelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc", baiBao.LinhVucs);
            ViewBag.DeTai = new MultiSelectList(db.DeTais, "MaDeTai", "TenDeTai", detaibaibao);
            ViewBag.NguonGoc = new List<SelectListItem> {
                new SelectListItem { Text = "Trong nước" , Value = "true"},
                new SelectListItem { Text = "Ngoài nước" , Value = "false"}
            };

            return View(baiBao);
        }

        // GET: BaiBaos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiBao baiBao = await db.BaiBaos.FindAsync(id);
            if (baiBao == null)
            {
                return HttpNotFound();
            }
            return View(baiBao);
        }

        // POST: BaiBaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BaiBao baiBao = await db.BaiBaos.FindAsync(id);
            db.BaiBaos.Remove(baiBao);
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
