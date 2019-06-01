using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebQLKhoaHoc.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult NotFound()
        {
           Response.StatusCode = 404;  //you may want to set this to 200
            var error = new HandleErrorInfo(new Exception("Trang không tồn tại"), "ErrorController","NotFound");
            return View(error);
        }
    }
}