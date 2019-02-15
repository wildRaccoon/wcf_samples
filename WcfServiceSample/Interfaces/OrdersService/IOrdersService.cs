using System.ServiceModel;
using System.ServiceModel.Web;
using WcfServiceSample.Interfaces.OrdersService.Contracts;

namespace WcfServiceSample.Interfaces.OrdersService
{
    [ServiceContract(Namespace = "http://wr.com/IOrdersService")]
    public interface IOrdersService
    {
        [OperationContract]
        [WebInvoke(ResponseFormat =WebMessageFormat.Json)]
        GetOrdersResponse GetOrders(GetOrdersRequest request);

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        CreateOrderResponse CreateOrder(CreateOrderRequest request);

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        CompleteOrderResponse CompleteOrder(CompleteOrderRequest request);

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        DiscardOrderResponse DiscardOrder(DiscardOrderRequest request);
    }
}
