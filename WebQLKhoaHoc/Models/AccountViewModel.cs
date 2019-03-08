using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class AccountViewModel
    {
        class LoginModel
        {
            [Display(Name ="Username")]
            [Required]
            public string Username { get; set; }
            [Display(Name = "Password")]
            [Required]
            public string Password { get; set; }
        }
    }
}