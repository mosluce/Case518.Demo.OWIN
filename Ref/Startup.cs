using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ref.Startup))]
namespace Ref
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
