using System.Runtime.Serialization;
using WcfServiceSample.BaseContracts;

namespace WcfServiceSample.Interfaces.OrdersService.Contracts
{
    [DataContract]
    public class DiscardOrderRequest : BaseRequest
    {
        [DataMember]
        public int OrderId { get; set; }
    }
}