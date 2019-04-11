using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class DonViChartsViewModel
    {
        public List<Object> Header { get; set; } 
        public List<List<Object>> Rows { get; set; } 

        public DonViChartsViewModel()
        {
            this.Header = new List<Object>();
            this.Rows = new List<List<Object>>();
        }
    }
}