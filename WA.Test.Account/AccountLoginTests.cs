using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
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
                    .UseInMemoryDatabase("Add_writes_to_database")
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
                    .UseInMemoryDatabase("Add_writes_to_database")
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
                    .UseInMemoryDatabase("Add_writes_to_database")
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
                Assert.IsNotNull(user);

                Assert.AreEqual(1, status.FailLoginCount);
                Assert.IsFalse(status.IsLocked);
                Assert.AreEqual(status.LastLoginFrom, req.RequestFrom);
            }
        }
    }
}
