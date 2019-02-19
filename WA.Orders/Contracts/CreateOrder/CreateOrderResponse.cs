using System.Runtime.Serialization;
using WA.Core;

namespace WA.Orders.Contracts.CreateOrder
{
    [DataContract]
    public class CreateOrderResponse : BaseResponse
    {
        [DataMember]
        public OrderDetails Order { get; set; }
    }
}