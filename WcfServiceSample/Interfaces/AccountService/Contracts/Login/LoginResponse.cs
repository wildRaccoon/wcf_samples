using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WcfServiceSample.BaseContracts;

namespace WcfServiceSample.Interfaces.AccountService.Contracts.Login
{
    [DataContract]
    public class LoginResponse : BaseResponse
    {
        [DataMember]
        public string Token { get; set; }
    }
}