using System.Runtime.Serialization;
using WcfServiceSample.BaseContracts;

namespace WcfServiceSample.Interfaces.OrdersService.Contracts
{
    [DataContract]
    public class CreateOrderResponse : BaseResponse
    {
        public OrderDetails Order { get; set; }
    }
}