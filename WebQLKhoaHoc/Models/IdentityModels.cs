using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace WebQLKhoaHoc.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int MaNKH { get; set; }
        public virtual ICollection<NhaKhoaHoc> NhaKhoaHoc { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("QLKhoaHocEntities")
        {
        }
    }
}