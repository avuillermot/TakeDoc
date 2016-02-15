using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ULibre.Drivers.Implementation
{
    public class OdsDriver : LibreOfficeDriver, Drivers.Interface.IDriver
    {
        public void FillField(string fieldName, string value) 
        {
            XmlNode nodeDefinition = this.GetField(fieldName);
            if (nodeDefinition != null) nodeDefinition.InnerText = value;
        }

        public void AddInsertionMenuItem(string menuContainer, string newItemName, string value)
        {
            this.AddModule("ModuleFiducialOds.xml","Ods");
            this.MenuBar.AddMenuItem(menuContainer, newItemName, "Fiducial.ModuleFiducialOds.InsertFiducialText", value);
            MemoryStream ms = new MemoryStream();
            this.MenuBar.XmlMenuBar.Save(ms);
            zipManager.addZipEntry(this._officeDocument.FullName, this._menuBarPathFile, ms);
        }

        /// <summary>
        /// Retourne le champ de fusion de texte definit dans le document
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private XmlNode GetField(string fieldName)
        {
            XmlNamespaceManager ns = this.GetNamespaceManager(_xmlContent);
            XmlNode nodeDefinition = _xmlContent.SelectSingleNode(string.Format("//text:p[text()='[{0}]']", fieldName), ns);
            return nodeDefinition;
        }
        public void FillField(Object obj) { this.ExceptionImplementation(); }
        public void FillField(IDictionary<string, string> dictionnary) { this.ExceptionImplementation(); }
        public void FillImage(string fieldName, string value) { this.ExceptionImplementation(); }
        public void AddLine(string tableName, string[] values) { this.ExceptionImplementation(); }
        public string GetFieldValue(string fieldName) { this.ExceptionImplementation(); return string.Empty; }
        public void RemoveEmptyLine(string tableName) { this.ExceptionImplementation(); }
        public void AddLineImage(string tableName, string title, string code, string base64) { this.ExceptionImplementation(); }

 
    }
}
