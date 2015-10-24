using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaskMgtApi.Startup))]
namespace TaskMgtApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
