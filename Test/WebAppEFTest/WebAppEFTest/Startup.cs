using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppEFTest.Startup))]
namespace WebAppEFTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
