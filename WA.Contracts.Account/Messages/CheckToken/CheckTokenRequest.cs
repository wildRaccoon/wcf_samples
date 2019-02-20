using System.Runtime.Serialization;

namespace WA.Contracts.Account.Messages.CheckToken
{
    [DataContract]
    public class CheckTokenRequest
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string SessionToken { get; set; }

        [DataMember]
        public string RequestFrom { get; set; }
    }
}