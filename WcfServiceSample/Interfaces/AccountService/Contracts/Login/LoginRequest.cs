using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfServiceSample.Interfaces.AccountService.Contracts.Login
{
    [DataContract]
    public class LoginRequest
    {
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public string ApplicationToken { get; set; }
    }
}