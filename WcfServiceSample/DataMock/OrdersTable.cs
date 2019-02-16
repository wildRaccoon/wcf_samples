using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServiceSample.DataMock
{
    public class OrdersTable
    {
        public static List<OrderData> Instance = new List<OrderData>()
        {
            new OrderData{
                Id = 1,
                Completed = DateTime.Now.AddMinutes(-1),
                Created = DateTime.Now.AddMinutes(-2),
                Status = eOrderStatus.Discarded,
                UserId = 1
            },
             new OrderData{
                Id = 2,
                Completed = DateTime.Now.AddMinutes(-1),
                Created = DateTime.Now.AddMinutes(-2),
                Status = eOrderStatus.Completed,
                UserId = 2
            },
            new OrderData{
                Id = 3,
                Completed = DateTime.Now.AddMinutes(-1),
                Created = DateTime.Now.AddMinutes(-2),
                Status = eOrderStatus.Completed,
                UserId = 2
            },
            new OrderData{
                Id = 4,
                Completed = DateTime.Now.AddMinutes(-1),
                Created = DateTime.MinValue,
                Status = eOrderStatus.Created,
                UserId = 2
            },
       };
    }
}