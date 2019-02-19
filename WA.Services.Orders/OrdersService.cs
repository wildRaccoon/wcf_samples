using Microsoft.Extensions.Logging;
using System;
using WA.Orders;
using WA.Orders.Contracts.CompleteOrder;
using WA.Orders.Contracts.CreateOrder;
using WA.Orders.Contracts.DiscardOrder;
using WA.Orders.Contracts.GetOrder;

namespace WA.Services.Orders
{
    public class OrdersService : IOrdersService
    {
        private ILogger<OrdersService> _logger { get; set; }

        public OrdersService(ILogger<OrdersService> logger)
        {
            _logger = logger;
        }

        public CompleteOrderResponse CompleteOrder(CompleteOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public CreateOrderResponse CreateOrder(CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public DiscardOrderResponse DiscardOrder(DiscardOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public GetOrdersResponse GetOrders(GetOrdersRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
