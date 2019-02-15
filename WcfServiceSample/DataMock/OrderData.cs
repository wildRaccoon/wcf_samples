using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServiceSample.DataMock
{
    public class OrderData
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Completed { get; set; }

        public eOrderStatus Status { get; set; }
    }
}