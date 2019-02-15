using System.Collections.Generic;

namespace WcfServiceSample.DataMock
{
    public class AccountTable
    {
        public static List<AccountData> Instance = new List<AccountData>()
        {
            new AccountData(){
                AuthToken = "token1",
                User = "admin",
                UserRole = eUserRole.Admin,
                Id = 1
            },

            new AccountData(){
                AuthToken = "token2",
                User = "manager",
                UserRole = eUserRole.Manager,
                Id = 2
            },

            new AccountData(){
                AuthToken = "token3",
                User = "guest1",
                UserRole = eUserRole.Guest,
                Id = 3
            },

            new AccountData(){
                AuthToken = "token4",
                User = "guest2",
                UserRole = eUserRole.Guest,
                Id = 4
            },
        };
    }
}