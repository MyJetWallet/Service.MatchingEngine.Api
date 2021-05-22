using Autofac;
using ME.Contracts.Api;

namespace Service.MatchingEngine.Api.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Services.BalancesService>()
                .As<BalancesService.BalancesServiceBase>()
                .SingleInstance();

            builder.RegisterType<Services.CashService>()
                .As<CashService.CashServiceBase>()
                .SingleInstance();

            builder.RegisterType<Services.OrderBooksService>()
                .As<OrderBooksService.OrderBooksServiceBase>()
                .SingleInstance();

            builder.RegisterType<Services.TradingService>()
                .As<TradingService.TradingServiceBase>()
                .SingleInstance();
        }
    }
}