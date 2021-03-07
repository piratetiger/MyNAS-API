using Microsoft.Extensions.DependencyInjection;
using MyNAS.Services.Abstraction;
using MyNAS.Services.FileSystemServices;

namespace MyNAS.Site
{
    public static class FileSystemServicesExt
    {
        public static void AddFileSystemServices(this IServiceCollection services)
        {
            services.AddScoped<IImagesService, ImagesService>();
        }
    }
}