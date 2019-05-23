using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class SignUpModel
    {
        [Display(Name = "Họ")]
        [Required]
        public string Ho { get; set; }
        [Display(Name = "Tên")]
        [Required]
        public string Ten { get; set; }
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }
    }

    public class NguoiDungViewModel
    {
        public string MaChucNang { get; set; }
     
        public string Username { get; set; }
      
        public string Password { get; set; }
    }
}