using Autofac;
using MyNoSqlServer.DataReader;
using Service.Fees.Client;
using Service.MatchingEngine.Api.Factory;

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
            builder
                .RegisterInstance(_myNoSqlClient)
                .AsSelf()
                .SingleInstance();
            
            builder.RegisterFeesClients(_myNoSqlClient);

            var factory = new MatchingEngineClientFactory(Program.Settings.CashGrpcUrl, Program.Settings.TradingGrpcUrl,
                Program.Settings.BalancesGrpcUrl, Program.Settings.OrderBookGrpcUrl);

            builder.RegisterInstance(factory).AsSelf().SingleInstance();
        }
    }
}