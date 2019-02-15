using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServiceSample.DataMock
{
    public class SessionData
    {
        public string SessionToken { get; set; }

        public int UserId { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime LastCheck { get; set; }

        public SessionData()
        {
        }

        public SessionData(SessionData toClone)
        {
            this.LastCheck = toClone.LastCheck;
            this.LoginTime = toClone.LoginTime;
            this.SessionToken = toClone.SessionToken;
            this.UserId = toClone.UserId;
        }
    }
}