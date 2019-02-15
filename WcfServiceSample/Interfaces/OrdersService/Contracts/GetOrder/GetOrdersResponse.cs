using System.Collections.Generic;
using System.Runtime.Serialization;
using WcfServiceSample.BaseContracts;

namespace WcfServiceSample.Interfaces.OrdersService.Contracts
{
    [DataContract]
    public class GetOrdersResponse : BaseResponse
    {
        [DataMember]
        public List<OrderDetails> Orders { get; set; }
    }
}