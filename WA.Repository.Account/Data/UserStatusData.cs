using System;

namespace WA.Repository.Account.Data
{
    public class UserStatusData
    {
        public int UserId { get; set; }

        public bool IsLocked { get; set; }

        public int FailLoginCount { get; set; }

        public DateTime LastLoginOn { get; set; }

        public string LastLoginFrom { get; set; }

        public UserData User { get; set; }
    }
}
