using System.Threading.Tasks;
using Grpc.Core;
using ME.Contracts.Api.BalancesMessages;
using MyJetWallet.Sdk.Service;
using Service.MatchingEngine.Api.Factory;

namespace Service.MatchingEngine.Api.Services
{
    public class BalancesService : ME.Contracts.Api.BalancesService.BalancesServiceBase
    {
        private readonly ME.Contracts.Api.BalancesService.BalancesServiceClient _balancesServiceClient;

        public BalancesService(MatchingEngineClientFactory matchingEngineClientFactory)
        {
            _balancesServiceClient = matchingEngineClientFactory.GetBalancesService();
        }

        public override Task<BalancesGetAllResponse> GetAll(BalancesGetAllRequest request, ServerCallContext context)
        {
            using var activity = MyTelemetry.StartActivity("Balances.GetAll");

            activity?.AddTag("brokerId", request.BrokerId)
                .AddTag("walletId", request.WalletId);

            return  _balancesServiceClient.GetAllAsync(request).ResponseAsync;
        }

        public override Task<BalancesGetByAssetIdResponse> GetByAssetId(BalancesGetByAssetIdRequest request, ServerCallContext context)
        {
            using var activity = MyTelemetry.StartActivity("Balances.GetByAssetId");

            activity?.AddTag("brokerId", request.BrokerId)
                .AddTag("walletId", request.WalletId)
                .AddTag("assetId", request.AssetId);

            return _balancesServiceClient.GetByAssetIdAsync(request).ResponseAsync;
        }
    }
}