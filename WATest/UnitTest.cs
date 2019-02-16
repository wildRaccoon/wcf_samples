using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel.Web;
using WcfServiceSample.Implementation;
using System.ServiceModel;
using WcfServiceSample.Interfaces.AccountService;
using System.ServiceModel.Description;
using WcfServiceSample.Interfaces.OrdersService;
using WcfServiceSample.Interfaces.AccountService.Contracts.Login;

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

            var factory = new WebChannelFactory<IT>(new WebHttpBinding(), new Uri(AccountUrl));

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
        }
    }
}
