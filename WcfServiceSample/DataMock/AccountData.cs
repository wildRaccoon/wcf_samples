using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServiceSample.DataMock
{
    public class AccountData
    {
        public int Id { get; set; }

        public string User { get; set; }

        public string AuthToken { get; set; }

        public eUserRole UserRole { get; set; }
    }
}