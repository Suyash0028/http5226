using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(n01629153_EventManagement.Startup))]
namespace n01629153_EventManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
