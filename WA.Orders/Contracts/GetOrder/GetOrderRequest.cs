using System.Runtime.Serialization;
using WA.Core;

namespace WA.Orders.Contracts.GetOrder
{
    [DataContract]
    public class GetOrdersRequest : BaseRequest
    {
        [DataMember]
        public eOrderStatus Status { get; set; } = eOrderStatus.None;
    }
}