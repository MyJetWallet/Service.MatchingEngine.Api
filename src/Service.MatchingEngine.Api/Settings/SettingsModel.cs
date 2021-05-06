using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.MatchingEngine.Api.Settings
{
    public class SettingsModel
    {
        [YamlProperty("MatchingEngineApi.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("MatchingEngineApi.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("MatchingEngineApi.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; set; }

        [YamlProperty("MatchingEngineApi.MatchingEngine.BalancesGrpcServiceUrl")]
        public string BalancesGrpcUrl { get; set; }

        [YamlProperty("MatchingEngineApi.MatchingEngine.CashGrpcServiceUrl")]
        public string CashGrpcUrl { get; set; }

        [YamlProperty("MatchingEngineApi.MatchingEngine.TradingGrpcServiceUrl")]
        public string TradingGrpcUrl { get; set; }

        [YamlProperty("MatchingEngineApi.MatchingEngine.OrderBookGrpcServiceUrl")]
        public string OrderBookGrpcUrl { get; set; }

        [YamlProperty("MatchingEngineApi.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }
    }
}