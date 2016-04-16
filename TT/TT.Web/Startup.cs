using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TT.Web.Startup))]
namespace TT.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
