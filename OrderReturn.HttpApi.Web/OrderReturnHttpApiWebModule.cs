using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using OrderReturn.BackgroundJob;
using OrderReturn.Data;
using OrderReturn.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace OrderReturn
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(OrderReturnHttpApiModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(OrderReturnApplicationModule),
        typeof(OrderReturnEntityFrameworkCoreDbMigrationsModule)
       )]
    public class OrderReturnHttpApiWebModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            base.ConfigureServices(context);

            //添加Swagger Api说明文档
            context.Services.AddSwaggerGen(options =>
            {
                //接口文档名称
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "订单退货 API", Version = "v1" });

                var basePath = AppContext.BaseDirectory;
                DirectoryInfo d = new DirectoryInfo(basePath);
                FileInfo[] files = d.GetFiles("*.xml");
                var xmls = files.Select(a => Path.Combine(basePath, a.FullName)).ToList();
                foreach (var item in xmls)
                {
                    options.IncludeXmlComments(item);
                }

                options.DocInclusionPredicate((docName, apiDes) =>
                {
                    //v1文档显示所有
                    if (docName == "v1")
                    {
                        return true;
                    }

                    if (docName == apiDes.GroupName)
                    {
                        return true;
                    }

                    return false;
                });

                options.CustomSchemaIds(type => type.FullName);
                //options.OperationFilter<AddAuthTokenHeaderParameter>();
                //options.DocumentFilter<EnumDocumentFilter>();
                //options.SchemaFilter<EnumSchemaFilter>();

                //添加Authorization
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            })
            .AddSwaggerGenNewtonsoftSupport()//show enum string
            .AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    //builder.SetPreflightMaxAge(TimeSpan.FromSeconds(1800L));//update by jason
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            context.Services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(configuration["ConnectionStrings:HangfireConnection"]
                    , new Hangfire.SqlServer.SqlServerStorageOptions { SchemaName = "Job" });
            });

            context.Services.AddHttpClient();
            //context.Services.AddRazorPages();
        }

        public override void OnApplicationInitialization(Volo.Abp.ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            //CorrelationId会在请求中产生一个唯一标识，并可以将唯一标识作为一个Header传递到下一请求，以此类推，从而整个链路都可以获取到这个标识，并自主打印到日志当中。
            app.UseCorrelationId();
            //静态文件
            app.UseStaticFiles();
            //使用路由
            app.UseRouting();
            //options 请求
            //app.UseOptionsRequest();

            app.UseCors(DefaultCorsPolicyName);


            //使用授权认证
            app.UseAuthentication();
            //功能授权
            app.UseAuthorization();
            app.UseAbpClaimsMap();

            //使用abp本地化
            app.UseAbpRequestLocalization();
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "订单退货API文档");
            });

            //Hangfire
            app.UseHangfireDashboard();

            app.UseAuditing();
            //app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            //generate migration
            AsyncHelper.RunSync(async () =>
            {
                using (var scope = context.ServiceProvider.CreateScope())
                {
                    IOrderReturnDbSchemaMigrator orderReturnDbSchemaMigrator = scope.ServiceProvider.GetRequiredService<IOrderReturnDbSchemaMigrator>();
                    await orderReturnDbSchemaMigrator.MigrateAsync();

                    await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync();
                }
            });

            //删除7天前文件任务
            var args = new DeleteExpiredFilesJobArgs() { };
            var jobName = $"delete_expired_files";
            RecurringJob.AddOrUpdate<DeleteExpiredFilesJob>(jobName, a => a.ExecuteAsync(args), "0 55 23 * * ?");
            RecurringJob.Trigger(jobName);

            //推送数据到OS任务
            var pushArgs = new PushToOSJobArgs() { };
            jobName = "push_to_os";
            RecurringJob.AddOrUpdate<PushToOSJob>(jobName, aa => aa.ExecuteAsync(pushArgs), "0 0/10 * * * ? ");
            RecurringJob.Trigger(jobName);
        }
    }
}
