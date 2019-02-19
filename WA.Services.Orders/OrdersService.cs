using Microsoft.Extensions.Logging;
using System;
using WA.Contracts.Orders;
using WA.Contracts.Orders.Messages.CompleteOrder;
using WA.Contracts.Orders.Messages.CreateOrder;
using WA.Contracts.Orders.Messages.DiscardOrder;
using WA.Contracts.Orders.Messages.GetOrder;

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
