using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;

namespace ULibre.Drivers.Implementation
{
    /// <summary>
    /// Permet la gestion des documents ODT
    /// </summary>
    public class OdtDriver : LibreOfficeDriver, Drivers.Interface.IDriver
    {
        private static IDictionary<string, string> uri = new Dictionary<string, string>();

        public OdtDriver():base() {
            
        }

        public void FillFormField(string fieldName, string value)
        {
            XmlNode nodeDefinition = this.GetFormField(fieldName);
            if (nodeDefinition != null)
                nodeDefinition.Attributes["form:current-value"].Value = value;
        }

        /// <summary>
        /// Rempli le champ de fusion indiqué
        /// </summary>
        /// <param name="fieldName">nom du champ de fusion</param>
        /// <param name="value">valeur</param>
        public void FillField(string fieldName, string value)
        {
            XmlNode nodeDefinition = this.GetField(fieldName);
            if (nodeDefinition != null)
                nodeDefinition.Attributes["office:string-value"].Value = value;
        }

        public void FillField(IDictionary<string, string> dictionnary)
        {
            foreach (KeyValuePair<string, string> k in dictionnary)
            {
                this.FillField(k.Key, k.Value);
            }
        }

        public void FillField(Object obj)
        {
            // parcours les champs
            foreach (FieldInfo p in obj.GetType().GetFields())
            {
                this.FillField(p.Name, p.GetValue(obj).ToString());
            }
            // parcours les propriétés
            foreach (PropertyInfo p in obj.GetType().GetProperties())
            {
                this.FillField(p.Name, p.GetValue(obj,null).ToString());
            }
        }
                        
        /// <summary>
        /// Ajout une ligne à un tableau avec les valeurs en parametres
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values"></param>
        public void AddLine(string tableName, string[] values)
        {
            XmlNamespaceManager ns = this.GetNamespaceManager(this._xmlContent);
            XmlNode nodeTable = this.GetTable(tableName);
            if (nodeTable != null)
            {
                XmlNode lastRow = null;
                foreach (XmlNode row in nodeTable.SelectNodes("table:table-row", ns))
                {
                    lastRow = row;
                }
                lastRow = lastRow.CloneNode(true);
                int index = 0;
                foreach (XmlNode cell in lastRow.SelectNodes("table:table-cell", ns))
                {
                    if (values.Length > index)
                    {
                        XmlNode txt = cell.SelectSingleNode("text:p", ns);
                        txt.RemoveAll();
                        ICollection<XmlNode> nodesInText = this.SplitLines(values[index]);
                        foreach (XmlNode nodeText in nodesInText)
                        {
                            txt.AppendChild(nodeText);
                            if (nodesInText.Count > 1)
                            {
                                XmlNode br = this._xmlContent.CreateElement("text", "line-break", "text");
                                txt.AppendChild(br);
                            }
                        }
                    }
                    index++;
                }
                nodeTable.AppendChild(lastRow);
            }
        }

        public void RemoveEmptyLine(string tableName)
        {
            XmlNamespaceManager ns = this.GetNamespaceManager(this._xmlContent);
            XmlNode nodeTable = this.GetTable(tableName);
            ICollection<XmlNode> deletes = new List<XmlNode>();

            foreach (XmlNode row in nodeTable.SelectNodes("table:table-row", ns))
            {
                bool empty = true;
                foreach (XmlNode cell in row.SelectNodes("table:table-cell", ns))
                {
                    XmlNode txt = cell.SelectSingleNode("text:p", ns);
                    if (txt.InnerText.Equals(string.Empty) == false)
                        empty = false;
                }
                if (empty) deletes.Add(row);
            }
            if (deletes.Count() > 0) {
                XmlNode[] temp = deletes.ToArray<XmlNode>();
                for (int i = 0; i < temp.Count(); i++)
                {
                    XmlNode node = temp[i];
                    node.ParentNode.RemoveChild(node);
                }
            }
        }

        public string GetFieldValue(string fieldName)
        {
            XmlNode nodeDefinition = this.GetField(fieldName);
            if (nodeDefinition != null) return nodeDefinition.Attributes["office:string-value"].Value;
            return string.Empty;
        }

        public void FillImage(string imageName, string fullFileName)
        {
            if (System.IO.File.Exists(fullFileName) == false)
                fullFileName = string.Concat(Utility.Configuration.ConfigurationHelper.GetAppSettings("ImagesDirectory") + fullFileName);

            FileInfo file = new FileInfo(fullFileName);
            this.FillImage(imageName, file);
        }

