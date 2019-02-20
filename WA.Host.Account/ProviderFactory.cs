using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WA.Host.Core;
using WA.Repository.Account;
using WA.Services.Account;

namespace WA.Host.Account
{
    public class ProviderFactory : IProviderFactory
    {
        private static IServiceProvider ServiceProviderInstance { get; set; } = GetProviderInstance();

        public static IServiceProvider GetProviderInstance()
        {
            //register all deps
            var sc = new ServiceCollection();
            sc.AddLogging(builder =>
            {
#if DEBUG
                builder.AddDebug();
#endif
            });

            sc.AddDbContext<AccountDataContext>(c => c.UseInMemoryDatabase("AccountStorage"));
            sc.AddTransient<AccountService>();

            var sp = sc.BuildServiceProvider();
            return sp;
        }



        public IServiceProvider GetProvider()
        {
            return ServiceProviderInstance;
        }
    }
}