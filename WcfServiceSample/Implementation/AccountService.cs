using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WcfServiceSample.BaseContracts;
using WcfServiceSample.DataMock;
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
            try
            {
                if (string.IsNullOrEmpty(request.ApplicationToken) || string.IsNullOrEmpty(request.User))
                {
                    return new LoginResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails() {
                            Code = eErrorCodes.InvalidRequest,
                            Message = eErrorCodes.InvalidRequesMessage
                        }
                    };
                }

                var acc = AccountTable.Instance.Find(m => m.User == request.User && m.AuthToken == request.ApplicationToken);

                if (acc == null)
                {
                    return new LoginResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails()
                        {
                            Code = eErrorCodes.LoginFailed,
                            Message = eErrorCodes.LoginFailedMessage
                        }
                    };
                }

                SessionData session = null;
                if (SessionStorage.Instance.TryGetValue(acc.Id, out session))
                {
                    if((DateTime.Now - session.LastCheck) > SessionStorage.SessionExpired)
                    {
                        session.SessionToken = Guid.NewGuid().ToString();
                        session.LoginTime = DateTime.Now;
                    }

                    session.LastCheck = DateTime.Now;
                }
                else
                {
                    session = new SessionData()
                    {
                        LastCheck = DateTime.Now,
                        LoginTime = DateTime.Now,
                        SessionToken = Guid.NewGuid().ToString(),
                        UserId = acc.Id
                    };
                }

                SessionStorage.Instance.AddOrUpdate(acc.Id,session, (id,s) => { return session; });

                return new LoginResponse()
                {
                    IsSuccess = true,
                    Token = session.SessionToken,
                    UserId = session.UserId
                };

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error when execute login {ex}");

                return new LoginResponse()
                {
                    IsSuccess = false,
                    Error = new ErrorDetails()
                    {
                        Code = eErrorCodes.ServerError,
                        Message = eErrorCodes.ServerErrorMessage
                    }
                };
            }
        }
    }
}