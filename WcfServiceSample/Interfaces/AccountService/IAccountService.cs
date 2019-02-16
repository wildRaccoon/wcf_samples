using System.ServiceModel;
using WcfServiceSample.Interfaces.AccountService.Contracts.CheckToken;
using WcfServiceSample.Interfaces.AccountService.Contracts.Login;

namespace WcfServiceSample.Interfaces.AccountService
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
