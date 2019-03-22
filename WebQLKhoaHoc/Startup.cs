using Microsoft.Owin;
using Owin;
using Serilog;

[assembly: OwinStartupAttribute(typeof(WebQLKhoaHoc.Startup))]
namespace WebQLKhoaHoc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/log-.txt"),rollingInterval: RollingInterval.Day)
                .CreateLogger();

        }
    }
}
