using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;

[assembly: OwinStartup(typeof(Case518Sample150411.Startup))]

namespace Case518Sample150411
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 如需如何設定應用程式的詳細資訊，請參閱  http://go.microsoft.com/fwlink/?LinkID=316888
            app.SetDefaultSignInAsAuthenticationType("ApplicationCookie");

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/#/auth/login")
            });

            

            app.UseFacebookAuthentication(new FacebookAuthenticationOptions
            {
                AppId = "530981250373549",
                AppSecret = "2ccff332099d2a2679ba1aa3c8968d84"
            });
        }
    }
}
