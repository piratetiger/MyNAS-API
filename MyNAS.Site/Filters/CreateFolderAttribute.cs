using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyNAS.Site
{
    public class CreateFolderAttribute : ActionFilterAttribute
    {
        private readonly IWebHostEnvironment _host;
        private string _folder;

        public string Folder { get; set; }

        public CreateFolderAttribute(IWebHostEnvironment host, string folder)
        {
            _host = host;
            _folder = folder;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var path = Path.Combine(_host.WebRootPath, _folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}