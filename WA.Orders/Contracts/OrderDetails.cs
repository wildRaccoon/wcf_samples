using System;
using System.Runtime.Serialization;


namespace WA.Orders.Contracts
{
    [DataContract]
    public class OrderDetails
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime? Created { get; set; }

        [DataMember]
        public DateTime? Completed { get; set; }

        [DataMember]
        public eOrderStatus Status { get; set; }

        [DataMember]
        public string UserName { get; set; }

        public OrderDetails()
        {
        }
    }
}