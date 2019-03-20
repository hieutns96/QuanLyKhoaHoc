using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace WebQLKhoaHoc.Models
{
    public class CustomizeAuthorize : AuthorizeAttribute
    {
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            UserLoginViewModel user = (UserLoginViewModel)HttpContext.Current.Session["user"];
            if (user == null)
            {
                return false;
            }


            if ((this.Roles.Length > 0) && (!this.Roles.Contains(user.Machucnang.ToString())))
            {
                return false;
            }
            return true;
        }

        



    }
}