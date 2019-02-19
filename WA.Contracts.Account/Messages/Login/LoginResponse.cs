using System.Runtime.Serialization;
using WA.Contracts.Core;

namespace WA.Contracts.Account.Messages.Login
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