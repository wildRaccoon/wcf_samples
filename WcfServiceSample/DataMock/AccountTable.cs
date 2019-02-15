using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServiceSample.DataMock
{
    public class AccountTable
    {
        public static List<AccountData> Instance = new List<AccountData>()
        {
            new AccountData(){
                AuthToken = "token1",
                User = "admin",
                UserRole = eUserRole.Admin
            },

            new AccountData(){
                AuthToken = "token2",
                User = "manager",
                UserRole = eUserRole.Manager
            },

            new AccountData(){
                AuthToken = "token3",
                User = "guest1",
                UserRole = eUserRole.Guest
            },

            new AccountData(){
                AuthToken = "token4",
                User = "guest2",
                UserRole = eUserRole.Guest
            },
        };
    }
}