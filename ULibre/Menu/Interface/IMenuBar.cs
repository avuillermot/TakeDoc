using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ULibre.Menu.Interface
{
    /// <summary>
    /// Classe de gestion des menus dans la toolbar LibreOffice
    /// </summary>
    public interface IMenuBar
    {
        void AddSubMenu(string menuContainer, string name);
        void AddMenuItem(string menuContainer, string newItemName, string commandLocation, string value);
        void AddMenu(string name);
        /// <summary>
        /// Fichier xml contenant la définition du menu
        /// </summary>
        XmlDocument XmlMenuBar { get; set; }
    }
}
