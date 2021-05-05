using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using ME.Contracts.Api;
using MyJetWallet.Sdk.GrpcMetrics;

namespace Service.MatchingEngine.Api.Factory
{
    public class MatchingEngineClientFactory
    {
        private readonly CallInvoker _invokerCashService;
        private readonly CallInvoker _invokerTradingService;
        private readonly CallInvoker _invokerBalancesService;
        private readonly CallInvoker _invokerOrderBookService;

        public MatchingEngineClientFactory(string cashServiceGrpcUrl, string tradingServiceGrpcUrl, string balancesServiceGrpcUrl, string orderBookServiceGrpcUrl)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            if (!string.IsNullOrEmpty(cashServiceGrpcUrl))
            {
                var channelCashService = GrpcChannel.ForAddress(cashServiceGrpcUrl);
                _invokerCashService = channelCashService.Intercept(new PrometheusMetricsInterceptor());
            }

            if (!string.IsNullOrEmpty(tradingServiceGrpcUrl))
            {
                var channelTradingService = GrpcChannel.ForAddress(tradingServiceGrpcUrl);
                _invokerTradingService = channelTradingService.Intercept(new PrometheusMetricsInterceptor());
            }

            if (!string.IsNullOrEmpty(balancesServiceGrpcUrl))
            {
                var channelBalancesService = GrpcChannel.ForAddress(balancesServiceGrpcUrl);
                _invokerBalancesService = channelBalancesService.Intercept(new PrometheusMetricsInterceptor());
            }

            if (!string.IsNullOrEmpty(orderBookServiceGrpcUrl))
            {
                var channelOrderBookService = GrpcChannel.ForAddress(orderBookServiceGrpcUrl);
                _invokerOrderBookService = channelOrderBookService.Intercept(new PrometheusMetricsInterceptor());
            }
        }

        public CashService.CashServiceClient GetCashService()
        {
            return new CashService.CashServiceClient(_invokerCashService);
        }

        public TradingService.TradingServiceClient GetTradingService()
        {
            return new TradingService.TradingServiceClient(_invokerTradingService);
        }

        public BalancesService.BalancesServiceClient GetBalancesService()
        {
            return new BalancesService.BalancesServiceClient(_invokerBalancesService);
        }

        public OrderBooksService.OrderBooksServiceClient GetOrderBookService()
        {
            return new OrderBooksService.OrderBooksServiceClient(_invokerOrderBookService);
        }
    }
}
