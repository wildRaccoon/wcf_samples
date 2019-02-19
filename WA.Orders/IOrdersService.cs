using System.ServiceModel;
using WA.Orders.Contracts.CompleteOrder;
using WA.Orders.Contracts.CreateOrder;
using WA.Orders.Contracts.DiscardOrder;
using WA.Orders.Contracts.GetOrder;

namespace WA.Orders
{
    [ServiceContract(Namespace = "http://wr.com/IOrdersService")]
    public interface IOrdersService
    {
        [OperationContract]
        GetOrdersResponse GetOrders(GetOrdersRequest request);

        [OperationContract]
        CreateOrderResponse CreateOrder(CreateOrderRequest request);

        [OperationContract]
        CompleteOrderResponse CompleteOrder(CompleteOrderRequest request);

        [OperationContract]
        DiscardOrderResponse DiscardOrder(DiscardOrderRequest request);
    }
}
