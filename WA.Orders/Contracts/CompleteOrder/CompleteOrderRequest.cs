using System.Runtime.Serialization;
using WA.Core;

namespace WA.Orders.Contracts.CompleteOrder
{
    [DataContract]
    public class CompleteOrderRequest : BaseRequest
    {
        [DataMember]
        public int OrderId { get; set; }
    }
}