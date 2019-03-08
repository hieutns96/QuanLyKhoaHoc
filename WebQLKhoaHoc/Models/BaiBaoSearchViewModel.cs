using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class BaiBaoSearchViewModel
    {
        public string MaLinhVuc { get; set; }
        public string MaLoaiTapChi { get; set; }
        public string CQXuatBan { get; set; }
        public string MaPhanLoaiTapChi { get; set; }
        public string MaCapTapChi { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string SearchValue { get; set; }
    }
}