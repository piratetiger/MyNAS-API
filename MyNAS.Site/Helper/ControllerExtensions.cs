using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerExtensions
    {
        public static IList<string> GetServiceFilterOrder(this ControllerBase controller)
        {
            List<string> result = null;
            if (controller.HttpContext.Items.ContainsKey("ServiceFilterOrder"))
            {
                result = (controller.HttpContext.Items["ServiceFilterOrder"] as string[]).ToList();
            }

            return result;
        }
    }
}