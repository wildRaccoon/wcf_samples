using System.Runtime.Serialization;

namespace WA.Contracts.Account.Messages.Login
{
    [DataContract]
    public class LoginRequest
    {
        [DataMember]
        public string User { get; set; }

        [DataMember]
        public string ApplicationToken { get; set; }
    }
}