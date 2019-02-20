using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WA.Contracts.Account;
using WA.Contracts.Account.Messages.Login;
using WA.Contracts.Core;
using WA.Repository.Account;
using WA.Repository.Account.Data;
using WA.Services.Account;

namespace WA.Test.Account
{
    [TestClass]
    public class AccountLoginTests
    {
        [TestMethod]
        public void LoginEmptyRequest()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                    .UseInMemoryDatabase("LoginEmptyRequest")
                    .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings());

                var resp = service.Login(new LoginRequest());

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);
            }
        }

        [TestMethod]
        public void LoginNotFound()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                    .UseInMemoryDatabase("LoginNotFound")
                    .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings());

                var resp = service.Login(new LoginRequest() {
                    Login = "l",
                    Password = "p",
                    RequestFrom = "rf"
                });

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);
            }
        }

        [TestMethod]
        public void LoginInvalidPassword()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                    .UseInMemoryDatabase("LoginInvalidPassword")
                    .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                var req = new LoginRequest()
                {
                    Login = "l",
                    Password = "pl",
                    RequestFrom = "rf"
                };

                accountDataContext.Users.Add(new UserData()
                {
                    Id = 1,
                    Login = req.Login,
                    Password = req.Password + "salt"
                });
                accountDataContext.SaveChanges();

                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings());

                var resp = service.Login(req);

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);

                var user = accountDataContext.Users.Single(u => u.Login == req.Login);
                Assert.IsNotNull(user);

                var status = user.Status;
                Assert.IsNotNull(status);

                Assert.AreEqual(1, status.FailLoginCount);
                Assert.IsFalse(status.IsLocked);
                Assert.AreEqual(status.LastLoginFrom, req.RequestFrom);
            }
        }

        [TestMethod]
        public void LoginLockAccount()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                    .UseInMemoryDatabase("LoginLockAccount")
                    .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                var req = new LoginRequest()
                {
                    Login = "l",
                    Password = "pl",
                    RequestFrom = "rf"
                };

                accountDataContext.Users.Add(new UserData()
                {
                    Id = 1,
                    Login = req.Login,
                    Password = req.Password + "salt"
                });
                accountDataContext.SaveChanges();

                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings() {
                    MaximuFailedLoginCount = 3
                });

                // 1 request
                var resp = service.Login(req);

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);

                var user = accountDataContext.Users.Single(u => u.Login == req.Login);
                Assert.IsNotNull(user);

                var status = accountDataContext.UserStatuses.Single(us => us.UserId == user.Id);
                Assert.IsNotNull(status);

                Assert.AreEqual(1, status.FailLoginCount);
                Assert.IsFalse(status.IsLocked);
                Assert.AreEqual(status.LastLoginFrom, req.RequestFrom);

                // 2 request
                req.RequestFrom += "1";
                resp = service.Login(req);

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);

                status = accountDataContext.UserStatuses.Single(us => us.UserId == user.Id);
                Assert.IsNotNull(user);

                Assert.AreEqual(2, status.FailLoginCount);
                Assert.IsFalse(status.IsLocked);
                Assert.AreEqual(status.LastLoginFrom, req.RequestFrom);

                // 3 request
                req.RequestFrom += "1";
                resp = service.Login(req);

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eErrorCodes.InvalidRequest, resp.Error.Code);
                Assert.AreEqual("Invalid Request", resp.Error.Message);

                status = accountDataContext.UserStatuses.Single(us => us.UserId == user.Id);
                Assert.IsNotNull(status);

                Assert.AreEqual(3, status.FailLoginCount);
                Assert.IsTrue(status.IsLocked);
                Assert.AreEqual(status.LastLoginFrom, req.RequestFrom);


                // lock request
                req.RequestFrom += "1";
                req.Password = user.Password;
                resp = service.Login(req);

                Assert.IsNotNull(resp);
                Assert.IsFalse(resp.IsSuccess);
                Assert.IsNotNull(resp.Error);
                Assert.AreEqual((int)eAccountErrorCodes.AccountLocked, resp.Error.Code);
                Assert.AreEqual("Account Locked", resp.Error.Message);

                status = accountDataContext.UserStatuses.Single(us => us.UserId == user.Id);
                Assert.IsNotNull(status);

                Assert.AreEqual(3, status.FailLoginCount);
                Assert.IsTrue(status.IsLocked);
                Assert.AreEqual(status.LastLoginFrom, req.RequestFrom);
            }
        }

        [TestMethod]
        public void LoginSuccess()
        {
            var options = new DbContextOptionsBuilder<AccountDataContext>()
                    .UseInMemoryDatabase("LoginSuccess")
                    .Options;

            using (var accountDataContext = new AccountDataContext(options))
            {
                var req = new LoginRequest()
                {
                    Login = "l",
                    Password = "pl",
                    RequestFrom = "rf"
                };

                accountDataContext.Users.Add(new UserData()
                {
                    Id = 1,
                    Login = req.Login,
                    Password = req.Password
                });
                accountDataContext.SaveChanges();

                var logger = new Moq.Mock<ILogger<AccountService>>();

                var service = new AccountService(logger.Object, accountDataContext, new AccountServiceSettings()
                {
                    MaximuFailedLoginCount = 3
                });

                var resp = service.Login(req);

                Assert.IsNotNull(resp);
                Assert.IsNotNull(resp.Token);
                Assert.IsTrue(resp.IsSuccess);
                Assert.IsNull(resp.Error);

                var user = accountDataContext.Users.Single(u => u.Login == req.Login);
                Assert.IsNotNull(user);
                Assert.AreEqual(user.Id, resp.UserId);

                var status = accountDataContext.UserStatuses.Single(us => us.UserId == user.Id);
                Assert.IsNotNull(status);
                Assert.AreEqual(0, status.FailLoginCount);
                Assert.IsFalse(status.IsLocked);
                Assert.AreEqual(status.LastLoginFrom, req.RequestFrom);

                var session = accountDataContext.Sessions.Single(s => s.SessionToken == resp.Token);
                Assert.IsNotNull(session);
                Assert.AreEqual(user.Id,session.UserId);
                Assert.AreEqual(req.RequestFrom, session.ConnectedFrom);
            }
        }
    }
}
