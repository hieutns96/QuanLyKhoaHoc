﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class DonViTablesViewModel
    {
        public List<Object> Header { get; set; } 
        public List<List<Object>> Rows { get; set; }

         public DonViTablesViewModel()
        {
            this.Header = new List<Object>();
            this.Rows = new List<List<Object>>();
        }
    }

    //public static DonViTablesViewModel Mapping(List<Object> header, List<List<Object>> rows)
    //{
    //    DonViTablesViewModel model = new DonViTablesViewModel();
    //    foreach (var item in header)
    //    {
    //        model.Header.Add(item);
    //    }
    //    foreach (var item in rows)
    //    {
    //        model.Rows.Add(item);
    //    }
    //    return model;
    //}

}