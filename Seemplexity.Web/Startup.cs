using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Seemplexity.Web.Startup))]
namespace Seemplexity.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
