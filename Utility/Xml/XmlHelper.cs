using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Utility.Xml
{
    public class XmlHelper : Base.ClassBase
    {
        public void loadXml(ref XmlDocument xmlDestination, string xmlFullName)
        {
            this.loadXmlFile(ref xmlDestination, xmlFullName);
        }
        private void loadXmlFile(ref XmlDocument xmlDestination, string xmlFullName)
        {
            try
            {
                string xml = System.IO.File.ReadAllText(xmlFullName);
                xmlDestination.LoadXml(xml);
            }
            catch (Exception)
            {

                Utility.Logger.myLogger.Debug("Création du reader de document XML.");
                XmlTextReader myTextReader = new XmlTextReader(xmlFullName);
                myTextReader.XmlResolver = null;
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ProhibitDtd = false;
                settings.ValidationType = ValidationType.None;
                using(XmlReader xr = XmlReader.Create(myTextReader, settings))
                {
                    xmlDestination = new XmlDocument();
            
                    Utility.Logger.myLogger.Debug("Lecture du document XML :" + xmlFullName);
                    try
                    {
                        if (xr == null) Utility.Logger.myLogger.Debug("Attention le XmlTextReader est null.");
                        if (xmlDestination == null) Utility.Logger.myLogger.Debug("Attention le xmlDestination est null.");
                        Utility.Logger.myLogger.Debug("Chargement du fichier xml.");
                        xmlDestination.Load(xr);
                    }
                    catch (Exception exXmlReader)
                    {
                        this.setError("XmlHelper.loadXML");
                        this.setError(exXmlReader);
                    }
                }
            }
        }
    }
}
