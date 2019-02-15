using System.Runtime.Serialization;

namespace WcfServiceSample.Interfaces.AccountService.Contracts.Login
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