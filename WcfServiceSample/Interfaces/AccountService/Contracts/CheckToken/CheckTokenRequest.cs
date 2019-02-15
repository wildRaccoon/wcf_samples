using System.Runtime.Serialization;

namespace WcfServiceSample.Interfaces.AccountService.Contracts.CheckToken
{
    [DataContract]
    public class CheckTokenRequest
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string SessionToken { get; set; }
    }
}