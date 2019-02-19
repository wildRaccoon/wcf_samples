using System;
using System.ComponentModel;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace WA.Host.Core.Behaviors
{
    public class WAServiceBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType => typeof(WAServiceBehavior);

        private const string PropertyName = "provider";

        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty(PropertyName, IsRequired = true)]
        public Type ProviderFactory
        {
            get { return (Type)this[PropertyName]; }
            set { this[PropertyName] = value; }
        }

        protected override object CreateBehavior()
        {
            var spf = Activator.CreateInstance(ProviderFactory) as IProviderFactory;
            var sp = spf.GetProvider();
            return new WAServiceBehavior(sp);
        }
    }
}