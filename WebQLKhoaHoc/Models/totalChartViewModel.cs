using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
namespace WebQLKhoaHoc.Models
{
    public class totalChartViewModel
    {
        public int MaDVQL { get; set; }
        public int GSDD { get; set; }
        public int GS { get; set; }
        public int PGS { get; set; }
        public int TSKH { get; set; }
        public int TS { get; set; }
        public int ThS { get; set; }
        public int GV { get; set; }
        public int NCV { get; set; }

        public static totalChartViewModel Mapping(int madvql,int gsdd,int gs,int pgs,int tskh,int ts,int ths,int gv,int ncv)
        {
            totalChartViewModel totalvm = new totalChartViewModel();
            totalvm.MaDVQL = madvql;
            totalvm.GSDD = gsdd;
            totalvm.GS = gs;
            totalvm.PGS = pgs;
            totalvm.TSKH = tskh;
            totalvm.TS = ts;
            totalvm.ThS = ths;
            totalvm.GV = gv;
            totalvm.NCV = ncv;

            return totalvm;
        }
    }
}