using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class topicdata
    {
        public int MaDVQL { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }

        public static topicdata Mapping(int madvql,DateTime fd,DateTime td)
        {
            topicdata topicdata = new topicdata();
            topicdata.MaDVQL = madvql;
            topicdata.fromdate = fd;
            topicdata.todate = td;

            return topicdata;
        }
    }
}