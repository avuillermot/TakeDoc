using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
namespace ULibre.Drivers.Implementation
{
    public abstract class LibreOfficeDriver : Utility.Base.ClassBase
    {
        private static IDictionary<string, string> uri = new Dictionary<string, string>();

        protected string _contentPathFile = "content.xml";
        protected string _stylesPathFile = "styles.xml";
        protected string _manifestPathFile = "META-INF/manifest.xml";
        protected string _menuBarPathFile = "Configurations2/menubar/menubar.xml";
        protected FileInfo _officeDocument = null;
        protected XmlDocument _xmlContent = null;
        protected XmlDocument _xmlStyles = null;
        public Menu.Interface.IMenuBar MenuBar { get; set; }
        protected Utils.ZipManager zipManager = new Utils.ZipManager();

        private static IDictionary<string, string> _menubars = new Dictionary<string, string>();
        private static IDictionary<string, string> _modules = new Dictionary<string, string>();

        private static string menubarsDirectory = Utility.Configuration.ConfigurationHelper.GetAppSettings("menubarsDirectory");
        private static string modulesDirectory = Utility.Configuration.ConfigurationHelper.GetAppSettings("modulesDirectory");

        private static object _lock = new object();

        public LibreOfficeDriver()
        {
            MenuBar = new Menu.Implementation.MenuBar();
            lock (_lock)
            {
                if (_modules.Count().Equals(0))
                {
                    try
                    {
                        string[] files = System.IO.Directory.GetFiles(modulesDirectory, "*.xml");
                        foreach (string file in files)
                        {
                            Utility.Logger.myLogger.Debug("Chargement module:" + file);
                            _modules.Add(new FileInfo(file).Name, file);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.setError(ex);
                        this.setError(string.Concat("Erreur lors du chargement des modules :", modulesDirectory));
                    }
                }
                if (_menubars.Count().Equals(0))
                {
                    try
                    {
                        string[] files = System.IO.Directory.GetFiles(menubarsDirectory, "*.xml");
                        foreach (string file in files)
                        {
                            Utility.Logger.myLogger.Debug("Chargement menubar:" + file);
                            _menubars.Add(new FileInfo(file).Name, file);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.setError(ex);
                        this.setError(string.Concat("Erreur lors du chargement des menubars :", menubarsDirectory));
                    }
                }

                if (uri.Count().Equals(0))
                {
                    uri.Add("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
                    uri.Add("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
                    uri.Add("draw", "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
                    uri.Add("table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
                    uri.Add("manifest", "urn:oasis:names:tc:opendocument:xmlns:manifest:1.0");
                    uri.Add("library", "http://openoffice.org/2000/library");
                    uri.Add("form", "urn:oasis:names:tc:opendocument:xmlns:form:1.0");
                    //uri.Add("forms", "http://www.w3.org/2002/xforms");
                    //uri.Add("xlink", "http://www.w3.org/1999/xlink");
                    uri.Add("number", "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
                    /*uri.Add("ooow", "http://openoffice.org/2004/writer");
                    uri.Add("rpt", "http://openoffice.org/2005/report");
                    uri.Add("grddl", "http://www.w3.org/2003/g/data-view#");*/
                    uri.Add("chart", "urn:oasis:names:tc:opendocument:xmlns:chart:1.0");
                }
            }
        }

        /// <summary>
        /// Retourne le XmlNamespaceManager adequat
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected XmlNamespaceManager GetNamespaceManager(XmlDocument content)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(content.NameTable);
            foreach (KeyValuePair<string, string> kv in uri)
            {
                ns.AddNamespace(kv.Key, kv.Value);
            }
            return ns;
        }
        
        public string GetContent()
        {
            return this._xmlContent.OuterXml;
        }
        
        /// <summary>
        /// Ouvre un document et construit le document de travail
        /// </summary>
        /// <param name="modeleFullName"></param>
        public void Open(string fileFullName)
        {
            _officeDocument = new FileInfo(fileFullName);
            this.Load(_officeDocument);
        }

        /// <summary>
        /// Décompresse et charge le contenu du document
        /// </summary>
        /// <param name="src"></param>
        protected void Load(FileInfo src)
        {
            _xmlContent = this.GetXmlDocument(zipManager.extractEntry(this._officeDocument.FullName, this._contentPathFile));
            if (zipManager.contains(this._officeDocument, this._stylesPathFile))
                _xmlStyles = this.GetXmlDocument(zipManager.extractEntry(this._officeDocument.FullName, this._stylesPathFile));
            
            if (zipManager.contains(this._officeDocument,this._menuBarPathFile))
            {
                XmlDocument myMenu = this.GetXmlDocument(zipManager.extractEntry(this._officeDocument.FullName, this._menuBarPathFile));
                this.MenuBar.XmlMenuBar = myMenu;
            }

        }
                       
        /// <summary>
        /// Enregistre le document
        /// </summary>
        public void Save()
        {
            Utility.Logger.myLogger.Debug("Sauvegarde du document.");
            this.SaveZipEntry(this._xmlContent, this._contentPathFile);
            this.SaveZipEntry(this._xmlStyles, this._stylesPathFile);
            if (this.MenuBar.XmlMenuBar != null)
            {
                this.SaveZipEntry(this.MenuBar.XmlMenuBar, this._menuBarPathFile);
            }
        }

        /// <summary>
        /// Sauvegarde dans le document Office au le path indiqué le document XML
        /// </summary>
        /// <param name="xml">xml to save</param>
        /// <param name="path">path in document (zip)</param>
        private void SaveZipEntry(XmlDocument xml, string path)
        {
            MemoryStream ms = new MemoryStream();
            xml.Save(ms);
            zipManager.addZipEntry(this._officeDocument.FullName, path, ms);
        }
        
        public bool HasBasicMacro(FileInfo file, string workingDirectory)
        {
            string basicFile = "Basic/script-lc.xml";
            return zipManager.contains(file, basicFile);
        }

        private XmlDocument GetXmlDocumentFromFile(string fullPath)
        {
            XmlTextReader myTextReader = new XmlTextReader(fullPath);
            return this.LoadXmlDocument(myTextReader);
        }

        private XmlDocument GetXmlDocument(string xml)
        {
            XmlDocument retour = new XmlDocument();
            byte[] encodedString = Encoding.UTF8.GetBytes(xml);

            // Put the byte array into a stream and rewind it to the beginning
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;
           
            StreamReader reader = new StreamReader(ms);
            XmlTextReader myTextReader = new XmlTextReader(reader);
            return this.LoadXmlDocument(myTextReader);
        }

        private XmlDocument LoadXmlDocument(XmlTextReader reader) {
            XmlDocument retour = new XmlDocument();
            reader.XmlResolver = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;
            using (XmlReader xr = XmlReader.Create(reader, settings))
            {
                retour.Load(xr);
            }
            return retour;
        }
        
        /// <summary>
        /// Ajoute un fichier au manifest du document
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="zipFilePath"></param>
        private void AddFileToManifest(string zipFile, string mediaTypeValue, string zipFilePath)
        {
             string manifestContent = zipManager.extractEntry(zipFile, this._manifestPathFile);
             if (string.IsNullOrEmpty(manifestContent) == false)
             {
                 XmlDocument manifest = this.GetXmlDocument(manifestContent);
                 XmlNode racine = manifest.DocumentElement;

                 XmlElement fileEntry = manifest.CreateElement("manifest", "file-entry", uri["manifest"]);

                 XmlAttribute mediaType = manifest.CreateAttribute("manifest", "media-type", uri["manifest"]);
                 mediaType.Value = mediaTypeValue;

                 XmlAttribute fullPath = manifest.CreateAttribute("manifest", "full-path", uri["manifest"]);
                 fullPath.Value = zipFilePath;

                 fileEntry.Attributes.Append(mediaType);
                 fileEntry.Attributes.Append(fullPath);

                 racine.AppendChild(fileEntry);
                 this.SaveZipEntry(manifest, this._manifestPathFile);
             }
        }

        /// <summary>
        /// Ajoute le menu par défaut dans le document Office
        /// </summary>
        public void InitMenuBar()
        {
            this.AddFileToManifest(this._officeDocument.FullName, string.Empty, this._menuBarPathFile);

            XmlDocument menubar = this.GetXmlDocumentFromFile(_menubars["menubar.xml"]);
            this.MenuBar.XmlMenuBar = menubar;
            MemoryStream ms = new MemoryStream();
            menubar.Save(ms);
            zipManager.addZipEntry(this._officeDocument.FullName, this._menuBarPathFile, ms);
        }

        /// <summary>
        /// Ajoute un module de macro dans le document Office
        /// </summary>
        /// <param name="moduleName"></param>
        protected void AddModule(string moduleName, string trigramme)
        {
            Utility.Logger.myLogger.Debug("Ajout des parties au manifest du document.");
            string modulePath = string.Concat("Basic/Fiducial/" + moduleName);
            string scriptLcPath = "Basic/script-lc.xml";
            string scriptLbPath = "Basic/Fiducial/script-lb.xml";
            // Ajout des parties au manifest
            if (zipManager.contains(this._officeDocument,"Basic/") == false) 
                this.AddFileToManifest(this._officeDocument.FullName, string.Empty, "Basic/");
            if (zipManager.contains(this._officeDocument, "Basic/Fiducial/") == false)
                this.AddFileToManifest(this._officeDocument.FullName, string.Empty, "Basic/Fiducial/");
            if (zipManager.contains(this._officeDocument, modulePath) == false) 
                this.AddFileToManifest(this._officeDocument.FullName, "text/xml", modulePath);

            if (zipManager.contains(this._officeDocument, scriptLcPath) == false)
                this.AddFileToManifest(this._officeDocument.FullName, "text/xml", scriptLcPath);
             if (zipManager.contains(this._officeDocument, scriptLbPath) == false)
                this.AddFileToManifest(this._officeDocument.FullName, "text/xml", scriptLbPath);

            Utility.Logger.myLogger.Debug("Chargement du module :" + _modules[moduleName]);
            XmlDocument module = this.GetXmlDocumentFromFile(_modules[moduleName]);
            Utility.Logger.myLogger.Debug("Ajout dans l'entry :" + modulePath);
            this.SaveZipEntry(module, modulePath);

            // le fichier script-lc existe t il déja ?
            // il s'agit du fichier des descriptions des modules présents dans le document Office
            XmlDocument lc = new XmlDocument();
            if (zipManager.contains(this._officeDocument, scriptLcPath) == false)
            {
                Utility.Logger.myLogger.Debug("Chargement du script-lc standard.");
                // si pas présent on charge le document standard
                lc = this.GetXmlDocumentFromFile(_modules["script-lc.xml"]);
            }
            else {
                // si présent on ajoute une entrée pour le module DSI
                Utility.Logger.myLogger.Debug("Chargement du script-lc existant dans le fichier Office.");
                string contentScriptLc = zipManager.extractEntry(this._officeDocument.FullName, scriptLcPath);
                lc = this.GetXmlDocument(contentScriptLc);

                XmlElement librairy = lc.CreateElement("librairy", "library", uri["library"]);

                XmlAttribute attName = lc.CreateAttribute("library", "name", uri["library"]);
                attName.Value = "Fiducial";

                XmlAttribute attLink = lc.CreateAttribute("library", "link", uri["library"]);
                attLink.Value = "false";

                librairy.Attributes.Append(attName);
                librairy.Attributes.Append(attLink);

                lc.DocumentElement.AppendChild(librairy);
            }
            Utility.Logger.myLogger.Debug("Sauvegarde du script-lc.");
            this.SaveZipEntry(lc, scriptLcPath);

            Utility.Logger.myLogger.Debug("Gestion du script-lb.");
            string fileTrigramme = string.Format("script-lb-{0}.xml", trigramme);
            XmlDocument lb = this.GetXmlDocumentFromFile(_modules[fileTrigramme]);
            // on ajoute le fichier de description du module
            this.SaveZipEntry(lb, scriptLbPath);
        }
        
        protected void ExceptionImplementation()
        {
            throw new Exception("Cette fonction/propriété n'est pas implémentée.");
        }


        public bool Exists(string name)
        {
            XmlNamespaceManager ns = this.GetNamespaceManager(this._xmlContent);
            XmlNode node = _xmlContent.SelectSingleNode("//table:table[@table:name='" + name + "']", ns);
            return node != null;
        }
    }
}
