using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WA.Host.Core;
using WA.Services.Account;

namespace WA.Host.Account
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

            sc.AddTransient<AccountService>();

            var sp = sc.BuildServiceProvider();

            return sp;
        }
    }
}