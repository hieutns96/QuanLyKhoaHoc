using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoaHoc.Models
{
    public class ImageViewModel
    {
        public string Image;

        public ImageViewModel(byte[] input)
        {
            Image = input != null ? string.Format("data:image/jpeg;base64,{0}", Convert.ToBase64String(input)) : String.Empty;
        }
    }
}