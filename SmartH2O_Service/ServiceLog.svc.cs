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

namespace SmartH2O_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceLog" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceLog.svc or ServiceLog.svc.cs at the Solution Explorer and start debugging.
    public class ServiceLog : IServiceLog
    {
        static string XmlPath= HostingEnvironment.ApplicationPhysicalPath + "App_Data\\XMLData.xml";
        XmlDocument doc = new XmlDocument();
        public string DoWork()
        {
            return "You are in!";
        }

        public string SendValues(string docc)
        {
            if(docc != null)
            {
                //this.doc.InnerText = docc;
                XElement t = XElement.Parse(docc);
                if(!File.Exists(XmlPath))
                {
                    XmlNode rootNode = doc.CreateElement("Sensors");
                    doc.AppendChild(rootNode);
                    writeOnLogFile(doc, rootNode, XmlPath, t);

                    return "Ok:" + t.Element("Name").Value +
                            t.Element("Value").Value +
                            t.Element("ID").Value +
                            t.Element("Date").Value +
                            t.Element("Time").Value + "\n\n";
                }
                else
                {
                    doc.Load(XmlPath);
                    XmlNode root = doc.DocumentElement;
                    XmlNode myNode = root.SelectSingleNode("/Sensors");
                    writeOnLogFile(doc, myNode, XmlPath, t);

                    return "Ok:" + t.Element("Name").Value +
                            t.Element("Value").Value +
                            t.Element("ID").Value +
                            t.Element("Date").Value +
                            t.Element("Time").Value + "\n\n";
                }
            }else
            {
                return "Error";
            }
        }

        public string GetAllValues()
        {
            return "I found the file!" + doc.InnerXml;
        }

        private static void writeOnLogFile(XmlDocument xmlDoc, XmlNode rootNode, string xmlPath, XElement t)
        {
            XmlElement sensor = xmlDoc.CreateElement("Sensor");
            rootNode.AppendChild(sensor);

            XmlNode nameNode = xmlDoc.CreateElement("Name");
            nameNode.InnerText = t.Element("Name").Value;
            sensor.AppendChild(nameNode);

            XmlNode valueNode = xmlDoc.CreateElement("Value");
            valueNode.InnerText = t.Element("Name").Value;
            sensor.AppendChild(valueNode);

            XmlNode idNode = xmlDoc.CreateElement("ID");
            valueNode.InnerText = t.Element("ID").Value;
            sensor.AppendChild(valueNode);

            XmlNode dateNode = xmlDoc.CreateElement("Date");
            dateNode.InnerText = t.Element("Date").Value;
            sensor.AppendChild(dateNode);

            XmlNode timeNode = xmlDoc.CreateElement("Time");
            timeNode.InnerText = t.Element("Time").Value;
            sensor.AppendChild(timeNode);

            //VALIDATION
           /* xmlDoc.Schemas.Add(null, xsdPath);
            ValidationEventHandler eventHandler = new ValidationEventHandler(validateXML);
            xmlDoc.Validate(eventHandler);
            if (xmlValid)
            {
                Console.Write("XML Status: OK!\n\n");
            }
            else
            {
                throw new XmlException(strXmlErrorReason.ToString());
            }*/

            // ServiceLog.ServiceLogClient service = new ServiceLog.ServiceLogClient();

            //Console.Write(service.SendValues(xmlDoc.InnerText));
            xmlDoc.Save(xmlPath);
        }
    }
}
