using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfServiceSample.BaseContracts
{
    [DataContract]
    public class ErrorDetails
    {
        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}