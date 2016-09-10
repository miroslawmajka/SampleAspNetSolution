using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleVideoStreamingSite.Startup))]
namespace SampleVideoStreamingSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
