﻿using System.Runtime.Serialization;

namespace WA.Contracts.Core
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