using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Stats
{
    public class DashboardService : BaseService, Interface.IDashboardService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.Document> daoDocument = new TakeDocDataAccess.DaoBase<TakeDocModel.Document>();
        TakeDocDataAccess.DaoBase<TakeDocModel.View_UserEntity> daoViewUserEntity = new TakeDocDataAccess.DaoBase<TakeDocModel.View_UserEntity>();
        TakeDocDataAccess.Document.DaoTypeDocument daoTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Document.DaoTypeDocument>();

        public ICollection<TakeDocModel.Dto.Stats.StatusDocument> GetDashboard(Guid userId)
        {
            ICollection<TakeDocModel.TypeDocument> types = daoTypeDocument.GetAll();

            ICollection<TakeDocModel.Dto.Stats.StatusDocument> back = new List<TakeDocModel.Dto.Stats.StatusDocument>();
            ICollection<TakeDocModel.View_UserEntity> userEntitys = daoViewUserEntity.GetBy(x => x.UserTkId == userId);
            foreach (TakeDocModel.View_UserEntity vue in userEntitys.Where(x => x.EtatDeleteData == false))
            {
                ICollection<TakeDocModel.Document> documents = daoDocument.GetBy(x =>
                    (x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Create
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Incomplete
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Complete
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Send)
                    && x.EntityId == vue.EntityId && x.DocumentOwner == vue.UserTkId).ToList();

                foreach (TakeDocModel.TypeDocument type in types.Where(x => x.EntityId == vue.EntityId && x.EtatDeleteData == false))
                {
                    Guid typeDocumentId = type.TypeDocumentId;
                    int nbCreate = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Create && x.Type_Document.TypeDocumentId == typeDocumentId).Count();
                    int nbIncomplete = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Incomplete && x.Type_Document.TypeDocumentId == typeDocumentId).Count();
                    int nbComplete = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Complete && x.Type_Document.TypeDocumentId == typeDocumentId).Count();
                    int nbSend = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Send && x.Type_Document.TypeDocumentId == typeDocumentId).Count();

                    back.Add(new TakeDocModel.Dto.Stats.StatusDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.Create,
                        Count = nbCreate
                    });
                    back.Add(new TakeDocModel.Dto.Stats.StatusDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.Incomplete,
                        Count = nbIncomplete
                    });
                    back.Add(new TakeDocModel.Dto.Stats.StatusDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.Complete,
                        Count = nbComplete
                    });
                    back.Add(new TakeDocModel.Dto.Stats.StatusDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.Send,
                        Count = nbSend
                    });
                }
            }
            return back.OrderBy(x => x.EntityReference).ToList();
        }
    }
}
