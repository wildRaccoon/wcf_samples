using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel.Web;
using WcfServiceSample.Implementation;
using System.ServiceModel;
using WcfServiceSample.Interfaces.AccountService;
using System.ServiceModel.Description;
using WcfServiceSample.Interfaces.OrdersService;
using WcfServiceSample.Interfaces.AccountService.Contracts.Login;
using WcfServiceSample.Interfaces.AccountService.Contracts.CheckToken;
using WcfServiceSample.Interfaces.OrdersService.Contracts;
using WcfServiceSample.DataMock;
using WcfServiceSample.BaseContracts;

namespace WATest
{
    [TestClass]
    public class UnitTest
    {
        #region preparation
        protected const string AccountUrl = "http://localhost:8090/API/AccountService.svc";
        protected const string OrdersUrl = "http://localhost:8090/API/OrdersService.svc";
        protected static WebServiceHost AccountHost = null;
        protected static IAccountService AccountService = null;
        protected static IOrdersService OrdersService = null;
        protected static WebServiceHost OrdersHost = null;

        protected static Tuple<WebServiceHost,IT> CreateHost<T, IT>(string url)
            where IT:class
        {
            var host = new WebServiceHost(
                typeof(T),
                new Uri(AccountUrl));

            var webBinding = new WebHttpBinding() { CrossDomainScriptAccessEnabled = true };

            var serviceEndPoint = host.AddServiceEndpoint(
                typeof(IT),
                webBinding,
                url);

            serviceEndPoint.EndpointBehaviors.Add(new WebHttpBehavior()
            {
                AutomaticFormatSelectionEnabled = true,
                DefaultOutgoingResponseFormat = WebMessageFormat.Json,
                HelpEnabled = true
            });

            host.Description.Behaviors.Add(new ServiceMetadataBehavior()
            {
                HttpGetEnabled = true,
                HttpGetBinding = webBinding,
                HttpsGetEnabled = true
            });

            host.Open();

            var factory = new WebChannelFactory<IT>(new WebHttpBinding(), new Uri(url));

            var service = factory.CreateChannel();


            return Tuple.Create(host,service);
        }

        [ClassInitialize]
        public static void UnitTestInit(TestContext testContext)
        {
            (AccountHost,AccountService) = CreateHost<AccountService, IAccountService>(AccountUrl);
            (OrdersHost,OrdersService) = CreateHost<OrdersService, IOrdersService>(OrdersUrl);
        }

        [ClassCleanup]
        public static void UnitTestClean()
        {
            if (AccountHost?.State == CommunicationState.Opened)
            {
                AccountHost.Close();
            }

            if (OrdersHost?.State == CommunicationState.Opened)
            {
                OrdersHost.Close();
            }
        } 
        #endregion

        [TestMethod]
        public void LoginAdmin()
        {
            var response = AccountService.Login(new LoginRequest()
            {
                ApplicationToken = "token1",
                User = "admin"
            });

            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Token);
            Assert.AreNotEqual(0, response.UserId);

            var respCheck = AccountService.CheckToken(new CheckTokenRequest()
            {
                SessionToken = response.Token,
                UserId = response.UserId
            });

            Assert.IsTrue(respCheck.IsSuccess);
            Assert.IsNull(respCheck.Error);
        }

        [TestMethod]
        public void LoginManager()
        {
            var response = AccountService.Login(new LoginRequest()
            {
                ApplicationToken = "token2",
                User = "manager"
            });

            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Token);
            Assert.AreNotEqual(0, response.UserId);

            var respCheck = AccountService.CheckToken(new CheckTokenRequest()
            {
                SessionToken = response.Token,
                UserId = response.UserId
            });

