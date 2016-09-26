using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleAspNetMvcWebProject.Startup))]
namespace SampleAspNetMvcWebProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
