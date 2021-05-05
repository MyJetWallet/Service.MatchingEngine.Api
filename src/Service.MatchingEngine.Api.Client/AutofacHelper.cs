using Autofac;
using ME.Contracts.Api;

namespace Service.MatchingEngine.Api.Client
{
    public static class AutofacHelper
    {
        public static void RegisterMatchingEngineApiClient(this ContainerBuilder builder,
            string matchingEngineApiUrl)
        {
            var factory = new MatchingEngineApiClientFactory(matchingEngineApiUrl);

            builder.RegisterInstance(factory.GetBalancesService()).As<BalancesService.BalancesServiceClient>()
                .SingleInstance();
            builder.RegisterInstance(factory.GetCashService()).As<CashService.CashServiceClient>()
                .SingleInstance();
            builder.RegisterInstance(factory.GetTradingService()).As<TradingService.TradingServiceClient>()
                .SingleInstance();
            builder.RegisterInstance(factory.GetOrderBookService()).As<OrderBooksService.OrderBooksServiceClient>()
                .SingleInstance();
        }
    }
}