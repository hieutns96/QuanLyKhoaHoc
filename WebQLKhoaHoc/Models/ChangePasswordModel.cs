using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Mời Bạn Nhập Mật Khẩu Cũ")]
        [StringLength(255, ErrorMessage = "Mật Khẩu Có Ít Nhất 8 Kí Tự", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "Mời Bạn Nhập Mật Khẩu Mới")]
        [StringLength(255, ErrorMessage = "Mật Khẩu Có Ít Nhất 8 Kí Tự", MinimumLength = 8)]
        [RegularExpression("/(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8}/g",
            ErrorMessage = "Mật Khẩu Phải Có Ít Nhất 1 Kí Tự Thường,1 Kí Tự In Hoa Và 1 Chữ Số")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác Nhận Mật Khẩu")]
        [StringLength(255, ErrorMessage = "Mật Khẩu Có Ít Nhất 8 Kí Tự", MinimumLength = 8)]
        [RegularExpression("/(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8}/g",
            ErrorMessage = "Mật Khẩu Phải Có Ít Nhất 1 Kí Tự Thường,1 Kí Tự In Hoa Và 1 Chữ Số")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Mật Khẩu Không Khớp" ) , ]
        public string ConfirmPassword { get; set; }
    }
}