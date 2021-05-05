using System;
using System.Globalization;
using ME.Contracts.Api.IncomingMessages;
using Service.Fees.Domain.Models;
using FeeSizeType = ME.Contracts.Api.IncomingMessages.FeeSizeType;
using FeeType = ME.Contracts.Api.IncomingMessages.FeeType;

namespace Service.MatchingEngine.Api.Converters
{
    public static class FeesConverter
    {
        public static Fee ConvertAssetFee(AssetFees assetFees)
        {
            var fee = new Fee {Type = ConvertFeeType(assetFees.FeeType)};
            if (fee.Type == FeeType.NoFee) return fee;

            fee.SizeType = ConvertFeeSizeType(assetFees.FeeSizeType);
            fee.Size = assetFees.FeeSize.ToString(CultureInfo.InvariantCulture);

            fee.TargetAccountId = assetFees.AccountId;
            fee.TargetWalletId = assetFees.WalletId;
            fee.AssetId.Add(assetFees.AssetId);

            return fee;
        }

        public static Fee ConvertMarketOrderFee(SpotInstrumentFees instrumentFees)
        {
            var fee = new Fee {Type = ConvertFeeType(instrumentFees.FeeType)};
            if (fee.Type == FeeType.NoFee) return fee;

            fee.SizeType = ConvertFeeSizeType(instrumentFees.TakerFeeSizeType);
            fee.Size = instrumentFees.TakerFeeSize.ToString(CultureInfo.InvariantCulture);

            fee.TargetAccountId = instrumentFees.AccountId;
            fee.TargetWalletId = instrumentFees.WalletId;
            fee.AssetId.Add(instrumentFees.FeeAssetId);

            return fee;
        }

        public static LimitOrderFee ConvertLimitOrderFee(SpotInstrumentFees instrumentFees)
        {
            var fee = new LimitOrderFee {Type = ConvertFeeType(instrumentFees.FeeType)};
            if (fee.Type == FeeType.NoFee) return fee;

            fee.MakerSizeType = ConvertFeeSizeType(instrumentFees.MakerFeeSizeType);
            fee.MakerSize = instrumentFees.MakerFeeSize.ToString(CultureInfo.InvariantCulture);

            fee.TakerSizeType = ConvertFeeSizeType(instrumentFees.TakerFeeSizeType);
            fee.TakerSize = instrumentFees.TakerFeeSize.ToString(CultureInfo.InvariantCulture);

            fee.TargetAccountId = instrumentFees.AccountId;
            fee.TargetWalletId = instrumentFees.WalletId;
            fee.AssetId.Add(instrumentFees.FeeAssetId);

            return fee;
        }

        private static FeeType ConvertFeeType(Fees.Domain.Models.FeeType type)
        {
            switch (type)
            {
                case Fees.Domain.Models.FeeType.NoFee:
                    return FeeType.NoFee;
                case Fees.Domain.Models.FeeType.ClientFee:
                    return FeeType.ClientFee;
                case Fees.Domain.Models.FeeType.ExternalFee:
                    return FeeType.ExternalFee;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static FeeSizeType ConvertFeeSizeType(Fees.Domain.Models.FeeSizeType sizeType)
        {
            switch (sizeType)
            {
                case Fees.Domain.Models.FeeSizeType.Percentage:
                    return FeeSizeType.Percentage;
                case Fees.Domain.Models.FeeSizeType.Absolute:
                    return FeeSizeType.Absolute;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sizeType), sizeType, null);
            }
        }
    }
}