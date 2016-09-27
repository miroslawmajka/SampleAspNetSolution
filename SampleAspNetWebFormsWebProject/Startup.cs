using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleAspNetWebFormsWebProject.Startup))]
namespace SampleAspNetWebFormsWebProject
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
