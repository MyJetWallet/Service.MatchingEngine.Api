using SimpleTrading.SettingsReader;

namespace Service.MatchingEngine.Api.Settings
{
    [YamlAttributesOnly]
    public class SettingsModel
    {
        [YamlProperty("MatchingEngine.Api.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("MatchingEngine.Api.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("MatchingEngine.Api.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; set; }

        [YamlProperty("MatchingEngine.Api.BalancesGrpcUrl")]
        public string BalancesGrpcUrl { get; set; }

        [YamlProperty("MatchingEngine.Api.CashGrpcUrl")]
        public string CashGrpcUrl { get; set; }

        [YamlProperty("MatchingEngine.Api.TradingGrpcUrl")]
        public string TradingGrpcUrl { get; set; }

        [YamlProperty("MatchingEngine.Api.OrderBookGrpcUrl")]
        public string OrderBookGrpcUrl { get; set; }
    }
}