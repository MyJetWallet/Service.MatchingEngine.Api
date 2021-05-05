using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using ME.Contracts.OrderBooksMessages;
using MyJetWallet.Sdk.Service;
using Service.MatchingEngine.Api.Factory;

namespace Service.MatchingEngine.Api.Services
{
    public class OrderBooksService : ME.Contracts.Api.OrderBooksService.OrderBooksServiceBase
    {
        private readonly ME.Contracts.Api.OrderBooksService.OrderBooksServiceClient _orderBooksServiceClient;

        public OrderBooksService(MatchingEngineClientFactory matchingEngineClientFactory)
        {
            _orderBooksServiceClient = matchingEngineClientFactory.GetOrderBookService();
        }

        public override Task OrderBookSnapshots(Empty request, IServerStreamWriter<OrderBookSnapshot> responseStream, ServerCallContext context)
        {
            using var activity = MyTelemetry.StartActivity("OrderBookSnapshots");
            using var resp =
                _orderBooksServiceClient.OrderBookSnapshots(new Empty(), cancellationToken: context.CancellationToken);

            return Task.WhenAll(resp.ResponseStream.ForEachAsync(responseStream.WriteAsync));
        }
    }
}