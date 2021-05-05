using System.Threading.Tasks;
using Grpc.Core;
using ME.Contracts.Api.IncomingMessages;
using MyJetWallet.Sdk.Service;
using Service.Fees.Client;
using Service.Fees.Domain.Models;
using Service.MatchingEngine.Api.Converters;
using Service.MatchingEngine.Api.Factory;

namespace Service.MatchingEngine.Api.Services
{
    public class CashService : ME.Contracts.Api.CashService.CashServiceBase
    {
        private readonly IAssetFeesClient _assetFeesClient;
        private readonly ME.Contracts.Api.CashService.CashServiceClient _cashServiceClient;

        public CashService(IAssetFeesClient assetFeesClient,
            MatchingEngineClientFactory matchingEngineClientFactory)
        {
            _assetFeesClient = assetFeesClient;
            _cashServiceClient = matchingEngineClientFactory.GetCashService();
        }

        public override Task<CashInOutOperationResponse> CashInOut(CashInOutOperation request,
            ServerCallContext context)
        {
            var isWithdrawal = double.Parse(request.Volume) < 0;
            using var activity = MyTelemetry.StartActivity(isWithdrawal ? "Withdrawal" : "Deposit");

            activity?.AddTag("operationId", request.Id)
                .AddTag("brokerId", request.BrokerId)
                .AddTag("accountId", request.AccountId)
                .AddTag("walletId", request.WalletId)
                .AddTag("assetId", request.AssetId)
                .AddTag("volume", request.Volume);

            if (isWithdrawal)
            {
                var fees = _assetFeesClient.GetAssetFees(request.BrokerId, request.AssetId,
                    OperationType.Withdrawal);
                if (fees != null)
                {
                    request.Fees.Add(FeesConverter.ConvertAssetFee(fees));
                }
            }

            return _cashServiceClient.CashInOutAsync(request, cancellationToken: context.CancellationToken)
                .ResponseAsync;
        }

        public override Task<CashTransferOperationResponse> CashTransfer(CashTransferOperation request,
            ServerCallContext context)
        {
            using var activity = MyTelemetry.StartActivity("CashTransfer");

            activity?.AddTag("operationId", request.Id)
                .AddTag("brokerId", request.BrokerId)
                .AddTag("accountId", request.AccountId)
                .AddTag("fromWalletId", request.FromWalletId)
                .AddTag("toWalletId", request.ToWalletId)
                .AddTag("assetId", request.AssetId)
                .AddTag("volume", request.Volume);

            return _cashServiceClient.CashTransferAsync(request, cancellationToken: context.CancellationToken)
                .ResponseAsync;
        }

        public override Task<CashSwapOperationResponse> CashSwap(CashSwapOperation request, ServerCallContext context)
        {
            using var activity = MyTelemetry.StartActivity("CashSwap");

            activity?.AddTag("operationId", request.Id)
                .AddTag("brokerId", request.BrokerId)
                .AddTag("accountId1", request.AccountId1)
                .AddTag("WalletId1", request.WalletId1)
                .AddTag("assetId1", request.AssetId1)
                .AddTag("volume1", request.Volume1)
                .AddTag("accountId2", request.AccountId2)
                .AddTag("WalletId2", request.WalletId2)
                .AddTag("assetId2", request.AssetId2)
                .AddTag("volume2", request.Volume2);

            return _cashServiceClient.CashSwapAsync(request, cancellationToken: context.CancellationToken)
                .ResponseAsync;
        }

        public override Task<ReservedCashInOutOperationResponse> ReservedCashInOut(ReservedCashInOutOperation request,
            ServerCallContext context)
        {
            using var activity = MyTelemetry.StartActivity("ReservedCashTransfer");

            activity?.AddTag("operationId", request.Id)
                .AddTag("brokerId", request.BrokerId)
                .AddTag("accountId", request.AccountId)
                .AddTag("walletId", request.WalletId)
                .AddTag("assetId", request.AssetId)
                .AddTag("reservedVolume", request.ReservedVolume)
                .AddTag("reservedSwapVolume", request.ReservedForSwapVolume);

            return _cashServiceClient.ReservedCashInOutAsync(request, cancellationToken: context.CancellationToken)
                .ResponseAsync;
        }
    }
}