using System;
using WA.Orders.Contracts.CompleteOrder;
using WA.Orders.Contracts.CreateOrder;
using WA.Orders.Contracts.DiscardOrder;
using WA.Orders.Contracts.GetOrder;

namespace WA.Orders.Service
{
    public class OrdersService : IOrdersService
    {
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
