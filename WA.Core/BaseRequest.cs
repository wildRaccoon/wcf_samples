using System.Runtime.Serialization;

namespace WA.Core
{
    [DataContract]
    public class BaseRequest
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Token { get; set; }
    }
}