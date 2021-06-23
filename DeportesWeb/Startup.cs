using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeportesWeb.Startup))]
namespace DeportesWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
