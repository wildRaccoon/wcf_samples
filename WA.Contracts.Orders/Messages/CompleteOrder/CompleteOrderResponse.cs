using System.Runtime.Serialization;
using WA.Contracts.Core;

namespace WA.Contracts.Orders.Messages.CompleteOrder
{
    [DataContract]
    public class CompleteOrderResponse : BaseResponse
    {
        [DataMember]
        public OrderDetails Order { get; set; }
    }
}