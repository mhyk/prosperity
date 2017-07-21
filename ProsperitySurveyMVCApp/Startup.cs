using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProsperitySurveyMVCApp.Startup))]
namespace ProsperitySurveyMVCApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
