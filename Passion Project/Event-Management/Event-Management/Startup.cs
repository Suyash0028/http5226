using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Event_Management.Startup))]
namespace Event_Management
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
