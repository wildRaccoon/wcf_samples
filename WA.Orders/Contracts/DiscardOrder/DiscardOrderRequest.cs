using System.Runtime.Serialization;
using WA.Core;

namespace WA.Orders.Contracts.DiscardOrder
{
    [DataContract]
    public class DiscardOrderRequest : BaseRequest
    {
        [DataMember]
        public int OrderId { get; set; }
    }
}