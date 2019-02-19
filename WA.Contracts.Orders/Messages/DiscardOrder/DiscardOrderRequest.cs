using System.Runtime.Serialization;
using WA.Contracts.Core;

namespace WA.Contracts.Orders.Messages.DiscardOrder
{
    [DataContract]
    public class DiscardOrderRequest : BaseRequest
    {
        [DataMember]
        public int OrderId { get; set; }
    }
}