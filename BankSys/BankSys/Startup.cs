using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BankSys.Startup))]
namespace BankSys
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
