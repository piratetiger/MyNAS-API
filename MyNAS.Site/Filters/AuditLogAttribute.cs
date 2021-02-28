using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace MyNAS.Site
{
    public class AuditLogAttribute : ActionFilterAttribute
    {
        protected Logger Logger
        {
            get
            {
                var factory = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config");
                return factory.GetCurrentClassLogger();
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Logger.Log(LogLevel.Info, context.HttpContext.User.Identity.Name);
            base.OnActionExecuting(context);
        }
    }
}