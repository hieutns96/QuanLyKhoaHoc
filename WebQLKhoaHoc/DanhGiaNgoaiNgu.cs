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
    
    public partial class DanhGiaNgoaiNgu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DanhGiaNgoaiNgu()
        {
            this.NgoaiNguNKHs = new HashSet<NgoaiNguNKH>();
            this.NgoaiNguNKHs1 = new HashSet<NgoaiNguNKH>();
            this.NgoaiNguNKHs2 = new HashSet<NgoaiNguNKH>();
        }
    
        public int MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string GhiChu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NgoaiNguNKH> NgoaiNguNKHs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NgoaiNguNKH> NgoaiNguNKHs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NgoaiNguNKH> NgoaiNguNKHs2 { get; set; }
    }
}
