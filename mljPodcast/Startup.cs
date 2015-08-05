using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mljPodcast.Startup))]
namespace mljPodcast
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
