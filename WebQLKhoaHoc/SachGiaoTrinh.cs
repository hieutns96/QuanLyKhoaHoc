//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebQLKhoaHoc
{
    using System;
    using System.Collections.Generic;
    
    public partial class SachGiaoTrinh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SachGiaoTrinh()
        {
            this.DSTacGias = new HashSet<DSTacGia>();
        }
    
        public int MaSach { get; set; }
        public string MaISBN { get; set; }
        public string TenSach { get; set; }
        public Nullable<int> MaLoai { get; set; }
        public Nullable<int> MaLinhVuc { get; set; }
        public Nullable<int> MaNXB { get; set; }
        public Nullable<System.DateTime> NamXuatBan { get; set; }
        public string Mota { get; set; }
        public byte[] AnhBia1 { get; set; }
        public byte[] AnhBia4 { get; set; }
        public byte[] AnhBiaISBN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSTacGia> DSTacGias { get; set; }
        public virtual LinhVuc LinhVuc { get; set; }
        public virtual NhaXuatBan NhaXuatBan { get; set; }
        public virtual PhanLoaiSach PhanLoaiSach { get; set; }
    }
}
