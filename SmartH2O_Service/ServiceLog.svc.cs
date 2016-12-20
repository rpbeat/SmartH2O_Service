using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace SmartH2O_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceLog" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceLog.svc or ServiceLog.svc.cs at the Solution Explorer and start debugging.
    public class ServiceLog : IServiceLog
    {
        static string xmlPathLog = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data\\param-data.xml");
        static string xsdPathLog = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data\\param-data.xsd");
        static string xmlPathAlarm = Path.Combine(HostingEnvironment.ApplicationPhysicalPath , "App_Data\\alarms-data.xml");
        static string xsdPathAlarm = Path.Combine(HostingEnvironment.ApplicationPhysicalPath , "App_Data\\alarms-data.xsd");
        static bool xmlValid = true;
        static string strXmlErrorReason;

        public string DoWork()
        {
                return "It Works Test";
        }

        public string SendAlarm(string docc)
        {
            XmlDocument doc = new XmlDocument();
            if (docc != null)
            {
                XElement t = XElement.Parse(docc);
                if (!File.Exists(xmlPathAlarm))
                {
                    XmlNode rootNode = doc.CreateElement("Alarms");
                    doc.AppendChild(rootNode);
                    writeOnLogFile(doc, rootNode, xmlPathAlarm, t, true);

                    return "Send to Web service Ok:" + t.Element("Name").Value + "  " +
                            t.Element("Value").Value + "  " +
                            t.Element("ID").Value + "  " +
                            t.Element("Date").Value + "  " +
                            t.Element("Time").Value + "  " +
                            t.Element("Alarm").Value + "\n\n";
                }
                else
                {
                    doc.Load(xmlPathAlarm);
                    XmlNode root = doc.DocumentElement;
                    XmlNode myNode = root.SelectSingleNode("/Alarms");
                    writeOnLogFile(doc, myNode, xmlPathAlarm, t, true);

                    return "Send to Web service Ok:" + t.Element("Name").Value + "  " +
                            t.Element("Value").Value + "  " +
                            t.Element("ID").Value + "  " +
                            t.Element("Date").Value + "  " +
                            t.Element("Time").Value + "  " +
                             t.Element("Alarm").Value + "\n\n";
                }
            }
            else
            {
                return "Some error verify your send data";
            }
        }

        public string GetAllAlmars()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPathAlarm);
            return doc.InnerXml;
        }

        public string GetAlarmsByDate(string date)
        {
            XmlDocument doc = new XmlDocument();
            XmlDocument docToSend = new XmlDocument();
            doc.Load(xmlPathAlarm);
            if (doc != null)
            {
               
                XmlNode rootNode = docToSend.CreateElement("Alarms");
                docToSend.AppendChild(rootNode);
               
                XmlNodeList xnList = doc.SelectNodes("/Alarms/Sensor[Date='" + date + "']");

                foreach (XmlNode node in xnList)
                {
                    XmlElement sensor = docToSend.CreateElement("Sensor");
                    rootNode.AppendChild(sensor);

                    XmlNode nameNode = docToSend.CreateElement("Name");
                    nameNode.InnerText = node["Name"].InnerText;
                    sensor.AppendChild(nameNode);

                    XmlNode valueNode = docToSend.CreateElement("Value");
                    valueNode.InnerText = node["Value"].InnerText;
                    sensor.AppendChild(valueNode);

                    XmlNode idNode = docToSend.CreateElement("ID");
                    idNode.InnerText = node["ID"].InnerText;
                    sensor.AppendChild(idNode);

                    XmlNode dateNode = docToSend.CreateElement("Date");
                    dateNode.InnerText = node["Date"].InnerText;
                    sensor.AppendChild(dateNode);

                    XmlNode timeNode = docToSend.CreateElement("Time");
                    timeNode.InnerText = node["Time"].InnerText;
                    sensor.AppendChild(timeNode);

                    XmlNode timeStampNode = docToSend.CreateElement("TimeStamp");
                    timeStampNode.InnerText = node["Time"].InnerText;
                    sensor.AppendChild(timeStampNode);

                    XmlNode alarmNode = docToSend.CreateElement("Alarm");
                    alarmNode.InnerText = node["Alarm"].InnerText;
                    sensor.AppendChild(alarmNode);
                }
            }
            return docToSend.InnerXml;
        }

        public string SendValues(string docc)
        {
            XmlDocument doc = new XmlDocument();
            if (docc != null)
            {
                //this.doc.InnerText = docc;
                XElement t = XElement.Parse(docc);
                if(!File.Exists(xmlPathLog))
                {
                    XmlNode rootNode = doc.CreateElement("Sensors");
                    doc.AppendChild(rootNode);
                    writeOnLogFile(doc, rootNode, xmlPathLog, t, false);

                    return "Send to Web service Ok:" + t.Element("Name").Value+"  "+
                            t.Element("Value").Value + "  " + 
                            t.Element("ID").Value + "  " +
                            t.Element("Date").Value + "  " +
                            t.Element("TimeStamp").Value + "  " +
                            t.Element("Time").Value + "\n\n";
                }
                else
                {
                    doc.Load(xmlPathLog);
                    XmlNode root = doc.DocumentElement;
                    XmlNode myNode = root.SelectSingleNode("/Sensors");
                    writeOnLogFile(doc, myNode, xmlPathLog, t, false);

                    return "Ok:" + t.Element("Name").Value + "  " +
                            t.Element("Value").Value + "  " +
                            t.Element("ID").Value + "  " +
                            t.Element("Date").Value + "  " +
                            t.Element("TimeStamp").Value + "  " +
                            t.Element("Time").Value + "\n\n";
                }
            }else
            {
                return "Error";
            }
        }

        public string GetAllValues()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPathLog);
            return  doc.InnerXml;
        }

        public string GetValuesBySensorName(string name)
        {
            XmlDocument doc = new XmlDocument();
            XmlDocument docToSend = new XmlDocument();

            XmlNode rootNode = docToSend.CreateElement("Sensors");
            docToSend.AppendChild(rootNode);

            doc.Load(xmlPathLog);
            if (doc != null)
            {
                XmlNodeList xnList =  doc.SelectNodes("/Sensors/Sensor[Name='"+name+"']");
                foreach(XmlNode node in xnList)
                {
                    XmlElement sensor = docToSend.CreateElement("Sensor");
                    rootNode.AppendChild(sensor);

                    XmlNode nameNode = docToSend.CreateElement("Name");
                    nameNode.InnerText = node["Name"].InnerText;
                    sensor.AppendChild(nameNode);

                    XmlNode valueNode = docToSend.CreateElement("Value");
                    valueNode.InnerText = node["Value"].InnerText;
                    sensor.AppendChild(valueNode);

                    XmlNode idNode = docToSend.CreateElement("ID");
                    idNode.InnerText = node["ID"].InnerText;
                    sensor.AppendChild(idNode);

                    XmlNode dateNode = docToSend.CreateElement("Date");
                    dateNode.InnerText = node["Date"].InnerText;
                    sensor.AppendChild(dateNode);

                    XmlNode timeNode = docToSend.CreateElement("Time");
                    timeNode.InnerText = node["Time"].InnerText;
                    sensor.AppendChild(timeNode);

                    XmlNode timeStampNode = docToSend.CreateElement("TimeStamp");
                    timeStampNode.InnerText = node["Time"].InnerText;
                    sensor.AppendChild(timeStampNode);
                }
            }
            return docToSend.InnerXml;
        }

        public string GetValuesByDate(string date)
        {
            XmlDocument doc = new XmlDocument();
            XmlDocument docToSend = new XmlDocument();

            XmlNode rootNode = docToSend.CreateElement("Sensors");
            docToSend.AppendChild(rootNode);

            doc.Load(xmlPathLog);
            if (doc != null)
            {
                XmlNodeList xnList = doc.SelectNodes("/Sensors/Sensor[Date='" + date + "']");
                foreach (XmlNode node in xnList)
                {
                    XmlElement sensor = docToSend.CreateElement("Sensor");
                    rootNode.AppendChild(sensor);

                    XmlNode nameNode = docToSend.CreateElement("Name");
                    nameNode.InnerText = node["Name"].InnerText;
                    sensor.AppendChild(nameNode);

                    XmlNode valueNode = docToSend.CreateElement("Value");
                    valueNode.InnerText = node["Value"].InnerText;
                    sensor.AppendChild(valueNode);

                    XmlNode idNode = docToSend.CreateElement("ID");
                    idNode.InnerText = node["ID"].InnerText;
                    sensor.AppendChild(idNode);

                    XmlNode dateNode = docToSend.CreateElement("Date");
                    dateNode.InnerText = node["Date"].InnerText;
                    sensor.AppendChild(dateNode);

                    XmlNode timeNode = docToSend.CreateElement("Time");
                    timeNode.InnerText = node["Time"].InnerText;
                    sensor.AppendChild(timeNode);

                    XmlNode timeStampNode = docToSend.CreateElement("TimeStamp");
                    timeStampNode.InnerText = node["Time"].InnerText;
                    sensor.AppendChild(timeStampNode);
                }
            }

            return docToSend.InnerXml;
        }

        public string GetValuesBetweenDate(string date1, string date2)
        {
            XmlDocument doc = new XmlDocument();
            XmlDocument docToSend = new XmlDocument();

            XmlNode rootNode = docToSend.CreateElement("Sensors");
            docToSend.AppendChild(rootNode);

            doc.Load(xmlPathLog);
            if (doc != null)
            {
                XmlNodeList xnList = doc.SelectNodes("/Sensors/Sensor[Date>'"+ date1 + "' and Date<'"+ date2 + "']");
                foreach (XmlNode node in xnList)
                {
                    XmlElement sensor = docToSend.CreateElement("Sensor");
                    rootNode.AppendChild(sensor);

                    XmlNode nameNode = docToSend.CreateElement("Name");
                    nameNode.InnerText = node["Name"].InnerText;
                    sensor.AppendChild(nameNode);

                    XmlNode valueNode = docToSend.CreateElement("Value");
                    valueNode.InnerText = node["Value"].InnerText;
                    sensor.AppendChild(valueNode);

                    XmlNode idNode = docToSend.CreateElement("ID");
                    idNode.InnerText = node["ID"].InnerText;
                    sensor.AppendChild(idNode);

                    XmlNode dateNode = docToSend.CreateElement("Date");
                    dateNode.InnerText = node["Date"].InnerText;
                    sensor.AppendChild(dateNode);

                    XmlNode timeNode = docToSend.CreateElement("Time");
                    timeNode.InnerText = node["Time"].InnerText;
                    sensor.AppendChild(timeNode);

                    XmlNode timeStampNode = docToSend.CreateElement("TimeStamp");
                    timeStampNode.InnerText = node["Time"].InnerText;
                    sensor.AppendChild(timeStampNode);
                }
            }
            return docToSend.InnerXml;
        }

        public string GetValuesByDateAndHour(string date, string hour)
        {
            XmlDocument doc = new XmlDocument();

            XmlDocument docToSend = new XmlDocument();

            XmlNode rootNode = docToSend.CreateElement("Sensors");
            docToSend.AppendChild(rootNode);

            doc.Load(xmlPathLog);
            if (doc != null)
            {
                XmlNodeList xnList = doc.SelectNodes("/Sensors/Sensor[Date='"+date+"' and starts-with(Time,'" + hour + "')]");
                foreach (XmlNode node in xnList)
                {
                    XmlElement sensor = docToSend.CreateElement("Sensor");
                    rootNode.AppendChild(sensor);

                    XmlNode nameNode = docToSend.CreateElement("Name");
                    nameNode.InnerText = node["Name"].InnerText;
                    sensor.AppendChild(nameNode);

                    XmlNode valueNode = docToSend.CreateElement("Value");
                    valueNode.InnerText = node["Value"].InnerText;
                    sensor.AppendChild(valueNode);

                    XmlNode idNode = docToSend.CreateElement("ID");
                    idNode.InnerText = node["ID"].InnerText;
                    sensor.AppendChild(idNode);

                    XmlNode dateNode = docToSend.CreateElement("Date");
                    dateNode.InnerText = node["Date"].InnerText;
                    sensor.AppendChild(dateNode);

                    XmlNode timeNode = docToSend.CreateElement("Time");
                    timeNode.InnerText = node["Time"].InnerText;
                    sensor.AppendChild(timeNode);

                    XmlNode timeStampNode = docToSend.CreateElement("TimeStamp");
                    timeStampNode.InnerText = node["Time"].InnerText;
                    sensor.AppendChild(timeStampNode);
                }
            }
            return docToSend.InnerXml;
        }


        private static void writeOnLogFile(XmlDocument xmlDoc, XmlNode rootNode, string xmlPath, XElement t, bool isAlarm)
        {
            XmlElement sensor = xmlDoc.CreateElement("Sensor");
            rootNode.AppendChild(sensor);

            XmlNode nameNode = xmlDoc.CreateElement("Name");
            nameNode.InnerText = t.Element("Name").Value;
            sensor.AppendChild(nameNode);

            XmlNode valueNode = xmlDoc.CreateElement("Value");
            valueNode.InnerText = t.Element("Value").Value;
            sensor.AppendChild(valueNode);

            XmlNode idNode = xmlDoc.CreateElement("ID");
            idNode.InnerText = t.Element("ID").Value;
            sensor.AppendChild(idNode);

            XmlNode dateNode = xmlDoc.CreateElement("Date");
            dateNode.InnerText = t.Element("Date").Value;
            sensor.AppendChild(dateNode);

            XmlNode timeNode = xmlDoc.CreateElement("Time");
            timeNode.InnerText = t.Element("Time").Value;
            sensor.AppendChild(timeNode);

            XmlNode timeStampNode = xmlDoc.CreateElement("TimeStamp");
            timeStampNode.InnerText = t.Element("TimeStamp").Value;
            sensor.AppendChild(timeStampNode);

            if (isAlarm)
            {
                XmlNode alarmNode = xmlDoc.CreateElement("Alarm");
                alarmNode.InnerText = t.Element("Alarm").Value;
                sensor.AppendChild(alarmNode);

                xmlDoc.Schemas.Add(null, xsdPathAlarm);
                ValidationEventHandler eventHandler = new ValidationEventHandler(validateXML);
                xmlDoc.Validate(eventHandler);
                if (xmlValid)
                {
                    Console.Write("XML Status: OK!\n\n");
                    xmlDoc.Save(xmlPath);
                }
                else
                {
                    throw new XmlException(strXmlErrorReason.ToString());
                }
            }
            else
            {
                xmlDoc.Schemas.Add(null, xsdPathLog);
                ValidationEventHandler eventHandler = new ValidationEventHandler(validateXML);
                xmlDoc.Validate(eventHandler);
                if (xmlValid)
                {
                    Console.Write("XML Status: OK!\n\n");
                    xmlDoc.Save(xmlPath);

                }
                else
                {
                    throw new XmlException(strXmlErrorReason.ToString());
                }
            }
        }

        private static void validateXML(Object sender, ValidationEventArgs args)
        {
            xmlValid = false;
            strXmlErrorReason = args.Message;
        }
    }
}
