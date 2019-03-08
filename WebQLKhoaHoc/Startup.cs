using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebQLKhoaHoc.Startup))]
namespace WebQLKhoaHoc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
