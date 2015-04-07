using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility.MyUnityHelper;

namespace TakeDocDataAccess.Document
{
    public class DaoDocument : DaoBase<TakeDocModel.Document>, Interface.IDaoDocument
    {
        public TakeDocModel.Document Create(Guid userId, Guid entityId, Guid documentId, Guid versionId, Guid typeDocumentId, string documentLabel)
        {
            TakeDocModel.Status_Document status = base.Context.Status_Document.Where(x => x.StatusDocumentReference == TakeDocModel.Status_Document.Create && x.EntityId == entityId).ToList().First();
 
            TakeDocModel.Document retour = new TakeDocModel.Document();
            retour.DocumentId = documentId;
            retour.DocumentCurrentVersionId = versionId;
            retour.DocumentReference = base.Context.GenerateReference("Document");
            retour.DateCreateData = System.DateTimeOffset.UtcNow;
            retour.Status_Document = status;
            retour.DocumentStatusId = status.StatusDocumentId;

            retour.DocumentLabel = documentLabel;
            retour.DocumentTypeId = typeDocumentId;

            retour.DocumentOwnerId = userId;
            retour.UserCreateData = userId;
            retour.EntityId = entityId;
            retour.EtatDeleteData = false;

            base.Context.Document.Add(retour);
            base.Context.SaveChanges();

            return retour;
        }
    }
}
