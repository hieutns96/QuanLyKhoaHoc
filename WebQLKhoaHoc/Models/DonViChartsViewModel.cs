using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class DonViChartsViewModel
    {
        public List<Object> Header { get; set; } = new List<Object>();
        public List<List<Object>> Rows { get; set; } = new List<List<Object>>();
    }
}