using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MyUnityHelper;
using dataDoc = TakeDocDataAccess.Document;
using System.IO;
using its = iTextSharp.text;
using iTextSharp.text.pdf;

namespace TakeDocService.Print
{
    public class ReportVersionService : BaseService, Interface.IReportVersionService
    {
        Document.Interface.IVersionService servVersion = UnityHelper.Resolve<Document.Interface.IVersionService>();
        TakeDocService.Document.MetaDataService servMetaData = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocService.Document.MetaDataService>();
        TakeDocService.Document.Interface.IImageService servImage = UnityHelper.Resolve<TakeDocService.Document.Interface.IImageService>();
        TraductionService servTraduction = new TraductionService();

        TakeDocDataAccess.DaoBase<TakeDocModel.UserTk> daoUser = new TakeDocDataAccess.DaoBase<TakeDocModel.UserTk>();
        TakeDocDataAccess.DaoBase<TakeDocModel.Entity> daoEntity = new TakeDocDataAccess.DaoBase<TakeDocModel.Entity>();

        protected System.IO.FileInfo GetGenerateFileInfo(string entite, string fileName, string extension)
        {
            string storeLocalPath = string.Concat(@"\", entite, @"\", extension);
            string[] arr = storeLocalPath.Split('/');
            string deep = string.Empty;
            foreach (string s in arr)
            {
                if (string.IsNullOrEmpty(s) == false)
                {
                    deep = string.Concat(deep, @"\", s);
                    if (System.IO.Directory.Exists(deep) == false) System.IO.Directory.CreateDirectory(string.Concat(TakeDocModel.Environnement.VersionStoreUNC, @"\", deep));
                }
            }
            return new System.IO.FileInfo(string.Concat(TakeDocModel.Environnement.VersionStoreUNC, @"\", storeLocalPath, @"\", fileName, ".", extension));
        }


        #region getfile
        /// <summary>
        /// Return file in byte array
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public byte[] GetBinaryFile(Guid versionId, Guid entityId)
        {
            ICollection<TakeDocModel.Version> versions = servVersion.GetBy(x => x.VersionId == versionId && x.EntityId == entityId);
            if (versions.Count() == 0)
            {
                string msg = string.Format("Unknow version {0} for entity {1}", versionId, entityId);
                base.Logger.Error(msg);
                throw new Exception(msg);
            }
            return System.IO.File.ReadAllBytes(string.Concat(TakeDocModel.Environnement.VersionStoreUNC,versions.First().VersionPath));
        }

        public string GetUrlFile(Guid versionId, Guid entityId)
        {
            byte[] data = this.GetBinaryFile(versionId, entityId);
            System.IO.FileInfo file = new System.IO.FileInfo(string.Concat(TakeDocModel.Environnement.TempDirectory, versionId, ".pdf"));
            if (System.IO.File.Exists(file.FullName)) System.IO.File.Delete(file.FullName);
            System.IO.File.WriteAllBytes(file.FullName, data);
            return file.Name;
        }
        #endregion


        #region generate
        private void FillField(TakeDocModel.Version version, TakeDocModel.Entity entity, FileInfo destination)
        {
            TakeDocModel.UserTk user = daoUser.GetBy(x => x.UserTkId == version.Document.DocumentOwnerId).First();

            string directoryEntity = string.Concat(TakeDocModel.Environnement.ModelDirectory, entity.EntityReference);

            ICollection<TakeDocModel.Dto.Document.ReadOnlyMetadata> roMetaDatas = servMetaData.GetReadOnlyMetaData(version);

            ULibre.Drivers.Interface.IDriver model = new ULibre.Drivers.Implementation.OdtDriver();
            model.Open(destination.FullName);
            ICollection<string> title = new List<string>();
            title.Add("Titre");
            title.Add(version.Document.DocumentLabel);
            model.AddLine("TabMetadata", title.ToArray<string>());
            ICollection<string> reference = new List<string>();
            reference.Add("Référence doc.");
            reference.Add(version.Document.DocumentReference);
            model.AddLine("TabMetadata", reference.ToArray<string>());
            foreach (TakeDocModel.Dto.Document.ReadOnlyMetadata ro in roMetaDatas.OrderBy(x => x.DisplayIndex))
            {
                ICollection<string> line = new List<string>();
                line.Add((string.IsNullOrEmpty(ro.Label) ? string.Empty : ro.Label));
                line.Add((string.IsNullOrEmpty(ro.Text) ? string.Empty : servTraduction.Get("fr", ro.Text)));
                model.AddLine("TabMetadata", line.ToArray<string>());
            }
            model.RemoveEmptyLine("TabMetadata");

            FileInfo logo = new FileInfo(string.Concat(directoryEntity,@"\images\Logo.png"));
            if (logo.Exists) model.FillImage("Logo", logo.FullName);

            model.FillField("TypeDocument", version.Document.Type_Document.TypeDocumentLabel);
            model.FillField("Entite", entity.EntityLabel);
            model.FillField("Consultant", string.Concat(user.UserTkFirstName, " ", user.UserTkLastName));

            model.Save();
        }

        /// <summary>
        /// Init tab with metadata name and value
        /// </summary>
        /// <param name="version"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private byte[] GenerateStarterPdf(TakeDocModel.Version version, TakeDocModel.Entity entity)
        {
            byte[] data = null;
            FileInfo modele = new FileInfo(string.Concat(TakeDocModel.Environnement.ModelDirectory, entity.EntityReference, @"\", version.Document.Type_Document.TypeDocumentReference, "_", "starter.odt"));
            if (modele.Exists == false) modele = new FileInfo(string.Concat(TakeDocModel.Environnement.ModelDirectory, "version_starter.odt"));

            FileInfo destinationOdt = new FileInfo(string.Concat(TakeDocModel.Environnement.TempDirectory, Guid.NewGuid().ToString(), modele.Extension));
            FileInfo destinationPdf = new FileInfo(string.Concat(destinationOdt.FullName.Replace(destinationOdt.Extension, ".pdf")));

            modele.CopyTo(destinationOdt.FullName);
            try
            {

                this.FillField(version, entity, destinationOdt);

                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(TakeDocModel.Environnement.BatchConvertPdf);
                info.WorkingDirectory = TakeDocModel.Environnement.TempDirectory;
                info.Arguments = destinationOdt.FullName;
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
                process.WaitForExit();

                data = System.IO.File.ReadAllBytes(destinationPdf.FullName);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex);
            }
            finally
            {
                if (destinationOdt.Exists) destinationOdt.Delete();
                if (destinationPdf.Exists) destinationPdf.Delete();
            }
            return data;
        }

        public byte[] Generate(Guid versionId, Guid entityId)
        {
            TakeDocModel.Version version = servVersion.GetBy(x => x.VersionId == versionId && x.EntityId == entityId).First();
            TakeDocModel.Entity entity = daoEntity.GetBy(x => x.EntityId == entityId).First();
            return this.Generate(version, entity);
        }

        public byte[] Generate(TakeDocModel.Version version, TakeDocModel.Entity entity)
        {
            byte[] data = this.GenerateStarterPdf(version, entity);
            if (data == null) return null;
            PdfReader entetePdf = new PdfReader(data);
            PdfReader imagePdf = servImage.GetImagePdf(version);
            its.Document document = new its.Document(iTextSharp.text.PageSize.A4, -70, -70, 0, 0);
            MemoryStream streamOut = new MemoryStream();
               
            using (PdfCopy copy = new PdfCopy(document, streamOut))
            {
                document.Open();
                for (int p = 1; p <= entetePdf.NumberOfPages; p++)
                {
                    PdfImportedPage page = copy.GetImportedPage(entetePdf, p);
                    copy.AddPage(page);
                }
                copy.FreeReader(entetePdf);
                entetePdf.Close();

                if (imagePdf != null)
                {
                    // add page image to the pdf
                    for (int p = 1; p <= imagePdf.NumberOfPages; p++)
                    {
                        PdfImportedPage page = copy.GetImportedPage(imagePdf, p);
                        copy.AddPage(page);
                    }
                    copy.FreeReader(imagePdf);
                    imagePdf.Close();
                }
            }

            System.IO.FileInfo file = this.GetGenerateFileInfo(entity.EntityReference, version.VersionReference, "pdf");
            System.IO.File.WriteAllBytes(file.FullName, streamOut.ToArray());

            version.VersionPath = file.FullName.Replace(TakeDocModel.Environnement.VersionStoreUNC, string.Empty);
            servVersion.Update(version);
            return streamOut.ToArray();
        }
        #endregion
    }
}
