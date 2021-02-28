using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyNAS.Model.User;
using MyNAS.Site.BackendServices;
using MyNAS.Site.Helper;

namespace MyNAS.Site
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ITorrentDownloadService, TorrentDownloadService>();
            services.AddHostedService<TorrentDownloadService>();

            services.AddControllers(options =>
                {
                    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
                    options.Filters.Add(new AuditLogAttribute());
                    options.Filters.Add(new ErrorLogAttribute());
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new MyNASJsonContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;
                });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = MyNASAuthOptions.DefaultScheme;
                    options.DefaultChallengeScheme = MyNASAuthOptions.DefaultScheme;
                })
                .AddMyNASAuth(options => { });
            services.AddAuthorization(options =>
                {
                    options.AddPolicy("UserBase", policy =>
                        policy.RequireRole(UserRole.User.ToString(), UserRole.DataAdmin.ToString(), UserRole.SystemAdmin.ToString()));
                    options.AddPolicy("DataAdminBase", policy =>
                        policy.RequireRole(UserRole.DataAdmin.ToString(), UserRole.SystemAdmin.ToString()));
                    options.AddPolicy("Admin", policy =>
                        policy.RequireRole(UserRole.SystemAdmin.ToString()));
                });
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyNAS API", Version = "v1" });
                    c.CustomSchemaIds(i => i.FullName);
                });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyNAS API V1");
                });
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller}/{action}");
                foreach (var item in new[] { "login", "main", "images", "videos", "movies", "system" })
                {
                    endpoints.MapControllerRoute(
                        name: item,
                        pattern: item + "/{item?}",
                        new { controller = "Home", action = "Index" });
                }
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
