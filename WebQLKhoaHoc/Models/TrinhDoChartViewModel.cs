using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace WebQLKhoaHoc.Models
{
    public class TrinhDoChartViewModel
    {
        public string TrinhDo { get; set; }
        public int SoLuong { get; set; }

        public static TrinhDoChartViewModel Mapping(string trinhDo,int soLuong)
        {
            TrinhDoChartViewModel trinhdovm = new TrinhDoChartViewModel();
            trinhdovm.TrinhDo = trinhDo;
            trinhdovm.SoLuong = soLuong;
            return trinhdovm;
        }
    }
}