            Assert.IsTrue(respCheck.IsSuccess);
            Assert.IsNull(respCheck.Error);
        }

        [TestMethod]
        public void LoginGuest1()
        {
            var response = AccountService.Login(new LoginRequest()
            {
                ApplicationToken = "token3",
                User = "guest1"
            });

            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Token);
            Assert.AreNotEqual(0, response.UserId);

            var respCheck = AccountService.CheckToken(new CheckTokenRequest()
            {
                SessionToken = response.Token,
                UserId = response.UserId
            });

            Assert.IsTrue(respCheck.IsSuccess);
            Assert.IsNull(respCheck.Error);
        }

        [TestMethod]
        public void LoginGuest2()
        {
            var response = AccountService.Login(new LoginRequest()
            {
                ApplicationToken = "token4",
                User = "guest2"
            });

            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Token);
            Assert.AreNotEqual(0, response.UserId);

            var respCheck = AccountService.CheckToken(new CheckTokenRequest()
            {
                SessionToken = response.Token,
                UserId = response.UserId
            });

            Assert.IsTrue(respCheck.IsSuccess);
            Assert.IsNull(respCheck.Error);
        }

        #region Admin Orders Checks
        [TestMethod]
        public void AdminOrderDiscard()
        {
            var loginResp = AccountService.Login(new LoginRequest()
            {
                ApplicationToken = "token1",
                User = "admin"
            });

            Assert.IsTrue(loginResp.IsSuccess);
            Assert.IsNotNull(loginResp.Token);
            Assert.AreNotEqual(0, loginResp.UserId);

            CreateOrderResponse orderCreateResp = null;

            {
                //create new order
                orderCreateResp = OrdersService.CreateOrder(new CreateOrderRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId
                });

                Assert.IsTrue(orderCreateResp.IsSuccess);
                Assert.IsNotNull(orderCreateResp.Order);
                Assert.AreEqual(eOrderStatus.Created, orderCreateResp.Order.Status);
                Assert.AreEqual("admin", orderCreateResp.Order.UserName);
            }

            {
                //check is it in orders list
                var ordersListResp = OrdersService.GetOrders(new GetOrdersRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId,
                    Status = eOrderStatus.None
                });

                Assert.IsTrue(ordersListResp.IsSuccess);
                Assert.IsNotNull(ordersListResp.Orders);
                Assert.IsTrue(ordersListResp.Orders.Exists(o => o.Id == orderCreateResp.Order.Id && o.Status == eOrderStatus.Created));

            }

            {
                var discardResp = OrdersService.DiscardOrder(new DiscardOrderRequest()
                {
                    UserId = loginResp.UserId,
                    Token = loginResp.Token,
                    OrderId = orderCreateResp.Order.Id
                });

                Assert.IsTrue(discardResp.IsSuccess);
                Assert.IsNotNull(discardResp.Order);
                Assert.AreEqual(eOrderStatus.Discarded, discardResp.Order.Status);
                Assert.AreEqual("admin", discardResp.Order.UserName);
            }

            {
                //check is it in orders list
                var ordersListResp = OrdersService.GetOrders(new GetOrdersRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId,
                    Status = eOrderStatus.None
                });

                Assert.IsTrue(ordersListResp.IsSuccess);
                Assert.IsNotNull(ordersListResp.Orders);
                Assert.IsTrue(ordersListResp.Orders.Exists(o => o.Id == orderCreateResp.Order.Id && o.Status == eOrderStatus.Discarded));
            }
        }

        [TestMethod]
        public void AdminOrderComplete()
        {
            var loginResp = AccountService.Login(new LoginRequest()
            {
                ApplicationToken = "token1",
                User = "admin"
            });

            Assert.IsTrue(loginResp.IsSuccess);
            Assert.IsNotNull(loginResp.Token);
            Assert.AreNotEqual(0, loginResp.UserId);

            CreateOrderResponse orderCreateResp = null;

            {
                //create new order
                orderCreateResp = OrdersService.CreateOrder(new CreateOrderRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId
                });

                Assert.IsTrue(orderCreateResp.IsSuccess);
                Assert.IsNotNull(orderCreateResp.Order);
                Assert.AreEqual(eOrderStatus.Created, orderCreateResp.Order.Status);
                Assert.AreEqual("admin", orderCreateResp.Order.UserName);
            }

            {
                //check is it in orders list
                var ordersListResp = OrdersService.GetOrders(new GetOrdersRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId,
                    Status = eOrderStatus.None
                });

                Assert.IsTrue(ordersListResp.IsSuccess);
                Assert.IsNotNull(ordersListResp.Orders);
                Assert.IsTrue(ordersListResp.Orders.Exists(o => o.Id == orderCreateResp.Order.Id && o.Status == eOrderStatus.Created));

            }

            {
                var completeResp = OrdersService.CompleteOrder(new CompleteOrderRequest()
                {
                    UserId = loginResp.UserId,
                    Token = loginResp.Token,
                    OrderId = orderCreateResp.Order.Id
                });

                Assert.IsTrue(completeResp.IsSuccess);
                Assert.IsNotNull(completeResp.Order);
                Assert.AreEqual(eOrderStatus.Completed, completeResp.Order.Status);
                Assert.AreEqual("admin", completeResp.Order.UserName);
            }

            {
                //check is it in orders list
                var ordersListResp = OrdersService.GetOrders(new GetOrdersRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId,
                    Status = eOrderStatus.None
                });

                Assert.IsTrue(ordersListResp.IsSuccess);
                Assert.IsNotNull(ordersListResp.Orders);
                Assert.IsTrue(ordersListResp.Orders.Exists(o => o.Id == orderCreateResp.Order.Id && o.Status == eOrderStatus.Completed));
            }
        }
        #endregion

        #region Manager Orders Checks
        [TestMethod]
        public void ManagerOrderDiscard()
        {
            var loginResp = AccountService.Login(new LoginRequest()
            {
                ApplicationToken = "token2",
                User = "manager"
            });

            Assert.IsTrue(loginResp.IsSuccess);
            Assert.IsNotNull(loginResp.Token);
            Assert.AreNotEqual(0, loginResp.UserId);

            CreateOrderResponse orderCreateResp = null;

            {
                //create new order
                orderCreateResp = OrdersService.CreateOrder(new CreateOrderRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId
                });

                Assert.IsTrue(orderCreateResp.IsSuccess);
                Assert.IsNotNull(orderCreateResp.Order);
                Assert.AreEqual(eOrderStatus.Created, orderCreateResp.Order.Status);
                Assert.AreEqual("manager", orderCreateResp.Order.UserName);
            }

            {
                //check is it in orders list
                var ordersListResp = OrdersService.GetOrders(new GetOrdersRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId,
                    Status = eOrderStatus.None
                });

                Assert.IsTrue(ordersListResp.IsSuccess);
                Assert.IsNotNull(ordersListResp.Orders);
                Assert.IsTrue(ordersListResp.Orders.Exists(o => o.Id == orderCreateResp.Order.Id && o.Status == eOrderStatus.Created));

            }

            {
                var discardResp = OrdersService.DiscardOrder(new DiscardOrderRequest()
                {
                    UserId = loginResp.UserId,
                    Token = loginResp.Token,
                    OrderId = orderCreateResp.Order.Id
                });

                Assert.IsFalse(discardResp.IsSuccess);
                Assert.IsNull(discardResp.Order);
                Assert.IsNotNull(discardResp.Error);
                Assert.AreEqual(eErrorCodes.NotAllowedRequest, discardResp.Error.Code);
                Assert.AreEqual(eErrorCodes.NotAllowedRequesMessage, discardResp.Error.Message);
            }

            {
                //check is it in orders list
                var ordersListResp = OrdersService.GetOrders(new GetOrdersRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId,
                    Status = eOrderStatus.None
                });

                Assert.IsTrue(ordersListResp.IsSuccess);
                Assert.IsNotNull(ordersListResp.Orders);
                Assert.IsTrue(ordersListResp.Orders.Exists(o => o.Id == orderCreateResp.Order.Id && o.Status == eOrderStatus.Created));
            }
        }

        [TestMethod]
        public void ManagerOrderComplete()
        {
            var loginResp = AccountService.Login(new LoginRequest()
            {
                ApplicationToken = "token2",
                User = "manager"
            });

            Assert.IsTrue(loginResp.IsSuccess);
            Assert.IsNotNull(loginResp.Token);
            Assert.AreNotEqual(0, loginResp.UserId);

            CreateOrderResponse orderCreateResp = null;

            {
                //create new order
                orderCreateResp = OrdersService.CreateOrder(new CreateOrderRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId
                });

                Assert.IsTrue(orderCreateResp.IsSuccess);
                Assert.IsNotNull(orderCreateResp.Order);
                Assert.AreEqual(eOrderStatus.Created, orderCreateResp.Order.Status);
                Assert.AreEqual("manager", orderCreateResp.Order.UserName);
            }

            {
                //check is it in orders list
                var ordersListResp = OrdersService.GetOrders(new GetOrdersRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId,
                    Status = eOrderStatus.None
                });

                Assert.IsTrue(ordersListResp.IsSuccess);
                Assert.IsNotNull(ordersListResp.Orders);
                Assert.IsTrue(ordersListResp.Orders.Exists(o => o.Id == orderCreateResp.Order.Id && o.Status == eOrderStatus.Created));

            }

            {
                var completeResp = OrdersService.CompleteOrder(new CompleteOrderRequest()
                {
                    UserId = loginResp.UserId,
                    Token = loginResp.Token,
                    OrderId = orderCreateResp.Order.Id
                });

                Assert.IsTrue(completeResp.IsSuccess);
                Assert.IsNotNull(completeResp.Order);
                Assert.AreEqual(eOrderStatus.Completed, completeResp.Order.Status);
                Assert.AreEqual("manager", completeResp.Order.UserName);
            }

            {
                //check is it in orders list
                var ordersListResp = OrdersService.GetOrders(new GetOrdersRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId,
                    Status = eOrderStatus.None
                });

                Assert.IsTrue(ordersListResp.IsSuccess);
                Assert.IsNotNull(ordersListResp.Orders);
                Assert.IsTrue(ordersListResp.Orders.Exists(o => o.Id == orderCreateResp.Order.Id && o.Status == eOrderStatus.Completed));
            }
        }
        #endregion

        #region Manager Orders Checks
        [TestMethod]
        public void GuestOrderSession()
        {
            var loginResp = AccountService.Login(new LoginRequest()
            {
                ApplicationToken = "token3",
                User = "guest1"
            });

            Assert.IsTrue(loginResp.IsSuccess);
            Assert.IsNotNull(loginResp.Token);
            Assert.AreNotEqual(0, loginResp.UserId);

            {
                //try to create new order
                var orderCreateResp = OrdersService.CreateOrder(new CreateOrderRequest()
                {
                    Token = loginResp.Token,
                    UserId = loginResp.UserId
                });

                Assert.IsFalse(orderCreateResp.IsSuccess);
                Assert.IsNull(orderCreateResp.Order);
                Assert.IsNotNull(orderCreateResp.Error);
                Assert.AreEqual(eErrorCodes.NotAllowedRequest, orderCreateResp.Error.Code);
                Assert.AreEqual(eErrorCodes.NotAllowedRequesMessage, orderCreateResp.Error.Message);
            }

            {
                //check is it in orders list
                var ordersListResp = OrdersService.GetOrders(new GetOrdersRequest()
                {
                    UserId = loginResp.UserId,
                    Token = loginResp.Token,
                    Status = eOrderStatus.None
                });

                Assert.IsTrue(ordersListResp.IsSuccess);
                Assert.IsNotNull(ordersListResp.Orders);
                Assert.IsTrue(ordersListResp.Orders.Count > 0);

            }

            {
                var discardResp = OrdersService.DiscardOrder(new DiscardOrderRequest()
                {
                    UserId = loginResp.UserId,
                    Token = loginResp.Token,
                    OrderId = 1
                });

                Assert.IsFalse(discardResp.IsSuccess);
                Assert.IsNull(discardResp.Order);
                Assert.IsNotNull(discardResp.Error);
                Assert.AreEqual(eErrorCodes.NotAllowedRequest, discardResp.Error.Code);
                Assert.AreEqual(eErrorCodes.NotAllowedRequesMessage, discardResp.Error.Message);
            }

            {
                var completeResp = OrdersService.CompleteOrder(new CompleteOrderRequest()
                {
                    UserId = loginResp.UserId,
                    Token = loginResp.Token,
                    OrderId = 1
                });

                Assert.IsFalse(completeResp.IsSuccess);
                Assert.IsNull(completeResp.Order);
                Assert.IsNotNull(completeResp.Error);
                Assert.AreEqual(eErrorCodes.NotAllowedRequest, completeResp.Error.Code);
                Assert.AreEqual(eErrorCodes.NotAllowedRequesMessage, completeResp.Error.Message);
            }
        }
        #endregion
    }
}
