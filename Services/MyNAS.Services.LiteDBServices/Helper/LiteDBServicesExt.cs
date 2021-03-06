using Microsoft.Extensions.DependencyInjection;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDBServices;

namespace MyNAS.Site
{
    public static class LiteDBServicesExt
    {
        public static void AddLiteDBServices(this IServiceCollection services)
        {
            var adminService = new AdminService();
            adminService.InitDB().GetAwaiter().GetResult();

            services.AddScoped<IFilesService, FilesService>();
            services.AddScoped<IImagesService, ImagesService>();
            services.AddScoped<IVideosService, VideoService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}