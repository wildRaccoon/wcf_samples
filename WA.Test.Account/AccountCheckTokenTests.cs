using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WA.Contracts.Account.Messages.CheckToken;
using WA.Contracts.Core;
using WA.Repository.Account;
using WA.Repository.Account.Data;
using WA.Services.Account;

namespace WA.Test.Account
{
    [TestClass]
    public class AccountCheckTokenTests
    {
        [TestMethod]
        public void CheckTokenEmptyRequest()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                .UseInMemoryDatabase("CheckTokenEmptyRequest")
                .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings());

                var resp = service.CheckToken(new CheckTokenRequest());

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);

                resp = service.CheckToken(new CheckTokenRequest() { RequestFrom = "1" });

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);

                resp = service.CheckToken(new CheckTokenRequest() { RequestFrom = "1", SessionToken = "1" });

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);
            }
        }

        [TestMethod]
        public void CheckTokenNotFoundAccount()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                .UseInMemoryDatabase("CheckTokenNotFoundAccount")
                .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings());

                var resp = service.CheckToken(new CheckTokenRequest()
                {
                    RequestFrom = "1",
                    SessionToken = "2",
                    UserId = 3
                });

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);

                Assert.AreEqual(2, logger.Invocations.Count);

                LogLevel? currentLogLevel = logger.Invocations[1].Arguments[0] as LogLevel?;
                Assert.AreEqual(LogLevel.Information, currentLogLevel.GetValueOrDefault(LogLevel.None));

                Exception logException = logger.Invocations[1].Arguments[3] as Exception;
                object logItem = logger.Invocations[1].Arguments[2];
                Func<object, Exception, string> formater = logger.Invocations[1].Arguments[4] as Func<object, Exception, string>;

                var logMessage = formater(logItem, logException);
                Assert.AreEqual("CheckToken attempt by undefined id 3.", logMessage);
            }
        }

        [TestMethod]
        public void CheckTokenNotFoundSession()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                .UseInMemoryDatabase("CheckTokenNotFoundSession")
                .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                accountDataContext.Users.Add(new UserData()
                {
                    Login = "login",
                    Name = "name",
                    Password = "p",
                    RoleId = 1
                });
                accountDataContext.SaveChanges();

                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings());

                var resp = service.CheckToken(new CheckTokenRequest()
                {
                    RequestFrom = "1",
                    SessionToken = "2",
                    UserId = 1
                });

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);
            }
        }

        [TestMethod]
        public void CheckTokenSessionOtherToken()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                .UseInMemoryDatabase("CheckTokenSessionOtherToken")
                .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                accountDataContext.Users.Add(new UserData()
                {
                    Login = "login",
                    Name = "name",
                    Password = "p",
                    RoleId = 1
                });

                accountDataContext.Sessions.Add(new SessionData()
                {
                    ConnectedFrom = "f",
                    SessionToken = "t1",
                    UserId = 1
                });
                accountDataContext.SaveChanges();

                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings());

                var resp = service.CheckToken(new CheckTokenRequest()
                {
                    RequestFrom = "f",
                    SessionToken = "t2",
                    UserId = 1
                });

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);
            }
        }

        [TestMethod]
        public void CheckTokenSessionOtherConnectedFrom()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                .UseInMemoryDatabase("CheckTokenSessionOtherConnectedFrom")
                .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                accountDataContext.Users.Add(new UserData()
                {
                    Login = "login",
                    Name = "name",
                    Password = "p",
                    RoleId = 1
                });

                accountDataContext.Sessions.Add(new SessionData()
                {
                    ConnectedFrom = "f1",
                    SessionToken = "t",
                    UserId = 1
                });
                accountDataContext.SaveChanges();

                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings());

                var resp = service.CheckToken(new CheckTokenRequest()
                {
                    RequestFrom = "f2",
                    SessionToken = "t",
                    UserId = 1
                });

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);
            }
        }

        [TestMethod]
        public void CheckTokenSessionExpired()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                .UseInMemoryDatabase("CheckTokenSessionExpired")
                .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                var settings = new AccountServiceSettings();

                accountDataContext.Users.Add(new UserData()
                {
                    Login = "login",
                    Name = "name",
                    Password = "p",
                    RoleId = 1
                });

                accountDataContext.Sessions.Add(new SessionData()
                {
                    ConnectedFrom = "f1",
                    SessionToken = "t",
                    Created = DateTime.MinValue,
                    LastCheck = DateTime.Now.AddSeconds(-settings.SessionExpirationTime.TotalSeconds-10),
                    UserId = 1
                });
                accountDataContext.SaveChanges();

                var logger = new Moq.Mock<ILogger<AccountService>>();                

                var service = new AccountService(logger.Object, accountDataContext, settings);

                var resp = service.CheckToken(new CheckTokenRequest()
                {
                    RequestFrom = "f1",
                    SessionToken = "t",
                    UserId = 1
                });

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);
            }
        }

        [TestMethod]
        public void CheckTokenSessionSuccess()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                .UseInMemoryDatabase("CheckTokenSessionSuccess")
                .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                accountDataContext.Users.Add(new UserData()
                {
                    Id = 1,
                    Login = "login",
                    Name = "name",
                    Password = "p",
                    RoleId = 1
                });

                accountDataContext.Sessions.Add(new SessionData()
                {
                    ConnectedFrom = "f1",
                    SessionToken = "t",
                    Created = DateTime.Now,
                    LastCheck = DateTime.Now,
                    UserId = 1
                });
                accountDataContext.SaveChanges();

                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings());

                var resp = service.CheckToken(new CheckTokenRequest()
                {
                    RequestFrom = "f1",
                    SessionToken = "t",
                    UserId = 1
                });

                Assert.IsNotNull(resp);
                Assert.IsTrue(resp.IsSuccess);
                Assert.IsNull(resp.Error);
            }
        }
    }
}
