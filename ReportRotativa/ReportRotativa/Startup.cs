using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoneyReport.Startup))]
namespace MoneyReport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
