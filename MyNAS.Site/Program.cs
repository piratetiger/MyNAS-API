using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace MyNAS.Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel(options =>
                    {
                        // options.ListenLocalhost(5443, listenOptions =>
                        // {
                        //     listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        //     listenOptions.UseHttps("my-nas.pfx", "mynasPWD");
                        // });
                        options.Listen(IPAddress.Any, 5000);
                    })
                // .UseKestrel(c => c.Listen(new System.Net.IPAddress(new byte[] { 0, 0, 0, 0 }), 5000))
                .UseStartup<Startup>()
                .UseNLog();
        }
    }
}
