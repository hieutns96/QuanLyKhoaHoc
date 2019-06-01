using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using WebQLKhoaHoc.Models;

namespace WebQLKhoaHoc.Controllers
{
    public class AccountController : Controller
    {
        QLKhoaHocEntities db = new QLKhoaHocEntities();
        // GET: /Account/Login

        public ActionResult Login()
        {
            if (Session["user"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        public ActionResult AdminLogin()
        {
            if (Session["user"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            if (Session["user"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (model.Email != null && model.Password != "")
            {

                NguoiDung nguoiDung = db.NguoiDungs.SingleOrDefault(p => p.Usernames == model.Email);
                NhaKhoaHoc checknkh = db.NhaKhoaHocs.Where(p => p.EmailLienHe == model.Email).FirstOrDefault();
                if (nguoiDung != null || checknkh != null)
                {
                    ViewBag.Error = "Tài khoản đã tồn tại";
                    return View();
                }
                NhaKhoaHoc nkh = new NhaKhoaHoc();
                nkh.HoNKH = model.Ho;
                nkh.TenNKH = model.Ten;
                nkh.EmailLienHe = model.Email;
                db.NhaKhoaHocs.Add(nkh);
                db.SaveChanges();

                string salt = "".GenRandomKey(); //update by Khiet
                NguoiDung newuser = new NguoiDung
                {
                    MaNKH = nkh.MaNKH,
                    Usernames = nkh.EmailLienHe,
                    Passwords = Encryptor.MD5Hash(model.Password + salt), //update by Khiet
                    MaChucNang = 2,
                    IsActive = true,
                    RandomKey = salt //update by Khiet
                };
                db.NguoiDungs.Add(newuser);
                db.SaveChanges();

                return RedirectToAction("Login", "Account");

            }
            else
            {
                ViewBag.Error = "Mời bạn điền đầy đủ thông tin";
                return View();
            }
        }


        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                UserLoginViewModel user = (UserLoginViewModel) Session["user"];
                NguoiDung nguoiDung = db.NguoiDungs.SingleOrDefault(p => p.Usernames == user.UserName && p.IsActive == true);
       
                if (Encryptor.GetHashString(nguoiDung.Passwords) != Encryptor.GetHashString(Encryptor.MD5Hash(model.OldPassword+ nguoiDung.RandomKey)))
                {
                    ModelState.AddModelError("OldPassword", "Mật Khẩu không chính xác");
                    return View(model);
                }

                string salt = "".GenRandomKey();
                nguoiDung.RandomKey = salt;
                nguoiDung.Passwords = Encryptor.MD5Hash(model.Password+salt);
                db.Entry(nguoiDung).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Session.Remove("user");
                return RedirectToAction("Login");
            }

            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(string username,string password)
        {
            if(username != null && username != "" && password != null && password != "")
            {

                NguoiDung nguoiDung = db.NguoiDungs.SingleOrDefault(p => p.Usernames == username && p.IsActive ==true);

                if (nguoiDung == null)
                {
                    ViewBag.Error = "Tài khoản không đúng hoặc chưa được kích hoạt!";
                    return View();
                }
                
                if(Encryptor.GetHashString(nguoiDung.Passwords) == Encryptor.GetHashString( Encryptor.MD5Hash(password + nguoiDung.RandomKey)) )
                {
                    //cap nhat lastlogin //update by Khiet
                    nguoiDung.LastLogin = DateTime.Now;
                    db.SaveChanges();
                
                    NhaKhoaHoc nhaKhoaHoc = db.NhaKhoaHocs.Find(nguoiDung.MaNKH);
                    string hovaten = nhaKhoaHoc.HoNKH + " " + nhaKhoaHoc.TenNKH;
                    string anhdaidien = nhaKhoaHoc.AnhCaNhan != null ? string.Format("data:image/jpeg;base64,{0}", Convert.ToBase64String(nhaKhoaHoc.AnhCaNhan)) : String.Empty; ;
                    
                    // lấy tên viết tắt
                    string tenhocham = "";
                    if (db.HocHams.Find(nhaKhoaHoc.MaHocHam) != null)
                    {
                        tenhocham = db.HocHams.Find(nhaKhoaHoc.MaHocHam).TenVietTat;
                    }
                    string tenhocvi = "";
                    if (db.HocVis.Find(nhaKhoaHoc.MaHocVi) != null)
                    {
                        tenhocvi = db.HocVis.Find(nhaKhoaHoc.MaHocVi).TenVietTat;
                    }
                    string chucvi ="";
                    if (tenhocham != "" && tenhocvi != "")
                    {
                        chucvi = tenhocham + "." + tenhocvi;
                    }
                    else if(tenhocham != "" && tenhocvi == ""){
                        chucvi = tenhocham + " - ";
                    }
                    else{
                        chucvi = tenhocvi + " - ";
                    }

                    int machucnang = Convert.ToInt16(nguoiDung.MaChucNang);
                    UserLoginViewModel user = UserLoginViewModel.Mapping(nhaKhoaHoc.TenNKH, nhaKhoaHoc.MaNKH, hovaten, anhdaidien, chucvi, machucnang,username);

                    Session["user"] = user;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Tài khoản mật khẩu không đúng";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "Tài khoản và mật khẩu không thể bỏ trống";
                return View();
            }
        }
        [HttpPost]
        public ActionResult AdminLogin(string username, string password)
        {
            if (username != null && username != "" && password != null && password != "")
            {

                NguoiDung nguoiDung = db.NguoiDungs.SingleOrDefault(p => p.Usernames == username && p.IsActive == true && p.MaChucNang == 1);

                if (nguoiDung == null)
                {
                    ViewBag.Error = "Tài khoản không đúng hoặc không phải Admin!";
                    return View();
                }

                if (Encryptor.GetHashString(nguoiDung.Passwords) == Encryptor.GetHashString(Encryptor.MD5Hash(password + nguoiDung.RandomKey)))
                {
                    //cap nhat lastlogin //update by Khiet
                    nguoiDung.LastLogin = DateTime.Now;
                    db.SaveChanges();

                    NhaKhoaHoc nhaKhoaHoc = db.NhaKhoaHocs.Find(nguoiDung.MaNKH);
                    string hovaten = nhaKhoaHoc.HoNKH + " " + nhaKhoaHoc.TenNKH;
                    string anhdaidien = nhaKhoaHoc.AnhCaNhan != null ? string.Format("data:image/jpeg;base64,{0}", Convert.ToBase64String(nhaKhoaHoc.AnhCaNhan)) : String.Empty; ;

                    // lấy tên viết tắt
                    string tenhocham = "";
                    if (db.HocHams.Find(nhaKhoaHoc.MaHocHam) != null)
                    {
                        tenhocham = db.HocHams.Find(nhaKhoaHoc.MaHocHam).TenVietTat;
                    }
                    string tenhocvi = "";
                    if (db.HocVis.Find(nhaKhoaHoc.MaHocVi) != null)
                    {
                        tenhocvi = db.HocVis.Find(nhaKhoaHoc.MaHocVi).TenVietTat;
                    }
                    string chucvi = "";
                    if (tenhocham != "" && tenhocvi != "")
                    {
                        chucvi = tenhocham + "." + tenhocvi;
                    }
                    else if (tenhocham != "" && tenhocvi == "")
                    {
                        chucvi = tenhocham + " - ";
                    }
                    else
                    {
                        chucvi = tenhocvi + " - ";
                    }

                    int machucnang = Convert.ToInt16(nguoiDung.MaChucNang);
                    UserLoginViewModel user = UserLoginViewModel.Mapping(nhaKhoaHoc.TenNKH, nhaKhoaHoc.MaNKH, hovaten, anhdaidien, chucvi, machucnang, username);

                    Session["user"] = user;
                    return RedirectToAction("Index", "AdminNhaKhoaHoc");
                }
                else
                {
                    ViewBag.Error = "Tài khoản mật khẩu không đúng";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "Tài khoản và mật khẩu không thể bỏ trống";
                return View();
            }
        }
        public ActionResult LogOff()
        {
            if(Session["user"] != null)
            {
                Session.Remove("user");
            }
            return RedirectToAction("Login", "Account");
        }
        public ActionResult AdminLogOff()
        {
            if (Session["user"] != null)
            {
                Session.Remove("user");
            }
            return RedirectToAction("AdminLogin", "Account");
        }
    }
}