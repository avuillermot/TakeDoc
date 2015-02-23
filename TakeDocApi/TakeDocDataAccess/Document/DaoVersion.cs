using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeDocDataAccess.Document
{
    public class DaoVersion : DaoBase<TakeDocModel.Version>, Interface.IDaoVersion
    {
        public TakeDocModel.Version Create(Guid userId, Guid entityId, Guid versionId, Guid documentId, decimal versionNumber)
        {
            TakeDocModel.StatutVersion statut = base.Context.Statut_Version.Where(x => x.StatutVersionReference == TakeDocModel.StatutVersion.Create).ToList().First();

            TakeDocModel.Version retour = new TakeDocModel.Version();
            retour.VersionId = versionId;
            retour.VersionReference = base.Context.GenerateReference("Version");
            retour.EntityId = entityId;
            retour.Statut_Version = statut;
            retour.VersionStatutId = statut.StatutVersionId;
            retour.VersionDocumentId = documentId;
            retour.VersionMajor = ((versionNumber % 1) == 0);
            retour.VersionNumber = versionNumber;

            retour.DateCreateData = System.DateTimeOffset.UtcNow;
            retour.UserCreateData = userId;
            retour.EtatDeleteData = false;

            base.Context.Version.Add(retour);
            ctx.SaveChanges();

            return retour;
        }

        public TakeDocModel.Version GetLastVersion(Guid documentId)
        {
            TakeDocModel.Version retour = base.Context.Version.Where(x => x.VersionDocumentId == documentId).OrderByDescending(x => x.VersionNumber).First();
            return retour;
        }
    }
}
