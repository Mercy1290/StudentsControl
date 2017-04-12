using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentsControl.Startup))]
namespace StudentsControl
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
