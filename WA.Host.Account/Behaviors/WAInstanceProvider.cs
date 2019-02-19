using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace WA.Host.Account.Behaviors
{
    public class WAInstanceProvider : IInstanceProvider
    {
        private IServiceProvider _provider { get; set; }
        private Type _serviceType { get; set; }

        public WAInstanceProvider(IServiceProvider provider,Type serviceType)
        {
            _provider = provider;
            _serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return _provider.GetService(_serviceType);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return _provider.GetService(_serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            
        }
    }
}