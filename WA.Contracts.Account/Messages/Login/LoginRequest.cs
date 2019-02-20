using System.Runtime.Serialization;

namespace WA.Contracts.Account.Messages.Login
{
    [DataContract]
    public class LoginRequest
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string RequestFrom { get; set; }
    }
}