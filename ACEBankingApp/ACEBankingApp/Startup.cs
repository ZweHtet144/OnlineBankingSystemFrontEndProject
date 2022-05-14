using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ACEBankingApp.Startup))]
namespace ACEBankingApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
