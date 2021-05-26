using DeliveryLogic;
using DeliveryLogic.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryExpiry
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IDeliveriesLogic, DeliveriesLogic>();
        }
    }
}
