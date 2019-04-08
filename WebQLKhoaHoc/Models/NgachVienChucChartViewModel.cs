using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class NgachVienChucChartViewModel
    {
        public string TenNgach { get; set; }
        public int SoLuong { get; set; }


        public static NgachVienChucChartViewModel Mapping(string tenNgach, int soLuong)
        {
            NgachVienChucChartViewModel ngachvm = new NgachVienChucChartViewModel();
            ngachvm.TenNgach = tenNgach;
            ngachvm.SoLuong = soLuong;
            return ngachvm;
        }
    }
}