using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(n01629153_Event_Management.Startup))]
namespace n01629153_Event_Management
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
