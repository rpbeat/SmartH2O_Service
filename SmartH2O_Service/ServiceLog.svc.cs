using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
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
        static string XmlPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath,"App_Data/log-sensors.xml");
        static string XsdPath = HostingEnvironment.ApplicationPhysicalPath + "App_Data\\log-sensors.xsd";
        static string XmlPathAlarm = HostingEnvironment.ApplicationPhysicalPath + "App_Data\\log-alarms.xml";
        static string XsdPathAlarm = HostingEnvironment.ApplicationPhysicalPath + "App_Data\\log-alarms.xsd";

        static bool xmlValid = true;
        static string strXmlErrorReason;

        
        public string DoWork()
        {
            if (File.Exists(@XmlPath))
            {
                return "You are in! " + @XmlPath;
            }
            return "fudeu";
        }

        public string SendAlarm(string docc)
        {
            XmlDocument doc = new XmlDocument();
            if (docc != null)
            {
                XElement t = XElement.Parse(docc);
                if (!File.Exists(XmlPathAlarm))
                {
                    XmlNode rootNode = doc.CreateElement("Alarms");
                    doc.AppendChild(rootNode);
                    writeOnLogFile(doc, rootNode, XmlPathAlarm, t, true);

                    return "Send to Web service Ok:" + t.Element("Name").Value + "  " +
                            t.Element("Value").Value + "  " +
                            t.Element("ID").Value + "  " +
                            t.Element("Date").Value + "  " +
                            t.Element("Time").Value + "  " +
                            t.Element("Alarm").Value + "\n\n";
                }
                else
                {
                    doc.Load(XmlPathAlarm);
                    XmlNode root = doc.DocumentElement;
                    XmlNode myNode = root.SelectSingleNode("/Alarms");
                    writeOnLogFile(doc, myNode, XmlPathAlarm, t, true);

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
                return "Error";
            }
        }

        public string GetAllAlmars()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(XmlPathAlarm);
            return doc.InnerXml;
        }

        public string GetAlarmsByDate(string date)
        {
            XmlDocument doc = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            doc.Load(XmlPathAlarm);
            if (doc != null)
            {
                XmlNodeList xnList = doc.SelectNodes("/Alarms/Sensor[Date='" + date + "']");
                foreach (XmlNode node in xnList)
                {
                    sb.Append(node.InnerXml);
                }
            }
            return sb.ToString();
        }

        public string SendValues(string docc)
        {
            XmlDocument doc = new XmlDocument();
            if (docc != null)
            {
                //this.doc.InnerText = docc;
                XElement t = XElement.Parse(docc);
                if(!File.Exists(XmlPath))
                {
                    XmlNode rootNode = doc.CreateElement("Sensors");
                    doc.AppendChild(rootNode);
                    writeOnLogFile(doc, rootNode, XmlPath, t, false);

                    return "Send to Web service Ok:" + t.Element("Name").Value+"  "+
                            t.Element("Value").Value + "  " +
                            t.Element("ID").Value + "  " +
                            t.Element("Date").Value + "  " +
                            t.Element("Time").Value + "\n\n";
                }
                else
                {
                    doc.Load(XmlPath);
                    XmlNode root = doc.DocumentElement;
                    XmlNode myNode = root.SelectSingleNode("/Sensors");
                    writeOnLogFile(doc, myNode, XmlPath, t, false);

                    return "Ok:" + t.Element("Name").Value + "  " +
                            t.Element("Value").Value + "  " +
                            t.Element("ID").Value + "  " +
                            t.Element("Date").Value + "  " +
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
            doc.Load(XmlPath);
            return  doc.InnerXml;
        }

        public string GetValuesBySensorName(string name)
        {
            XmlDocument doc = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            doc.Load(XmlPath);
            if (doc != null)
            {
                XmlNodeList xnList =  doc.SelectNodes("/Sensors/Sensor[Name='"+name+"']");
                foreach(XmlNode node in xnList)
                {
                    sb.Append(node.InnerXml);
                }
            }
            return sb.ToString();
        }

        public string GetValuesByDate(string date)
        {
            XmlDocument doc = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            doc.Load(XmlPath);
            if (doc != null)
            {
                XmlNodeList xnList = doc.SelectNodes("/Sensors/Sensor[Date='" + date + "']");
                foreach (XmlNode node in xnList)
                {
                    sb.Append(node.InnerXml);
                }
            }
            return sb.ToString();
        }

        public string GetValuesBetweenDate(string date1, string date2)
        {
            XmlDocument doc = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            doc.Load(XmlPath);
            if (doc != null)
            {
                XmlNodeList xnList = doc.SelectNodes("/Sensors/Sensor[Date>'"+ date1 + "' and Date<'"+ date2 + "']");
                foreach (XmlNode node in xnList)
                {
                    sb.Append(node.InnerXml);
                }
            }
            return sb.ToString();
        }

        public string GetValuesByDateAndHour(string date, string hour)
        {
            XmlDocument doc = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            doc.Load(XmlPath);
            if (doc != null)
            {
                XmlNodeList xnList = doc.SelectNodes("/Sensors/Sensor[Date='18/12/2016' and starts-with(Time,'" + hour + "')]");
                foreach (XmlNode node in xnList)
                {
                    sb.Append(node.InnerXml);
                }
            }
            return sb.ToString();
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

            if (isAlarm)
            {
                XmlNode alarmNode = xmlDoc.CreateElement("Alarm");
                alarmNode.InnerText = t.Element("Alarm").Value;
                sensor.AppendChild(alarmNode);

                xmlDoc.Schemas.Add(null, XsdPathAlarm);
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
                xmlDoc.Schemas.Add(null, XsdPath);
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
