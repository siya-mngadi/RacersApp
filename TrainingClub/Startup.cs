using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrainingClub.Startup))]
namespace TrainingClub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
