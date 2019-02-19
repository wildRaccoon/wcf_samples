using System.Runtime.Serialization;
using WA.Core;

namespace WA.Orders.Contracts.CompleteOrder
{
    [DataContract]
    public class CompleteOrderResponse : BaseResponse
    {
        [DataMember]
        public OrderDetails Order { get; set; }
    }
}