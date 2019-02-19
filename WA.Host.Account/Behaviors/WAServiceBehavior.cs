using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using WA.Account.AccountService;
using WA.Services.Account;

namespace WA.Host.Account.Behaviors
{
    public class WAServiceBehavior : IServiceBehavior
    {
        protected static IServiceProvider _serviceProvider { get; set; }

        public WAServiceBehavior()
        {
            if (_serviceProvider == null)
            {
                var collectionDeps = new ServiceCollection();
                collectionDeps.AddLogging(c => c.AddDebug());
                collectionDeps.AddTransient<AccountService>();
                _serviceProvider = collectionDeps.BuildServiceProvider();
            }
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
 
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher ed in cd.Endpoints)
                {
                    ed.DispatchRuntime.InstanceProvider = new WAInstanceProvider(_serviceProvider, serviceDescription.ServiceType);
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}