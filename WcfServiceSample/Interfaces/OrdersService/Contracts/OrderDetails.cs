using System;
using System.Runtime.Serialization;
using WcfServiceSample.DataMock;

namespace WcfServiceSample.Interfaces.OrdersService.Contracts
{
    [DataContract]
    public class OrderDetails
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public DateTime Completed { get; set; }

        [DataMember]
        public eOrderStatus Status { get; set; }

        [DataMember]
        public string UserName { get; set; }

        public OrderDetails()
        {
        }

        public OrderDetails(OrderData data)
        {
            Id = data.Id;
            Completed = data.Completed;
            Created = data.Created;
            Status = data.Status;
        }
    }
}