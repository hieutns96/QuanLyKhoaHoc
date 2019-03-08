using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class UserLoginViewModel
    {
        public int MaNKH { get; set; }
        public string Ten { get; set; }
        public string HoVaTen { get; set; }
        public string AnhCaNhan { get; set; }
        public string ChucVi { get; set; }
        public int machucnang { get; set; }
        public static UserLoginViewModel Mapping(string ten,int mankh,string hovaten,string anhcanhan,string chucvi,int machucnang)
        {
            UserLoginViewModel res = new UserLoginViewModel();
            res.Ten = ten;
            res.MaNKH = mankh;
            res.HoVaTen = hovaten ?? String.Empty;
            res.AnhCaNhan = anhcanhan ?? String.Empty;
            res.ChucVi = chucvi ?? String.Empty;
            res.machucnang = machucnang;

            return res;
        }
    }
}