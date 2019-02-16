using System.Runtime.Serialization;
using WcfServiceSample.BaseContracts;

namespace WcfServiceSample.Interfaces.OrdersService.Contracts
{
    [DataContract]
    public class CreateOrderResponse : BaseResponse
    {
        [DataMember]
        public OrderDetails Order { get; set; }
    }
}