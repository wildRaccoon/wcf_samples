using System.Collections.Generic;
using System.Runtime.Serialization;
using WA.Contracts.Core;

namespace WA.Contracts.Orders.Messages.GetOrder
{
    [DataContract]
    public class GetOrdersResponse : BaseResponse
    {
        [DataMember]
        public List<OrderDetails> Orders { get; set; }
    }
}