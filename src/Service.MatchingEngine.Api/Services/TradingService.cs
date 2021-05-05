using System.Threading.Tasks;
using Grpc.Core;
using ME.Contracts.Api.IncomingMessages;
using MyJetWallet.Sdk.Service;
using Service.Fees.Client;
using Service.MatchingEngine.Api.Converters;
using Service.MatchingEngine.Api.Factory;

namespace Service.MatchingEngine.Api.Services
{
    public class TradingService : ME.Contracts.Api.TradingService.TradingServiceBase
    {
        private readonly ME.Contracts.Api.TradingService.TradingServiceClient _tradingServiceClient;
        private readonly ISpotInstrumentFeesClient _spotInstrumentFeesClient;

        public TradingService(MatchingEngineClientFactory matchingEngineClientFactory,
            ISpotInstrumentFeesClient spotInstrumentFeesClient)
        {
            _tradingServiceClient = matchingEngineClientFactory.GetTradingService();
            _spotInstrumentFeesClient = spotInstrumentFeesClient;
        }

        public override Task<MarketOrderResponse> MarketOrder(MarketOrder request, ServerCallContext context)
        {
            using var activity = MyTelemetry.StartActivity("MarketOrder");

            activity?.AddTag("operationId", request.Id)
                .AddTag("brokerId", request.BrokerId)
                .AddTag("accountId", request.AccountId)
                .AddTag("walletId", request.WalletId)
                .AddTag("assetPairId", request.AssetPairId)
                .AddTag("straight", request.Straight)
                .AddTag("volume", request.Volume);

            AddMarketOrderFees(request);

            return _tradingServiceClient.MarketOrderAsync(request, cancellationToken: context.CancellationToken)
                .ResponseAsync;
        }

        public override Task<LimitOrderResponse> LimitOrder(LimitOrder request, ServerCallContext context)
        {
            using var activity = MyTelemetry.StartActivity("LimitOrder");

            activity?.AddTag("operationId", request.Id)
                .AddTag("brokerId", request.BrokerId)
                .AddTag("accountId", request.AccountId)
                .AddTag("walletId", request.WalletId)
                .AddTag("type", request.Type)
                .AddTag("assetPairId", request.AssetPairId)
                .AddTag("price", request.Price)
                .AddTag("volume", request.Volume)
                .AddTag("cancelAllPreviousLimitOrders", request.CancelAllPreviousLimitOrders);

            AddLimitOrderFees(request);
            return _tradingServiceClient.LimitOrderAsync(request, cancellationToken: context.CancellationToken)
                .ResponseAsync;
        }

        public override Task<LimitOrderCancelResponse> CancelLimitOrder(LimitOrderCancel request,
            ServerCallContext context)
        {
            using var activity = MyTelemetry.StartActivity("CancelLimitOrder");

            activity?.AddTag("operationId", request.Id)
                .AddTag("brokerId", request.BrokerId)
                .AddTag("accountId", request.AccountId)
                .AddTag("walletId", request.WalletId)
                .AddTag("orderIds", request.LimitOrderId);

            return _tradingServiceClient.CancelLimitOrderAsync(request, cancellationToken: context.CancellationToken)
                .ResponseAsync;
        }

        public override Task<MultiLimitOrderResponse> MultiLimitOrder(MultiLimitOrder request,
            ServerCallContext context)
        {
            using var activity = MyTelemetry.StartActivity("MultiLimitOrder");

            activity?.AddTag("operationId", request.Id)
                .AddTag("brokerId", request.BrokerId)
                .AddTag("accountId", request.AccountId)
                .AddTag("walletId", request.WalletId)
                .AddTag("assetPairId", request.AssetPairId)
                .AddTag("ordersCount", request.Orders.Count);

            AddMultiLimitOrderFees(request);
            return _tradingServiceClient.MultiLimitOrderAsync(request, cancellationToken: context.CancellationToken)
                .ResponseAsync;
        }

        private void AddLimitOrderFees(LimitOrder request)
        {
            var fees = _spotInstrumentFeesClient.GetSpotInstrumentFees(request.BrokerId, request.AssetPairId);
            if (fees != null)
            {
                request.Fees.Add(FeesConverter.ConvertLimitOrderFee(fees));
            }
        }

        private void AddMarketOrderFees(MarketOrder request)
        {
            var fees = _spotInstrumentFeesClient.GetSpotInstrumentFees(request.BrokerId, request.AssetPairId);
            if (fees != null)
            {
                request.Fees.Add(FeesConverter.ConvertMarketOrderFee(fees));
            }
        }

        private void AddMultiLimitOrderFees(MultiLimitOrder request)
        {
            var fees = _spotInstrumentFeesClient.GetSpotInstrumentFees(request.BrokerId, request.AssetPairId);
            if (fees != null)
            {
                foreach (var requestOrder in request.Orders)
                {
                    requestOrder.Fees.Add(FeesConverter.ConvertLimitOrderFee(fees));
                }
            }
        }
    }
}