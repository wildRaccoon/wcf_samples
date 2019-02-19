using System.Runtime.Serialization;

namespace WA.Contracts.Core
{

    [DataContract]
    public class BaseResponse
    {
        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public ErrorDetails Error { get; set; }
    }
}