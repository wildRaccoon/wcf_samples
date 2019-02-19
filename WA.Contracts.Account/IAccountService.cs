using System.ServiceModel;
using WA.Contracts.Account.Messages.CheckToken;
using WA.Contracts.Account.Messages.Login;

namespace WA.Contracts.Account
{
    [ServiceContract(Namespace = "http://wr.com/IAccountService")]
    public interface IAccountService
    {
        [OperationContract]
        LoginResponse Login(LoginRequest request);

        [OperationContract]
        CheckTokenResponse CheckToken(CheckTokenRequest request);
    }
}
