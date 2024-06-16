using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;

[assembly: OwinStartup(typeof(CostManager.Startup))]
namespace CostManager
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;

            app.SetDefaultSignInAsAuthenticationType("Cookies");
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromHours(9)
            });

            app.Use((context, next) =>
            {
                if (context.Response.StatusCode == 403 || context.Response.StatusCode == 401) // Acesso negado (Forbidden)
                {
                    context.Response.Redirect("~/Erro/AccessDenied");
                }

                return next.Invoke();
            });
        }
    }
}