using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfServiceSample.BaseContracts
{
    [DataContract]
    public class BaseRequest
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Token { get; set; }
    }
}