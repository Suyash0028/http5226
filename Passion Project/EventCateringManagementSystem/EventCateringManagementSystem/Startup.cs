using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventCateringManagementSystem.Startup))]
namespace EventCateringManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
