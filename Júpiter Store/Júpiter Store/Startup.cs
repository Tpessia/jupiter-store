using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Júpiter_Store.Startup))]
namespace Júpiter_Store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
