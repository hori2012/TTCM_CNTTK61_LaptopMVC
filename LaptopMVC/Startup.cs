using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LaptopMVC.Startup))]
namespace LaptopMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
