using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SensenbrennerHospital.Startup))]
namespace SensenbrennerHospital
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
