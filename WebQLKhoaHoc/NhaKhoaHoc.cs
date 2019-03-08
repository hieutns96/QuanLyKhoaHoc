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
    
    public partial class NhaKhoaHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaKhoaHoc()
        {
            this.ChuyenMonNKHs = new HashSet<ChuyenMonNKH>();
            this.DSNguoiThamGiaBaiBaos = new HashSet<DSNguoiThamGiaBaiBao>();
            this.DSNguoiThamGiaDeTais = new HashSet<DSNguoiThamGiaDeTai>();
            this.DSTacGias = new HashSet<DSTacGia>();
            this.NguoiDungs = new HashSet<NguoiDung>();
            this.QuaTrinhCongTacs = new HashSet<QuaTrinhCongTac>();
            this.QuaTrinhDaoTaos = new HashSet<QuaTrinhDaoTao>();
            this.LinhVucs = new HashSet<LinhVuc>();
            this.TrinhDoNgoaiNgus = new HashSet<TrinhDoNgoaiNgu>();
        }
    
        public int MaNKH { get; set; }
        public string MaNKHHoSo { get; set; }
        public string HoNKH { get; set; }
        public string TenNKH { get; set; }
        public string GioiTinhNKH { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string DiaChiLienHe { get; set; }
        public string DienThoai { get; set; }
        public string EmailLienHe { get; set; }
        public Nullable<int> MaHocHam { get; set; }
        public Nullable<int> MaHocVi { get; set; }
        public Nullable<int> MaCNDaoTao { get; set; }
        public Nullable<int> MaDonViQL { get; set; }
        public string AnhDaiDien { get; set; }
        public byte[] AnhCaNhan { get; set; }
        public Nullable<int> MaNgachVienChuc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChuyenMonNKH> ChuyenMonNKHs { get; set; }
        public virtual ChuyenNganh ChuyenNganh { get; set; }
        public virtual DonViQL DonViQL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSNguoiThamGiaBaiBao> DSNguoiThamGiaBaiBaos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSNguoiThamGiaDeTai> DSNguoiThamGiaDeTais { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSTacGia> DSTacGias { get; set; }
        public virtual HocHam HocHam { get; set; }
        public virtual HocVi HocVi { get; set; }
        public virtual NgachVienChuc NgachVienChuc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuaTrinhCongTac> QuaTrinhCongTacs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuaTrinhDaoTao> QuaTrinhDaoTaos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LinhVuc> LinhVucs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrinhDoNgoaiNgu> TrinhDoNgoaiNgus { get; set; }
    }
}
