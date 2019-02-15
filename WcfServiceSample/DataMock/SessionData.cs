using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServiceSample.DataMock
{
    public class SessionData
    {
        public string SessionToken { get; set; }

        public string UserId { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime LastCheck { get; set; }
    }
}