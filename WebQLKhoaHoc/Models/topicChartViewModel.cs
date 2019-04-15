using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class topicChartViewModel
    {
        public List<object> Headers {get;set;}
        public List<object> Rows { get; set; }

       public topicChartViewModel()
        {
           
        }
    }
}