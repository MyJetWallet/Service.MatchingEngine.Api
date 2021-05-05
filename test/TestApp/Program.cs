using System;
using System.Threading.Tasks;
using ME.Contracts.Api.BalancesMessages;
using ProtoBuf.Grpc.Client;
using Service.MatchingEngine.Api.Client;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();

            var factory = new MatchingEngineApiClientFactory("http://localhost:80");
            var service = factory.GetBalancesService();

            Console.WriteLine(service.GetAll(new BalancesGetAllRequest()
            {
                BrokerId = "",
                WalletId = ""
            }).Balances.Count);

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}