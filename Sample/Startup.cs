using System;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;

[assembly: OwinStartup(typeof(Sample.Startup))]

namespace Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 如需如何設定應用程式的詳細資訊，請參閱  http://go.microsoft.com/fwlink/?LinkID=316888

            var opt = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            };
            app.UseCookieAuthentication(opt);
            app.SetDefaultSignInAsAuthenticationType(opt.AuthenticationType);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            var fbOpt = new FacebookAuthenticationOptions
            {
                AppId = System.Configuration.ConfigurationManager.AppSettings["FacebookAppId"],
                AppSecret = System.Configuration.ConfigurationManager.AppSettings["FacebookAppSecret"]
            };

            fbOpt.Scope.Add("email");
            fbOpt.Scope.Add("user_birthday");

            app.UseFacebookAuthentication(fbOpt);
        }
    }
}
