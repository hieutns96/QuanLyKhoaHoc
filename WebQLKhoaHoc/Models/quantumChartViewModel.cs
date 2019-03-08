using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class quantumChartViewModel
    {
        public int MaDVQL { get; set; }
        public int GV { get; set; }
        public int GVC { get; set; }
        public int GVCC { get; set; }
        public int GVTH { get; set; }
        public int CV { get; set; }
        public int CVC { get; set; }
        public int TVV { get; set; }
        public int KTV { get; set; }
        public int KTVCC { get; set; }
        public int NCV { get; set; }
        public int KTOAN { get; set; }
        public int KTVC { get; set; }
        public int NVVT { get; set; }
        public int CS { get; set; }

        public static quantumChartViewModel Mapping(int MaDonVi, int nGV, int nGVC, int nGVCC, int nGVTH, int nCV, int nCVC, int nTVV, int nKTV, int nKTVCC, int nNCV, int nKTOAN, int nKTVC, int nNVVT, int nCS)
        {
            quantumChartViewModel quantumvm = new quantumChartViewModel();
            quantumvm.MaDVQL = MaDonVi;
            quantumvm.GV = nGV;
            quantumvm.GVC = nGVC;
            quantumvm.GVCC = nGVCC;
            quantumvm.GVTH = nGVTH;
            quantumvm.CV = nCV;
            quantumvm.CVC = nCVC;
            quantumvm.TVV = nTVV;
            quantumvm.KTV = nKTV;
            quantumvm.KTVCC = nKTVCC;
            quantumvm.NCV = nNCV;
            quantumvm.KTOAN = nKTOAN;
            quantumvm.KTVC = nKTVC;
            quantumvm.NVVT = nNVVT;
            quantumvm.CS = nCS;

            return quantumvm;
        }
    }
}