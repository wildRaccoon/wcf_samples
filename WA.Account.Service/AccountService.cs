using System;
using WA.Account.AccountService;
using WA.Account.Contracts.CheckToken;
using WA.Account.Contracts.Login;

namespace WA.Account.Service
{
    public class AccountService : IAccountService
    {
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
