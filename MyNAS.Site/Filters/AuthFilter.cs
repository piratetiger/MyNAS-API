using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNAS.Model.User;
using MyNAS.Service;
using MyNAS.Site.Helper;

namespace MyNAS.Site
{
    public class MyNASAuthHandler : AuthenticationHandler<MyNASAuthOptions>
    {
        private readonly HttpContext _httpContext;

        public MyNASAuthHandler(IOptionsMonitor<MyNASAuthOptions> options,
                                ILoggerFactory logger,
                                UrlEncoder encoder,
                                ISystemClock clock,
                                IHttpContextAccessor httpContext) : base(options, logger, encoder, clock)
        {
            _httpContext = httpContext.HttpContext;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var service = new UserService();
                var token = _httpContext.Request.Headers["x-login-token"];
                var userName = _httpContext.Request.Headers["x-login-user"];
                var hostInfo = _httpContext.GetUserAgent();
                var user = new UserModel();
                user.UserName = userName;
                user.Token = token;
                user.HostInfo = hostInfo;
                var result = service.ValidateUser(user);
                if (result)
                {
                    _httpContext.Items.Add("User", user);
                    var claim = new[]{
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };
                    var identity = new ClaimsIdentity(claim, MyNASAuthOptions.DefaultType);
                    var principal = new ClaimsPrincipal(identity);
                    var ticker = new AuthenticationTicket(principal, MyNASAuthOptions.DefaultScheme);
                    return Task.FromResult(AuthenticateResult.Success(ticker));
                }
                else
                {
                    return Task.FromResult(AuthenticateResult.Fail("Authentication Failed."));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail(ex.Message));
            }
        }
    }

    public class MyNASAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "MyNASAuthScheme";
        public const string DefaultType = "MyNASAuthType";
    }
}