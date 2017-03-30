using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_MyHotel_example.Startup))]
namespace MVC_MyHotel_example
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
