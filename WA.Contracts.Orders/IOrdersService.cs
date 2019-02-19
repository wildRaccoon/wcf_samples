using System.ServiceModel;
using WA.Contracts.Orders.Messages.CompleteOrder;
using WA.Contracts.Orders.Messages.CreateOrder;
using WA.Contracts.Orders.Messages.DiscardOrder;
using WA.Contracts.Orders.Messages.GetOrder;

namespace WA.Contracts.Orders
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
