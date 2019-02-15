using System.Runtime.Serialization;
using WcfServiceSample.BaseContracts;

namespace WcfServiceSample.Interfaces.AccountService.Contracts.Login
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