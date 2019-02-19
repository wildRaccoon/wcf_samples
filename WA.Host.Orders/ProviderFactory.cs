using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WA.Host.Core;
using WA.Services.Orders;

namespace WA.Host.Orders
{
    public class ProviderFactory : IProviderFactory
    {
        public IServiceProvider GetProvider()
        {
            //register all deps
            var sc = new ServiceCollection();
            sc.AddLogging(builder =>
            {
#if DEBUG
                builder.AddDebug();
#endif
            });

            sc.AddTransient<OrdersService>();

            var sp = sc.BuildServiceProvider();

            return sp;
        }
    }
}