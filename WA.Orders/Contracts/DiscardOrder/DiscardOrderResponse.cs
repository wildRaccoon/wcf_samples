using System.Runtime.Serialization;
using WA.Core;

namespace WA.Orders.Contracts.DiscardOrder
{
    [DataContract]
    public class DiscardOrderResponse : BaseResponse
    {
        [DataMember]
        public OrderDetails Order { get; set; }
    }
}