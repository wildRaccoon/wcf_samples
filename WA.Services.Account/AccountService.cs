﻿using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using WA.Contracts.Account;
using WA.Contracts.Account.Messages.CheckToken;
using WA.Contracts.Account.Messages.Login;
using WA.Contracts.Core;
using WA.Repository.Account;
using WA.Repository.Account.Data;

namespace WA.Services.Account
{
    public class AccountService : IAccountService
    {
        private ILogger<AccountService> _logger { get; set; }
        private AccountDataContext _dataContext { get; set; }
        private AccountServiceSettings _settings { get; set;  }

        public AccountService(ILogger<AccountService> logger,AccountDataContext dataContext, AccountServiceSettings settings)
        {
            _logger = logger;
            _dataContext = dataContext;
            _settings = settings;

            _logger.LogInformation($"AccountService instance created on thread {Thread.CurrentThread.ManagedThreadId}");
        }

        public CheckTokenResponse CheckToken(CheckTokenRequest request)
        {
            try
            {
                #region basic checks
                if (string.IsNullOrEmpty(request.SessionToken) 
                    || string.IsNullOrEmpty(request.RequestFrom) 
                    || request.UserId <= 0)
                {
                    return new CheckTokenResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails()
                        {
                            Code = (int)eErrorCodes.InvalidRequest,
                            Message = "Invalid Request"
                        }
                    };
                }

                var user = _dataContext.Users.SingleOrDefault(u => u.Id == request.UserId);
                if (user == null)
                {
                    _logger.LogInformation($"CheckToken attempt by undefined id {request.UserId}.");

                    return new CheckTokenResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails()
                        {
                            Code = (int)eErrorCodes.InvalidRequest,
                            Message = "Invalid Request"
                        }
                    };
                }

                var session = user.Session;

                if (session == null
                    || session.SessionToken != request.SessionToken
                    || session.ConnectedFrom != request.RequestFrom
                    || DateTime.Now - session.LastCheck > _settings.SessionExpirationTime)
                {
                    if (session != null)
                    {
                        _dataContext.Sessions.Remove(session);
                    }

                    return new CheckTokenResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails() {
                            Code = (int)eErrorCodes.InvalidRequest,
                            Message = "Invalid Request" 
                        }
                    };
                }
                #endregion

                session.LastCheck = DateTime.Now;
                _dataContext.Sessions.Update(session);

                return new CheckTokenResponse()
                {
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Error during check token.");

                return new CheckTokenResponse()
                {
                    IsSuccess = false,
                    Error = new ErrorDetails()
                    {
                        Code = (int)eErrorCodes.ServerError,
                        Message = "Server Error"
                    }
                };
            }
            finally
            {
                if (_dataContext.ChangeTracker.HasChanges())
                {
                    _dataContext.SaveChanges();
                }
            }
        }

        public LoginResponse Login(LoginRequest request)
        {
            try
            {
                #region basic checks
                if (string.IsNullOrEmpty(request.Login) 
                    || string.IsNullOrEmpty(request.Password) 
                    || string.IsNullOrEmpty(request.RequestFrom))
                {
                    return new LoginResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails()
                        {
                            Code = (int)eErrorCodes.InvalidRequest,
                            Message = "Invalid Request"
                        }
                    };
                }

                var user = _dataContext.Users.SingleOrDefault(u => u.Login == request.Login);

                if (user == null)
                {
                    _logger.LogInformation($"Login attempt by undefined login {request.Login}.");

                    return new LoginResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails()
                        {
                            Code = (int)eErrorCodes.InvalidRequest,
                            Message = "Invalid Request"
                        }
                    };
                } 
                #endregion

                #region status processing
                var status = user.Status;

                if (status == null)
                {
                    status = new UserStatusData()
                    {
                        FailLoginCount = 0,
                        IsLocked = false,
                        UserId = user.Id,
                        LastLoginOn = DateTime.MinValue,
                        LastLoginFrom = request.RequestFrom
                    };

                    _dataContext.UserStatuses.Add(status);
                    _dataContext.SaveChanges();
                }

                if (user.Password != request.Password)
                {
                    status.FailLoginCount++;
                    status.LastLoginFrom = request.RequestFrom;
                    status.IsLocked = status.FailLoginCount >= _settings.MaximuFailedLoginCount;
                    _dataContext.UserStatuses.Update(status);

                    return new LoginResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails()
                        {
                            Code = (int)eErrorCodes.InvalidRequest,
                            Message = "Invalid Request"
                        }
                    };
                }

                if (status.IsLocked)
                {
                    status.LastLoginOn = DateTime.Now;
                    status.LastLoginFrom = request.RequestFrom;
                    _dataContext.UserStatuses.Update(status);

                    return new LoginResponse()
                    {
                        IsSuccess = false,
                        Error = new ErrorDetails()
                        {
                            Code = (int)eAccountErrorCodes.AccountLocked,
                            Message = "Account Locked"
                        }
                    };
                }

                status.FailLoginCount = 0;
                status.LastLoginOn = DateTime.Now;
                status.LastLoginFrom = request.RequestFrom;
                _dataContext.UserStatuses.Update(status);

                #endregion

                #region session processing
                var session = user.Session;

                //different place or exppired
                if (session != null && (session.LastCheck - DateTime.Now > _settings.SessionExpirationTime
                    || session.ConnectedFrom != request.RequestFrom))
                {
                    _dataContext.Sessions.Remove(session);
                    session = null;
                }

                if (session == null)
                {
                    session = new SessionData()
                    {
                        SessionToken = Guid.NewGuid().ToString(),
                        Created = DateTime.Now,
                        ConnectedFrom = request.RequestFrom,
                        UserId = user.Id
                    };
                    _dataContext.Sessions.Add(session);
                    _dataContext.SaveChanges();
                }

                session.LastCheck = DateTime.Now;

                _dataContext.Sessions.Update(session);
                #endregion

                return new LoginResponse()
                {
                    IsSuccess = true,
                    Token = session.SessionToken,
                    UserId = user.Id
                };

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Error during login.");
                return new LoginResponse()
                {
                    IsSuccess = false,
                    Error = new ErrorDetails()
                    {
                        Code = (int)eErrorCodes.ServerError,
                        Message = "Server Error"
                    }
                };
            }
            finally
            {
                if (_dataContext.ChangeTracker.HasChanges())
                {
                    _dataContext.SaveChanges();
                }
            }
        }
    }
}