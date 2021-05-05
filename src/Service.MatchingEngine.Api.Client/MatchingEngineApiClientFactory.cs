using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using JetBrains.Annotations;
using ME.Contracts.Api;
using MyJetWallet.Sdk.GrpcMetrics;

namespace Service.MatchingEngine.Api.Client
{
    [UsedImplicitly]
    public class MatchingEngineApiClientFactory
    {
        private readonly CallInvoker _channel;

        public MatchingEngineApiClientFactory(string matchingEngineApiUrl)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress(matchingEngineApiUrl);
            _channel = channel.Intercept(new PrometheusMetricsInterceptor());
        }

        public BalancesService.BalancesServiceClient GetBalancesService() =>
            new BalancesService.BalancesServiceClient(_channel);

        public CashService.CashServiceClient GetCashService() =>
            new CashService.CashServiceClient(_channel);

        public OrderBooksService.OrderBooksServiceClient GetOrderBookService() =>
            new OrderBooksService.OrderBooksServiceClient(_channel);

        public TradingService.TradingServiceClient GetTradingService() =>
            new TradingService.TradingServiceClient(_channel);
    }
}