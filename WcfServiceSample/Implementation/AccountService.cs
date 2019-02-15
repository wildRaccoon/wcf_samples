using System;
using System.Diagnostics;
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

        #region Check Token
        public CheckTokenResponse CheckToken(CheckTokenRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.SessionToken) || request.UserId <= 0)
                {
                    return new CheckTokenResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails()
                        {
                            Code = eErrorCodes.InvalidRequest,
                            Message = eErrorCodes.InvalidRequesMessage
                        }
                    };
                }

                var acc = AccountTable.Instance.Find(m => m.Id == request.UserId);

                if (acc == null)
                {
                    Trace.WriteLine($"Error when execute CheckToken: user not found {request.UserId}");
                    return new CheckTokenResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails()
                        {
                            Code = eErrorCodes.InvalidRequest,
                            Message = eErrorCodes.InvalidRequesMessage
                        }
                    };
                }

                SessionData session = null;
                if (SessionStorage.Instance.TryGetValue(acc.Id, out session) && session?.SessionToken == request.SessionToken)
                {
                    var newSession = new SessionData(session)
                    {
                        LastCheck = DateTime.Now
                    };

                    SessionStorage.Instance.TryUpdate(acc.Id, newSession, session);

                    return new CheckTokenResponse()
                    {
                        IsSuccess = true
                    };
                }

                Trace.WriteLine($"Error when execute CheckToken: session not found {request.UserId}:{request.SessionToken}");
                return new CheckTokenResponse()
                {
                    IsSuccess = false,
                    Error = new ErrorDetails()
                    {
                        Code = eErrorCodes.InvalidRequest,
                        Message = eErrorCodes.InvalidRequesMessage
                    }
                };
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error when execute CheckToken {ex}");

                return new CheckTokenResponse()
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
        #endregion

        #region Login
        public LoginResponse Login(LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ApplicationToken) || string.IsNullOrEmpty(request.User))
                {
                    return new LoginResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails()
                        {
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
                    if ((DateTime.Now - session.LastCheck) > SessionStorage.SessionExpired)
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

                SessionStorage.Instance.AddOrUpdate(acc.Id, session, (id, s) => { return session; });

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
        #endregion
    }
}