using System.Collections.Generic;
using System.Runtime.Serialization;
using WA.Core;

namespace WA.Orders.Contracts.GetOrder
{
    [DataContract]
    public class GetOrdersResponse : BaseResponse
    {
        [DataMember]
        public List<OrderDetails> Orders { get; set; }
    }
}