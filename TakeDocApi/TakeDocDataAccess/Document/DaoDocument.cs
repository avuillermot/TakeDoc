﻿using System;
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
            TakeDocModel.StatutDocument statut = base.Context.Statut_Document.Where(x => x.StatutDocumentReference == TakeDocModel.StatutDocument.Create).ToList().First();
 
            TakeDocModel.Document retour = new TakeDocModel.Document();
            retour.DocumentId = documentId;
            retour.DocumentCurrentVersion = versionId;
            retour.DocumentReference = base.Context.GenerateReference("Document");
            retour.DateCreateData = System.DateTimeOffset.UtcNow;
            retour.Statut_Document = statut;
            retour.DocumentSatutId = statut.StatutDocumentId;

            retour.DocumentLabel = documentLabel;
            retour.DocumentTypeId = typeDocumentId;

            retour.UserCreateData = userId;
            retour.EntityId = entityId;
            retour.EtatDeleteData = false;

            base.Context.Document.Add(retour);
            base.Context.SaveChanges();

            return retour;
        }
    }
}
