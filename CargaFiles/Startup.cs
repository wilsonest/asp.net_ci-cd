using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CargaFiles.StartupOwin))]

namespace CargaFiles
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            //AuthStartup.ConfigureAuth(app);
        }
    }
}
