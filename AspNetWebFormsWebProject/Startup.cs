using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AspNetWebFormsWebProject.Startup))]
namespace AspNetWebFormsWebProject
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