        public void FillImageBase64(string fieldName, string base64) {

            string imgName = string.Empty;

            XmlNamespaceManager ns = this.GetNamespaceManager(_xmlContent);
            XmlNode nodeFrame = _xmlContent.SelectSingleNode("//draw:frame[@draw:name='" + fieldName + "']", ns);
            if (nodeFrame != null)
            {
                XmlNode nodeImage = nodeFrame.SelectSingleNode("draw:image", ns);
                if (nodeImage != null) imgName = nodeImage.Attributes["xlink:href"].Value;
            }

            byte[] data = Convert.FromBase64String(base64.Replace("data:image/png;base64,", string.Empty));
            MemoryStream ms = new MemoryStream(data);
            zipManager.addZipEntry(this._officeDocument.FullName, imgName, ms);
        }

        public void AddInsertionMenuItem(string menuContainer, string newItemName, string value)
        {
            this.AddModule("ModuleFiducialOdt.xml","Odt");
            this.MenuBar.AddMenuItem(menuContainer, newItemName, "Fiducial.ModuleFiducialOdt.InsertFiducialText", value);
            MemoryStream ms = new MemoryStream();
            this.MenuBar.XmlMenuBar.Save(ms);
            zipManager.addZipEntry(this._officeDocument.FullName, this._menuBarPathFile, ms);
        }

        private void SetNewImage(XmlNode nodeFrame, XmlNamespaceManager ns, FileInfo image)
        {
            if (nodeFrame != null)
            {
                XmlNode nodeImage = nodeFrame.SelectSingleNode("draw:image", ns);
                if (nodeImage != null)
                {
                    string odtImageName = nodeImage.Attributes["xlink:href"].Value;

                    byte[] data = System.IO.File.ReadAllBytes(image.FullName);
                    MemoryStream ms = new MemoryStream(data);
                    // remplacement de l'ancienne image
                    zipManager.addZipEntry(this._officeDocument.FullName, odtImageName, ms);
                }
            }
        }

        private void FillImage(string imageName, FileInfo file)
        {
            // update document header
            XmlNamespaceManager ns = this.GetNamespaceManager(_xmlStyles);
            XmlNode nodeFrame = _xmlStyles.SelectSingleNode("//draw:frame[@draw:name='" + imageName + "']", ns);
            this.SetNewImage(nodeFrame, ns, file);
            
            // update document content
            ns = this.GetNamespaceManager(_xmlContent);
            nodeFrame = _xmlContent.SelectSingleNode("//draw:frame[@draw:name='" + imageName + "']", ns);
            this.SetNewImage(nodeFrame, ns, file);
        }

        /// <summary>
        /// Retourne le tableau definit dans le document
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private XmlNode GetTable(string tableName)
        {
            XmlNamespaceManager ns = this.GetNamespaceManager(_xmlContent);
            XmlNode nodeDefinition = _xmlContent.SelectSingleNode("//table:table[@table:name='" + tableName + "']", ns);
            return nodeDefinition;
        }

        private XmlNode GetFormField(string fieldName)
        {
            XmlNamespaceManager ns = this.GetNamespaceManager(_xmlContent);
            XmlNode nodeDefinition = _xmlContent.SelectSingleNode("//*[@form:name='" + fieldName + "']", ns);
            return nodeDefinition;
        }

        /// <summary>
        /// Retourne une collection de noeud xml ou chaque noeud représente une ligne de la chaine en paramètre.
        /// La scission est effectuée sur le motif \r\n
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private ICollection<XmlNode> SplitLines(string value)
        {
            string[] separator = new string[1];
            separator[0] = "\r\n";
            ICollection<XmlNode> retour = new List<XmlNode>();
            if (value.IndexOf(separator[0]) > -1)
            {
                string[] arr = value.Split(separator, System.StringSplitOptions.None);
                foreach (string str in arr)
                {
                    XmlText txt = this._xmlContent.CreateTextNode(str);
                    retour.Add(txt);
                }
            }
            else
            {
                XmlText txt = this._xmlContent.CreateTextNode(value);
                retour.Add(txt);
            }

            return retour;
        }

        /// <summary>
        /// Retourne le champ de fusion de texte definit dans le document
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private XmlNode GetField(string fieldName)
        {
            XmlNamespaceManager ns = this.GetNamespaceManager(_xmlContent);
            XmlNode nodeDefinition = _xmlContent.SelectSingleNode("//text:user-field-decl[@text:name='" + fieldName + "']", ns);
            return nodeDefinition;
        }
    }
}
