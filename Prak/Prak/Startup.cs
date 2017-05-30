using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Prak.Startup))]
namespace Prak
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
