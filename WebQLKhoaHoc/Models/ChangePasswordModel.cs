using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebQLKhoaHoc.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Mời Bạn Nhập Mật Khẩu Cũ")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "Mời Bạn Nhập Mật Khẩu Mới")]
        [MembershipPassword(
            MinRequiredPasswordLength = 8,
            ErrorMessage = "Mật Khẩu Có Ít Nhất 8 Kí Tự",
            MinRequiredNonAlphanumericCharacters = 1,
            MinNonAlphanumericCharactersError = "Mật Khẩu Phải Có Ít Nhất 1 Ký Tự Đặc Biệt")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác Nhận Mật Khẩu")]
        [MembershipPassword(
            MinRequiredPasswordLength = 8,
            ErrorMessage = "Mật Khẩu Có Ít Nhất 8 Kí Tự",
            MinRequiredNonAlphanumericCharacters = 1,
            MinNonAlphanumericCharactersError = "Mật Khẩu Phải Có Ít Nhất 1 Ký Tự Đặc Biệt")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Mật Khẩu Không Khớp" ) , ]
        public string ConfirmPassword { get; set; }
    }
}