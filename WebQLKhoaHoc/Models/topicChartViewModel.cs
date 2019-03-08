using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class topicChartViewModel
    {
        public int MaDVQL { get; set; }
        public int DACNN { get; set; }
        public int DTCB { get; set; }
        public int DTCT { get; set; }
        public int DTCCS { get; set; }
        public int DAHTQT { get; set; }
        public int DASXTNCNN { get; set; }
        public int DTDLCNN { get; set; }
        public int DTTCTDTCNN { get; set; }
        public int DATCTTDCNN { get; set; }
        public int NCCB { get; set; }
        public int NVNCTNDT { get; set; }
        public int GDBVMT { get; set; }
        public int DTTDCB { get; set; }
        public int DASXTNCB { get; set; }
        public int DTCDHH { get; set; }

        public int Sum { get; set; }

        public static topicChartViewModel Mapping(int madvql,int a,int b,int c,int d,int e,int f,int g,int h,int i,int k,int l,int m,int n,int p,int q)
        {
            topicChartViewModel topicvm = new topicChartViewModel();
            topicvm.MaDVQL = madvql;
            topicvm.DACNN = a;
            topicvm.DTCB = b;
            topicvm.DTCT = c;
            topicvm.DTCCS = d;
            topicvm.DAHTQT = e;
            topicvm.DASXTNCNN = f;
            topicvm.DTDLCNN = g;
            topicvm.DTTCTDTCNN = h;
            topicvm.DATCTTDCNN = i;
            topicvm.NCCB = k;
            topicvm.NVNCTNDT = l;
            topicvm.GDBVMT = m;
            topicvm.DTTDCB = n;
            topicvm.DASXTNCB = p;
            topicvm.DTCDHH = q;
            topicvm.Sum = a + b + c + d + e + f + g + h + i + k + l + m + n + p + q;
            return topicvm; 
        }
    }
}