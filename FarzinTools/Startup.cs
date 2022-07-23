using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FarzinTools.Startup))]
namespace FarzinTools
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
