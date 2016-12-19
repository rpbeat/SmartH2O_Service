using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace SmartH2O_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceLog" in both code and config file together.
    [ServiceContract]
    public interface IServiceLog
    {
        [OperationContract]
        string DoWork();

        [OperationContract]
        string SendValues(string data);

        [OperationContract]
        string GetAllValues();

        [OperationContract]
        string GetValuesBySensorName(string name);

        [OperationContract]
        string GetValuesByDate(string date);

        [OperationContract]
        string GetValuesByDateAndHour(string date, string hour);

        [OperationContract]
        string GetValuesBetweenDate(string date1, string date2);

        [OperationContract]
        string SendAlarm(string docc);

        [OperationContract]
        string GetAllAlmars();


    }

}
