using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ULibre.Menu.Implementation
{
    public class MenuBar : Utility.Base.ClassBase, Menu.Interface.IMenuBar
    {
        private static IDictionary<string, string> uri = new Dictionary<string, string>();
        public XmlDocument XmlMenuBar { get; set; }

        public MenuBar()
        {
            if (uri.Count().Equals(0))
            {
                uri.Add("menu", @"http://openoffice.org/2001/menu");
            }
        }

        protected XmlNamespaceManager GetNamespaceManager(XmlDocument content)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(content.NameTable);
            ns.AddNamespace("menu", uri["menu"]);
            return ns;
        }

        /// <summary>
        /// Permet de créer un element xml avec le namespace menu
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private XmlElement CreateMenuElement(string name)
        {
            XmlElement retour = XmlMenuBar.CreateElement("menu", name, uri["menu"] );
            return retour;
        }

        private XmlAttribute CreateMenuAttribute(string name, string value) 
        {
            XmlAttribute att = XmlMenuBar.CreateAttribute("menu", name, uri["menu"]);
            att.Value = value;
            return att;
        }

        /// <summary>
        /// Permet de créer un nouveau menu dans la toolbal
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private XmlElement CreateMenu(string name)
        {
            Utility.Logger.myLogger.Debug("Création du menu:" + name);
            
            XmlElement menu = this.CreateMenuElement("menu");
            XmlElement menupopup = this.CreateMenuElement("menupopup");
         
            XmlAttribute attId = this.CreateMenuAttribute("id",string.Concat("vnd.openoffice.org:", name));
            XmlAttribute attLabel = this.CreateMenuAttribute("label",name);

            menu.Attributes.Append(attId);
            menu.Attributes.Append(attLabel);

            menu.AppendChild(menupopup);
            return menu;
        }

        public void AddMenuItem(string menuContainer, string newItemName, string commandLocation, string value)
        {
            XmlNode container = this.XmlMenuBar.SelectSingleNode("//menu:menu[@menu:label='"+menuContainer+"']/menu:menupopup", this.GetNamespaceManager(this.XmlMenuBar));
            if (container != null)
            {
                string baseCommand = "macro://./{0}("+value+")"; 
                string myCommand = string.Format(baseCommand, commandLocation);
                XmlElement menuitem = this.CreateMenuElement("menuitem");
                menuitem.Attributes.Append(this.CreateMenuAttribute("id", myCommand));
                menuitem.Attributes.Append(this.CreateMenuAttribute("helpid", myCommand));
                menuitem.Attributes.Append(this.CreateMenuAttribute("label", newItemName));
                container.AppendChild(menuitem);
            }
        }
                
        public void AddMenu(string name)
        {
            XmlElement newMenu = this.CreateMenu(name);
            Utility.Logger.myLogger.Debug("Ajout du menu à la définition des menus.");
            if (XmlMenuBar == null || XmlMenuBar.DocumentElement == null)
                this.setError("Le XML du menu est null.");

            XmlMenuBar.DocumentElement.AppendChild(newMenu);
        }

        public void AddSubMenu(string menuContainer, string name)
        {
            XmlElement newMenu = this.CreateMenu(name);
            Utility.Logger.myLogger.Debug("Ajout du menu à la définition des menus pour un container.");
            XmlNode container = this.XmlMenuBar.SelectSingleNode("//menu:menu[@menu:label='"+menuContainer+"']/menu:menupopup", this.GetNamespaceManager(this.XmlMenuBar));
            if (container != null)
            {
                container.AppendChild(newMenu);
            }
        }
    }
}
