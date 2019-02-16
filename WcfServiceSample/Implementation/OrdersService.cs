using System;
using System.Diagnostics;
using System.Linq;
using WcfServiceSample.BaseContracts;
using WcfServiceSample.DataMock;
using WcfServiceSample.Interfaces.OrdersService;
using WcfServiceSample.Interfaces.OrdersService.Contracts;

namespace WcfServiceSample.Implementation
{
    public class OrdersService : IOrdersService
    {
        public OrdersService()
        {
        }

        private Tuple<SessionData, AccountData, Res> CheckRequest<Req, Res>(Req request)
                where Res : BaseResponse, new()
                where Req : BaseRequest
        {
            SessionData session = null;

            if (SessionStorage.Instance.TryGetValue(request.UserId, out session) && session?.SessionToken != request.Token)
            {
                return Tuple.Create<SessionData, AccountData, Res>(null,null,new Res()
                {
                    IsSuccess = false,
                    Error = new ErrorDetails()
                    {
                        Code = eErrorCodes.InvalidRequest,
                        Message = eErrorCodes.InvalidRequesMessage
                    }
                });
            }

            var acc = AccountTable.Instance.Find(m => m.Id == request.UserId);

            if (acc == null)
            {
                Trace.WriteLine($"Error when execute {typeof(Res).Name}: user not found {request.UserId}");
                return Tuple.Create<SessionData, AccountData, Res>(null, null, new Res()
                {
                    IsSuccess = false,
                    Error = new ErrorDetails()
                    {
                        Code = eErrorCodes.InvalidRequest,
                        Message = eErrorCodes.InvalidRequesMessage
                    }
                });
            }

            return Tuple.Create<SessionData, AccountData, Res>(session, acc, null);
        }

        #region CompleteOrder
        public CompleteOrderResponse CompleteOrder(CompleteOrderRequest request)
        {
            var (session, acc, resp) = CheckRequest<CompleteOrderRequest, CompleteOrderResponse>(request);

            if (resp != null)
            {
                return resp;
            }

            if (acc.UserRole != eUserRole.Manager && acc.UserRole != eUserRole.Admin)
            {
                return new CompleteOrderResponse()
                {
                    Error = new ErrorDetails()
                    {
                        Code = eErrorCodes.NotAllowedRequest,
                        Message = eErrorCodes.NotAllowedRequesMessage
                    },
                    IsSuccess = false
                };
            }

            var order = OrdersTable.Instance.Find(i => i.Id == request.OrderId);


            order.Completed = DateTime.Now;
            order.Status = eOrderStatus.Completed;
            order.UserId = acc.Id;

            return new CompleteOrderResponse()
            {
                IsSuccess = true,
                Order = new OrderDetails(order)
                {
                    UserName = acc.User
                }
            };
        }
        #endregion

        #region CreateOrder
        public CreateOrderResponse CreateOrder(CreateOrderRequest request)
        {
            var (session, acc, resp) = CheckRequest<CreateOrderRequest, CreateOrderResponse>(request);

            if (resp != null)
            {
                return resp;
            }

            if (acc.UserRole != eUserRole.Manager && acc.UserRole != eUserRole.Admin)
            {
                return new CreateOrderResponse()
                {
                    Error = new ErrorDetails()
                    {
                        Code = eErrorCodes.NotAllowedRequest,
                        Message = eErrorCodes.NotAllowedRequesMessage
                    },
                    IsSuccess = false
                };
            }

            var maxId = OrdersTable.Instance.Max(i => i.Id) + 1;

            var order = new OrderData()
            {
                Id = maxId,
                UserId = acc.Id,
                Created = DateTime.Now,
                Status = eOrderStatus.Created
            };

            OrdersTable.Instance.Add(order);

            return new CreateOrderResponse()
            {
                IsSuccess = true,
                Order = new OrderDetails(order) {
                    UserName = acc.User
                }
            };
        }
        #endregion

        #region DiscardOrder
        public DiscardOrderResponse DiscardOrder(DiscardOrderRequest request)
        {
            var (session, acc, resp) = CheckRequest<DiscardOrderRequest, DiscardOrderResponse>(request);

            if (resp != null)
            {
                return resp;
            }

            if (acc.UserRole != eUserRole.Admin)
            {
                return new DiscardOrderResponse()
                {
                    Error = new ErrorDetails() {
                        Code = eErrorCodes.NotAllowedRequest,
                        Message = eErrorCodes.NotAllowedRequesMessage
                    },
                    IsSuccess = false
                };
            }

            var order = OrdersTable.Instance.Find(i => i.Id == request.OrderId);


            order.Completed = DateTime.Now;
            order.Status = eOrderStatus.Discarded;
            order.UserId = acc.Id;

            return new DiscardOrderResponse()
            {
                IsSuccess = true,
                Order = new OrderDetails(order)
                {
                    UserName = acc.User
                }
            };
        }
        #endregion

        #region GetOrders
        public GetOrdersResponse GetOrders(GetOrdersRequest request)
        {
            var (session, account, resp) = CheckRequest<GetOrdersRequest, GetOrdersResponse>(request);

            if (resp != null)
            {
                return resp;
            }

            var query = from order in OrdersTable.Instance
                        join acc in AccountTable.Instance on order.UserId equals acc.Id
                        where
                            (request.Status == eOrderStatus.None || request.Status == order.Status)
                        select
                            new OrderDetails(order)
                            {
                                UserName = acc.User
                            };

            return new GetOrdersResponse()
            {
                IsSuccess = true,
                Orders = query.ToList()
            };
        } 
        #endregion
    }
}