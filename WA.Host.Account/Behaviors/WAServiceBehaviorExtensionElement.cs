using System;
using System.ServiceModel.Configuration;

namespace WA.Host.Account.Behaviors
{
    public class WAServiceBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType => typeof(WAServiceBehavior);

        protected override object CreateBehavior()
        {
            return new WAServiceBehavior();
        }
    }
}