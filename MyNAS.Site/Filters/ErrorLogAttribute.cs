using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace MyNAS.Site
{
    public class ErrorLogAttribute : ExceptionFilterAttribute
    {
        protected Logger Logger
        {
            get
            {
                var factory = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config");
                return factory.GetCurrentClassLogger();
            }
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                Logger.Properties["stackTrace"] = context.Exception.StackTrace;
                Logger.Log(LogLevel.Error, context.Exception.Message);
            }

            base.OnException(context);
        }
    }
}