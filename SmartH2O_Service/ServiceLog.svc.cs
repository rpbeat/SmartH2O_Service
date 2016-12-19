using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace SmartH2O_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceLog" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceLog.svc or ServiceLog.svc.cs at the Solution Explorer and start debugging.
    public class ServiceLog : IServiceLog
    {
        XmlDocument doc = new XmlDocument();
        public string DoWork()
        {
            return "You are in!";
        }

        public string SendValues(string docc)
        {
            if(docc != null)
            {
                this.doc.InnerText = docc;
                return "Ok";
            }else
            {
                return "Error";
            }
        }

        public string GetAllValues()
        {
            return "I found the file!" + doc.InnerXml;
        }
    }
}
