using System.Runtime.Serialization;
using WA.Contracts.Core;

namespace WA.Contracts.Orders.Messages.CreateOrder
{
    [DataContract]
    public class CreateOrderResponse : BaseResponse
    {
        [DataMember]
        public OrderDetails Order { get; set; }
    }
}