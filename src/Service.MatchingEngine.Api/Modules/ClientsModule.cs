using Autofac;
using MyNoSqlServer.DataReader;
using Service.Fees.Client;
using Service.MatchingEngine.Api.Factory;
using Service.MatchingEngine.Api.Services;

namespace Service.MatchingEngine.Api.Modules
{
    public class ClientsModule : Module
    {
        private readonly MyNoSqlTcpClient _myNoSqlClient;

        public ClientsModule(MyNoSqlTcpClient myNoSqlClient)
        {
            _myNoSqlClient = myNoSqlClient;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssetFeesClients(_myNoSqlClient);
            builder.RegisterSpotInstrumentFeesClients(_myNoSqlClient);

            var factory = new MatchingEngineClientFactory(Program.Settings.CashGrpcUrl, Program.Settings.TradingGrpcUrl,
                Program.Settings.BalancesGrpcUrl, Program.Settings.OrderBookGrpcUrl);

            builder.RegisterInstance(factory).AsSelf().SingleInstance();

            builder.RegisterType<BalancesService>()
                .As<ME.Contracts.Api.BalancesService.BalancesServiceBase>()
                .SingleInstance();
            
            builder.RegisterType<CashService>()
                .As<ME.Contracts.Api.CashService.CashServiceBase>()
                .SingleInstance();

            // builder.RegisterInstance(factory.GetCashService()).As<CashService.CashServiceClient>().SingleInstance();
            // builder.RegisterInstance(factory.GetTradingService()).As<TradingService.TradingServiceClient>()
            //     .SingleInstance();
            // builder.RegisterInstance(factory.GetBalancesService()).As<BalancesService.BalancesServiceClient>()
            //     .SingleInstance();
            // builder.RegisterInstance(factory.GetOrderBookService()).As<OrderBooksService.OrderBooksServiceClient>()
            //     .SingleInstance();
        }
    }
}