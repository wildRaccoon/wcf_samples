using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfServiceSample.Interfaces.OrdersService;
using WcfServiceSample.Interfaces.OrdersService.Contracts;

namespace WcfServiceSample.Implementation
{
    public class OrdersService : IOrdersService
    {
        public OrdersService()
        {
        }

        #region CompleteOrder
        public CompleteOrderResponse CompleteOrder(CompleteOrderRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CreateOrder
        public CreateOrderResponse CreateOrder(CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DiscardOrder
        public DiscardOrderResponse DiscardOrder(DiscardOrderRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GetOrders
        public GetOrdersResponse GetOrders(GetOrdersRequest request)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}