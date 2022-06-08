using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Caching;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace OrderReturn
{
    [DependsOn(
    typeof(OrderReturnDomainModule),
    typeof(OrderReturnApplicationContractsModule),
    typeof(AbpBlobStoringFileSystemModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpCachingModule),
    typeof(AbpEmailingModule),
    typeof(AbpVirtualFileSystemModule)
    )]
    public class OrderReturnApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<OrderReturnApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<OrderReturnApplicationModuleAutoMapperProfile>(validate: true);
            });

            //blob存储位置配置
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseFileSystem(fileSystem =>
                    {
                        var path = Environment.CurrentDirectory;
                        fileSystem.BasePath = path;
                        fileSystem.AppendContainerNameToBasePath = true;
                    });
                });
            });

            //hangfire队列配置
            context.Services.AddHangfireServer(config =>
            {
                config.Queues = new string[] { "delete_expired_files", "push_to_os" };//指定队列
                config.WorkerCount = 1; //最小为服务器处理器数*5，如果大于20 则最小为20
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<OrderReturnApplicationModule>();
            });
        }
    }
}
