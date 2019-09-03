using HCVisionFiles.Configuration;
using MongoDB.Driver;
using System;
using MongoDB.Driver.GridFS;
using HCVisionFiles.Services;
using HCVisionFiles.Models.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FileServiceCollectionExtensions
    {
        private static FilesOptions Options(this IServiceProvider provider)
        {
            return provider.GetService<FilesOptions>();
        }

        private static IMongoDatabase Database(this IServiceProvider provider)
        {
            return provider.GetService<IMongoClient>().GetDatabase(provider.Options().Database);
        }

        private static IGridFSBucket GridFSBucket(this IServiceProvider provider)
        {
            return new GridFSBucket(provider.Database(), provider.Options().FSBucketOptions);
        }

        public static IServiceCollection AddFilesServices(this IServiceCollection services, Action<FilesOptions> configure = null)
        {
            var options = new FilesOptions();
            configure?.Invoke(options);
            return services.AddFilesServices(options);
        }

        public static IServiceCollection AddFilesServices(this IServiceCollection services, FilesOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            services.AddSingleton(options);

            services.AddSingleton(provider => provider.GridFSBucket());
            services.AddSingleton<IFileService, FileService>();

            return services;
        }
    }
}
