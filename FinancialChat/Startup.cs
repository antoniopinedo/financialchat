using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinancialChat.Startup))]
namespace FinancialChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
