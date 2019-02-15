using System.ServiceModel;
using WcfServiceSample.Interfaces.OrdersService.Contracts;

namespace WcfServiceSample.Interfaces.OrdersService
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
