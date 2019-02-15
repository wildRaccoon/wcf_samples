using System.Runtime.Serialization;
using WcfServiceSample.BaseContracts;
using WcfServiceSample.DataMock;

namespace WcfServiceSample.Interfaces.OrdersService.Contracts
{
    [DataContract]
    public class GetOrdersRequest : BaseRequest
    {
        [DataMember]
        public eOrderStatus Status { get; set; } = eOrderStatus.None;
    }
}