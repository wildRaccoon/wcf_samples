using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfServiceSample.BaseContracts
{

    [DataContract]
    public class BaseResponse
    {
        [DataMember]
        public bool IsValid { get; set; }

        [DataMember]
        public ErrorDetails Error { get; set; }
    }
}