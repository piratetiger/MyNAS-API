using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using MyNAS.Model.User;

namespace MyNAS.Site.Helper
{
    public static class ExtensionsHelper
    {
        public static AuthenticationBuilder AddMyNASAuth(this AuthenticationBuilder builder, Action<MyNASAuthOptions> configOptions)
        {
            return builder.AddScheme<MyNASAuthOptions, MyNASAuthHandler>(MyNASAuthOptions.DefaultScheme, configOptions);
        }

        public static string GetUserAgent(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return null;
            }

            return httpContext.Request.Headers["User-Agent"];
        }

        public static UserModel GetUser(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return null;
            }

            return (UserModel)httpContext.Items["User"];
        }
    }
}