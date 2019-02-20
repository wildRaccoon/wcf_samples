using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WA.Services.Account
{
    public class AccountServiceSettings
    {
        public int MaximuFailedLoginCount { get; set; } = 3;

        public TimeSpan SessionExpirationTime { get; set; } = TimeSpan.FromMinutes(5);
    }
}
