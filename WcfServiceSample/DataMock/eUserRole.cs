using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServiceSample.DataMock
{
    public enum eUserRole
    {
        //remove orders
        Admin,

        //get list of orders
        Guest,

        //create and change status
        Manager
    }
}