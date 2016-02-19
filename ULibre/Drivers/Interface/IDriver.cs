using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ULibre.Drivers.Interface
{
    public interface IDriver
    {
        /// <summary>
        /// Rempli le champ indiqué avec la valeur indiqué
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        void FillField(string fieldName, string value);
        void FillField(Object obj);
        void FillField(IDictionary<string, string> dictionnary);
        /// <summary>
        /// Rempli le champ avec une image
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value">chemin d'accès complet ou paramètre ImagesDirectory(appSettings) plus la valeur en paramètre</param>
        void FillImage(string fieldName, string value);
        /// <summary>
        /// Rempli le champ avec une image
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value">image format base 64</param>
        void FillImageBase64(string fieldName, string base64);
        /// <summary>
        /// Ajout une ligne à un tableau avec les valeurs en parametres
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values"></param>
        void AddLine(string tableName, string[] values);
        /// <summary>
        /// Retourne la valeur du champ
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        string GetFieldValue(string fieldName);
        /// <summary>
        /// Ouvre un document en modification
        /// </summary>
        /// <param name="fileFullName"></param>
        void Open(string fileFullName);
        /// <summary>
        /// Enregistre un document après modification
        /// </summary>
        void Save();
        string GetContent();
        /// <summary>
        /// Indique si le fichier contient des macros
        /// </summary>
        /// <param name="file"></param>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        bool HasBasicMacro(System.IO.FileInfo file, string workingDirectory);
        /// <summary>
        /// Permet d'acceder la bar de menu du document, doit être initialiser avant
        /// </summary>
        Menu.Interface.IMenuBar MenuBar { get; set; }
        void InitMenuBar();
        /// <summary>
        /// Ajoute une toolbar dans le document Office
        /// </summary>
        /// <param name="toolbarName"></param>
        //void AddToolBar(string toolbarName);
        /// <summary>
        /// Ajoute un item de menu dans le document Office
        /// </summary>
        /// <param name="menuContainer"></param>
        /// <param name="newItemName"></param>
        /// <param name="value"></param>
        void AddInsertionMenuItem(string menuContainer, string newItemName,  string value);
        void RemoveEmptyLine(string tableName);
    }
}
