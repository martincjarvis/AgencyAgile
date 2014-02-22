using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AgencyAgile.Web.Startup))]
namespace AgencyAgile.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
