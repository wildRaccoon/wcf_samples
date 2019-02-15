using System;
using System.Collections.Concurrent;

namespace WcfServiceSample.DataMock
{
    public class SessionStorage
    {
        public static TimeSpan SessionExpired = TimeSpan.FromMinutes(1);

        public static ConcurrentDictionary<int, SessionData> Instance = new ConcurrentDictionary<int, SessionData>();
    }
}