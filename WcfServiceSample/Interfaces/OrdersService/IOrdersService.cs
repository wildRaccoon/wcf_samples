using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceSample.Interfaces.OrdersService
{
    [ServiceContract(Namespace = "http://wr.com/IOrdersService")]
    public interface IOrdersService
    {
    }
}
