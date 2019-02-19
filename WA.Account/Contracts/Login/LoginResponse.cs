using System.Runtime.Serialization;
using WA.Core;

namespace WA.Account.Contracts.Login
{
    [DataContract]
    public class LoginResponse : BaseResponse
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public int UserId { get; set; }
    }
}