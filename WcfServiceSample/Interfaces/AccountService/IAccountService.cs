using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceSample.Interfaces.AccountService
{
    [ServiceContract(Namespace = "http://wr.com/IAccountService")]
    public interface IAccountService
    {
    }
}
