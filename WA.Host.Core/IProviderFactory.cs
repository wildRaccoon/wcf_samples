using System;

namespace WA.Host.Core
{
    public interface IProviderFactory
    {
        IServiceProvider GetProvider();
    }
}
