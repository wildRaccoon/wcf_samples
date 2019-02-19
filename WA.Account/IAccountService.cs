using System.ServiceModel;
using WA.Account.Contracts.CheckToken;
using WA.Account.Contracts.Login;

namespace WA.Account.AccountService
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
