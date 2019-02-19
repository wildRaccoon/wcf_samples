using System.Runtime.Serialization;
using WA.Contracts.Core;

namespace WA.Contracts.Orders.Messages.CompleteOrder
{
    [DataContract]
    public class CompleteOrderRequest : BaseRequest
    {
        [DataMember]
        public int OrderId { get; set; }
    }
}