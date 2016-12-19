using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SmartH2O_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceLog" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceLog.svc or ServiceLog.svc.cs at the Solution Explorer and start debugging.
    public class ServiceLog : IServiceLog
    {
        public string DoWork()
        {

            return "You are in!";
        }
    }
}
