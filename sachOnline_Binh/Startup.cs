using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sachOnline_Binh.Startup))]
namespace sachOnline_Binh
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
