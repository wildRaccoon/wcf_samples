using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfServiceSample.Interfaces.AccountService;
using WcfServiceSample.Interfaces.AccountService.Contracts.CheckToken;
using WcfServiceSample.Interfaces.AccountService.Contracts.Login;

namespace WcfServiceSample.Implementation
{
    public class AccountService : IAccountService
    {
        public AccountService()
        {
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