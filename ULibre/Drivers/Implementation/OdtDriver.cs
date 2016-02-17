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

        public void AddLineImage(string tableName, string title, string code, string base64)
        {
            XmlNamespaceManager ns = this.GetNamespaceManager(this._xmlContent);
            XmlNode nodeTable = this.GetTable(tableName);
            XmlNode lastRow = null;
            foreach (XmlNode row in nodeTable.SelectNodes("table:table-row", ns))
            {
                lastRow = row;
            }
            lastRow = lastRow.CloneNode(true);
            int index = 0;
            foreach (XmlNode cell in lastRow.SelectNodes("table:table-cell", ns))
            {
                XmlNode txt = cell.SelectSingleNode("text:p", ns);
                txt.RemoveAll();

                ICollection<XmlNode> nodesInText = this.SplitLines(title);
                if (index == 0 && nodesInText.Count() > 0)
                {
                    txt.AppendChild(nodesInText.First());
                }
                else if (index == 1)
                {
                    
                    /*
                     * <draw:image d7p1:href="Pictures/SIGN_CLIENT.png" 
                     * d7p2:href="simple" d7p3:href="onLoad" xmlns:d7p3="actuate" 
                     * xmlns:d7p2="type" xmlns:d7p1="xlink" xmlns:draw="draw" />
          </*/
                    XmlNode nodeFrame = _xmlContent.CreateElement("draw", "frame", "draw");
                    XmlAttribute attName = _xmlContent.CreateAttribute("draw", "name", "draw");
                    attName.Value = code;
                    nodeFrame.Attributes.Append(attName);

                    XmlAttribute attStyle = _xmlContent.CreateAttribute("draw", "style-name", "draw");
                    attStyle.Value = "fr1";
                    nodeFrame.Attributes.Append(attStyle);

                    XmlAttribute attAnchor = _xmlContent.CreateAttribute("text", "anchor-type", "draw");
                    attAnchor.Value = "paragraph";
                    nodeFrame.Attributes.Append(attAnchor);

                    XmlAttribute attIndex = _xmlContent.CreateAttribute("text", "z-index", "draw");
                    attIndex.Value = "1";
                    nodeFrame.Attributes.Append(attIndex);

                    XmlAttribute attHeight = _xmlContent.CreateAttribute("svg", "height", "draw");
                    attHeight.Value = "1.852cm";
                    nodeFrame.Attributes.Append(attHeight);

                    XmlAttribute attWidth = _xmlContent.CreateAttribute("svg", "width", "draw");
                    attIndex.Value = "3.627cm";
                    nodeFrame.Attributes.Append(attIndex);

                    XmlNode nodeImage = _xmlContent.CreateElement("draw", "image", "draw"); //"//draw:frame[@draw:name='" + code + "']", ns);
                    
                    XmlAttribute attHref = _xmlContent.CreateAttribute("xlink", "href","xlink");
                    attHref.Value = string.Concat("Pictures/", code, ".png");
                    nodeImage.Attributes.Append(attHref);

                    XmlAttribute attType = _xmlContent.CreateAttribute("xlink", "type", "xlink");
                    attType.Value = "simple";
                    nodeImage.Attributes.Append(attType);

                    XmlAttribute attShow = _xmlContent.CreateAttribute("xlink", "show", "xlink");
                    attShow.Value = "embed";
                    nodeImage.Attributes.Append(attShow);

                    XmlAttribute attActuate = _xmlContent.CreateAttribute("xlink", "actuate", "xlink");
                    attActuate.Value = "onLoad";
                    nodeImage.Attributes.Append(attActuate);

                    txt.AppendChild(nodeFrame);
                    nodeFrame.AppendChild(nodeImage);

                    byte[] data = Convert.FromBase64String(base64.Replace("data:image/png;base64,",string.Empty));
                    MemoryStream ms = new MemoryStream(data);
                    // remplacement de l'ancienne image
                    zipManager.addZipEntry(this._officeDocument.FullName, string.Concat("Pictures/", code, ".png"), ms);
                }
                index++;
            }
            nodeTable.AppendChild(lastRow);
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
            XmlNode lastRow = null;
            foreach (XmlNode row in nodeTable.SelectNodes("table:table-row",ns))
            {
                lastRow = row;
            }
            lastRow = lastRow.CloneNode(true);
            int index = 0;
            foreach (XmlNode cell in lastRow.SelectNodes("table:table-cell",ns)) 
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

        /*private void AddImage(XmlNode nodeFrame, XmlNamespaceManager ns, string base64)
        {
            if (nodeFrame != null)
            {
                XmlNode nodeImage = nodeFrame.SelectSingleNode("draw:image", ns);
                if (nodeImage != null)
                {
                    string odtImageName = nodeImage.Attributes["xlink:href"].Value;

                    byte[] data = Convert.FromBase64String(base64);
                    MemoryStream ms = new MemoryStream(data);
                    // remplacement de l'ancienne image
                    zipManager.addZipEntry(this._officeDocument.FullName, odtImageName, ms);
                }
            }
        }*/

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
