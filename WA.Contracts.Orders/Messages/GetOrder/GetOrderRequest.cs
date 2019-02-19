using System.Runtime.Serialization;
using WA.Contracts.Core;

namespace WA.Contracts.Orders.Messages.GetOrder
{
    [DataContract]
    public class GetOrdersRequest : BaseRequest
    {
        [DataMember]
        public eOrderStatus Status { get; set; } = eOrderStatus.None;
    }
}