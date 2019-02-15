using System.ServiceModel;
using System.ServiceModel.Web;
using WcfServiceSample.Interfaces.AccountService.Contracts.CheckToken;
using WcfServiceSample.Interfaces.AccountService.Contracts.Login;

namespace WcfServiceSample.Interfaces.AccountService
{
    [ServiceContract(Namespace = "http://wr.com/IAccountService")]
    public interface IAccountService
    {
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        LoginResponse Login(LoginRequest request);

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        CheckTokenResponse CheckToken(CheckTokenRequest request);
    }
}
