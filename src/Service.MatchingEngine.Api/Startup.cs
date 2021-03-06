using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyJetWallet.Sdk.GrpcMetrics;
using MyJetWallet.Sdk.GrpcSchema;
using MyJetWallet.Sdk.Service;
using MyNoSqlServer.DataReader;
using Prometheus;
using ProtoBuf.Grpc.Server;
using Service.MatchingEngine.Api.Modules;
using Service.MatchingEngine.Api.Services;
using SimpleTrading.BaseMetrics;
using SimpleTrading.ServiceStatusReporterConnector;

namespace Service.MatchingEngine.Api
{
    public class Startup
    {
        private readonly MyNoSqlTcpClient _myNoSqlClient;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _myNoSqlClient = new MyNoSqlTcpClient(
                Program.ReloadedSettings(e => e.MyNoSqlReaderHostPort),
                ApplicationEnvironment.HostName ??
                $"{ApplicationEnvironment.AppName}:{ApplicationEnvironment.AppVersion}");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.BindCodeFirstGrpc(options =>
            {
                options.Interceptors.Add<LogRequestInterceptor>();
            });

            services.AddHostedService<ApplicationLifetimeManager>();

            services.AddMyTelemetry("SP-", Program.Settings.ZipkinUrl);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMetricServer();

            app.BindServicesTree(Assembly.GetExecutingAssembly());

            app.BindIsAlive();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapGrpcSchema<OrderBooksService, IOrderBookService>();
                // endpoints.MapGrpcSchema<TradingService, ITradingService>();

                endpoints.MapGrpcService<BalancesService>();
                endpoints.MapGrpcService<CashService>();
                endpoints.MapGrpcService<OrderBooksService>();
                endpoints.MapGrpcService<TradingService>();
                endpoints.MapGrpcSchemaRegistry();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<SettingsModule>();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule(new ClientsModule(_myNoSqlClient));
        }
    }
}