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
    
    public partial class BaiBao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaiBao()
        {
            this.DSBaiBaoDeTais = new HashSet<DSBaiBaoDeTai>();
            this.DSNguoiThamGiaBaiBaos = new HashSet<DSNguoiThamGiaBaiBao>();
            this.LinhVucs = new HashSet<LinhVuc>();
        }
    
        public int MaBaiBao { get; set; }
        public string MaISSN { get; set; }
        public string TenBaiBao { get; set; }
        public Nullable<bool> LaTrongNuoc { get; set; }
        public string CQXuatBan { get; set; }
        public Nullable<int> MaLoaiTapChi { get; set; }
        public Nullable<int> MaCapTapChi { get; set; }
        public Nullable<System.DateTime> NamDangBao { get; set; }
        public string TapPhatHanh { get; set; }
        public string SoPhatHanh { get; set; }
        public string TrangBaiBao { get; set; }
        public string LienKetWeb { get; set; }
        public string LinkFileUpLoad { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSBaiBaoDeTai> DSBaiBaoDeTais { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSNguoiThamGiaBaiBao> DSNguoiThamGiaBaiBaos { get; set; }
        public virtual CapTapChi CapTapChi { get; set; }
        public virtual PhanLoaiTapChi PhanLoaiTapChi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LinhVuc> LinhVucs { get; set; }
    }
}
