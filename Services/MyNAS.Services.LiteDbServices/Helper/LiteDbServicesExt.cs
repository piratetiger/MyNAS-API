using Microsoft.Extensions.DependencyInjection;
using MyNAS.Services.Abstraction;
using MyNAS.Services.LiteDbServices;

namespace MyNAS.Site
{
    public static class LiteDbServicesExt
    {
        public static void AddLiteDbServices(this IServiceCollection services)
        {
            var adminService = new AdminService();
            adminService.InitDB().GetAwaiter().GetResult();

            services.AddScoped<IFilesService, FilesService>();
            services.AddScoped<IImagesService, ImagesService>();
            services.AddScoped<IVideosService, VideoService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILogsService, LogsService>();
        }
    }
}
