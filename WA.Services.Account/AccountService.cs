using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using WA.Account.AccountService;
using WA.Account.Contracts.CheckToken;
using WA.Account.Contracts.Login;

namespace WA.Services.Account
{
    public class AccountService : IAccountService
    {
        private ILogger<AccountService> _logger { get; set; }

        public AccountService(ILogger<AccountService> logger)
        {
            _logger = logger;

            _logger.LogInformation($"AccountService instance created on thread {Thread.CurrentThread.ManagedThreadId}");
        }

        public CheckTokenResponse CheckToken(CheckTokenRequest request)
        {
            throw new NotImplementedException();
        }

        public LoginResponse Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}