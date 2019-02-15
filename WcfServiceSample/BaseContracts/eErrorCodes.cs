using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServiceSample.BaseContracts
{
    public class eErrorCodes
    {
        public const int InvalidRequest = 1000;
        public const string InvalidRequesMessage = "Invalid Request";

        public const int ServerError = 1001;
        public const string ServerErrorMessage = "Server error";

        public const int NotAllowedRequest = 1002;
        public const string NotAllowedRequesMessage = "Invalid Request";

        //20xx - login errors
        public const int LoginFailed = 2001;
        public const string LoginFailedMessage = "Invalid user or auth token. Please try again";

        //30xx orders service
        public const int OrderNotFound = 3001;
        public const string OrderNotFoundMessage = "Order not found";
    }
}