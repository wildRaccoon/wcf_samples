using System;

namespace WA.Repository.Account.Data
{
    public class SessionData
    {
        public int UserId { get; set; }

        public string SessionToken { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastCheck { get; set; }

        public string ConnectedFrom { get; set; }

        public UserData User { get; set; }
    }
}
