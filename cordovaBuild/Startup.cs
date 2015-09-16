using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cordovaBuild.Startup))]
namespace cordovaBuild
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
