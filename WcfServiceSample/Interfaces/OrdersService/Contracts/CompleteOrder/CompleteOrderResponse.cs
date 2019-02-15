using System.Runtime.Serialization;
using WcfServiceSample.BaseContracts;

namespace WcfServiceSample.Interfaces.OrdersService.Contracts
{
    [DataContract]
    public class CompleteOrderResponse : BaseResponse
    {
        public OrderDetails Order { get; set; }
    }
}