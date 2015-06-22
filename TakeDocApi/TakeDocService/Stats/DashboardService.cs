using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocDataAccess;
using TakeDocDataAccess.Document;
using TakeDocService.Workflow.Document.Interface;

namespace TakeDocService.Stats
{
    public class DashboardService : BaseService, Interface.IDashboardService
    {
        DaoBase<TakeDocModel.Document> daoDocument = new DaoBase<TakeDocModel.Document>();
        DaoBase<TakeDocModel.View_UserEntity> daoViewUserEntity = new DaoBase<TakeDocModel.View_UserEntity>();
        DaoBase<TakeDocModel.Status_Document> daoStDocument = new DaoBase<TakeDocModel.Status_Document>();

        DaoTypeDocument daoTypeDocument = Utility.MyUnityHelper.UnityHelper.Resolve<DaoTypeDocument>();

        IDocumentToValidate servDocumentToValidate = Utility.MyUnityHelper.UnityHelper.Resolve<IDocumentToValidate>();

        public ICollection<TakeDocModel.Dto.Stats.StatsDocument> GetDashboard(Guid userId)
        {
            //ICollection<TakeDocModel.Status_Document> statusDocument = daoStDocument.GetBy(x => x.)
            ICollection<TakeDocModel.Document> documents = daoDocument.GetBy(x =>
                    (x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Create
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Incomplete
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Complete
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.ToValidate
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Approve
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Refuse
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Archive)
                    && x.DocumentOwnerId == userId && x.EtatDeleteData == false).ToList();
            
            int nbCreate = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Create).Count();
            int nbIncomplete = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Incomplete).Count();
            int nbComplete = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Complete).Count();
            int toValidate = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.ToValidate).Count();
            int approve = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Approve).Count();
            int refuse = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Refuse).Count();
            int archive = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Archive).Count();

            ICollection<TakeDocModel.Dto.Stats.StatsDocument> back = new List<TakeDocModel.Dto.Stats.StatsDocument>();
            back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
            {
                StatusReference = TakeDocModel.Status_Document.Create,
                Count = nbCreate
            });
            back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
            {
                StatusReference = TakeDocModel.Status_Document.Incomplete,
                Count = nbIncomplete
            });
            back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
            {
                StatusReference = TakeDocModel.Status_Document.Complete,
                Count = nbComplete
            });
            back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
            {
                StatusReference = TakeDocModel.Status_Document.ToValidate,
                Count = toValidate
            });
            back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
            {
                StatusReference = TakeDocModel.Status_Document.Approve,
                Count = approve
            });
            back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
            {
                StatusReference = TakeDocModel.Status_Document.Refuse,
                Count = refuse
            });
            back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
            {
                StatusReference = TakeDocModel.Status_Document.Archive,
                Count = archive
            });

            return back;
        }

        public ICollection<TakeDocModel.Dto.Stats.StatsDocument> GetDetailedDashboard(Guid userId)
        {
            ICollection<TakeDocModel.TypeDocument> types = daoTypeDocument.GetAll();

            ICollection<TakeDocModel.Dto.Stats.StatsDocument> back = new List<TakeDocModel.Dto.Stats.StatsDocument>();
            ICollection<TakeDocModel.View_UserEntity> userEntitys = daoViewUserEntity.GetBy(x => x.UserTkId == userId);
            ICollection<TakeDocModel.Status_Document> statusDocument = daoStDocument.GetAll();

            foreach (TakeDocModel.View_UserEntity vue in userEntitys)
            {
                //*******************************************
                // my document
                //*******************************************
                ICollection<TakeDocModel.Document> documents = daoDocument.GetBy(x =>
                    (x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Create
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Incomplete
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Complete
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.ToValidate
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Approve
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Refuse
                    || x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Archive)
                    && x.EntityId == vue.EntityId && x.DocumentOwnerId == vue.UserTkId && x.EtatDeleteData == false).ToList();
                
                foreach (TakeDocModel.TypeDocument type in types.Where(x => x.EntityId == vue.EntityId && x.EtatDeleteData == false))
                {
                    Guid typeDocumentId = type.TypeDocumentId;
                    int nbCreate = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Create && x.Type_Document.TypeDocumentId == typeDocumentId).Count();
                    int nbIncomplete = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Incomplete && x.Type_Document.TypeDocumentId == typeDocumentId).Count();
                    int nbComplete = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Complete && x.Type_Document.TypeDocumentId == typeDocumentId).Count();
                    int toValidate = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.ToValidate && x.Type_Document.TypeDocumentId == typeDocumentId).Count();
                    int approve = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Approve && x.Type_Document.TypeDocumentId == typeDocumentId).Count();
                    int refuse = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Refuse && x.Type_Document.TypeDocumentId == typeDocumentId).Count();
                    int archive = documents.Where(x => x.Status_Document.StatusDocumentReference == TakeDocModel.Status_Document.Archive && x.Type_Document.TypeDocumentId == typeDocumentId).Count();
                    
                    back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.Create,
                        StatusLabel = statusDocument.Where(x => x.EntityId == type.EntityId && x.StatusDocumentReference == TakeDocModel.Status_Document.Create).First().StatusDocumentLabel,
                        Count = nbCreate
                    });
                    back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.Incomplete,
                        StatusLabel = statusDocument.Where(x => x.EntityId == type.EntityId && x.StatusDocumentReference == TakeDocModel.Status_Document.Incomplete).First().StatusDocumentLabel,
                        Count = nbIncomplete
                    });
                    back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.Complete,
                        StatusLabel = statusDocument.Where(x => x.EntityId == type.EntityId && x.StatusDocumentReference == TakeDocModel.Status_Document.Complete).First().StatusDocumentLabel,
                        Count = nbComplete
                    });
                    back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.ToValidate,
                        StatusLabel = statusDocument.Where(x => x.EntityId == type.EntityId && x.StatusDocumentReference == TakeDocModel.Status_Document.ToValidate).First().StatusDocumentLabel,
                        Count = toValidate
                    });
                    back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.Approve,
                        StatusLabel = statusDocument.Where(x => x.EntityId == type.EntityId && x.StatusDocumentReference == TakeDocModel.Status_Document.Approve).First().StatusDocumentLabel,
                        Count = approve
                    });
                    back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.Refuse,
                        StatusLabel = statusDocument.Where(x => x.EntityId == type.EntityId && x.StatusDocumentReference == TakeDocModel.Status_Document.Refuse).First().StatusDocumentLabel,
                        Count = refuse
                    });
                    back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
                    {
                        EntityId = vue.EntityId,
                        EntityReference = vue.EntityReference,
                        TypeDocumentId = type.TypeDocumentId,
                        TypeDocumentReference = type.TypeDocumentReference,
                        TypeDocumentLabel = type.TypeDocumentLabel,
                        StatusReference = TakeDocModel.Status_Document.Archive,
                        StatusLabel = statusDocument.Where(x => x.EntityId == type.EntityId && x.StatusDocumentReference == TakeDocModel.Status_Document.Archive).First().StatusDocumentLabel,
                        Count = archive
                    });
                }
            }
            //***************************************************
            // document that i must validate(approve or refuse)
            //***************************************************
            ICollection<TakeDocModel.DocumentToValidate> toValidates = servDocumentToValidate.GetAll(userId);
            int toValidateAsManager = toValidates.Where(x => x.TypeValidation == "MANAGER").Count();
            int toValidateAsBackOffice = toValidates.Where(x => x.TypeValidation == "TYPE_DOCUMENT").Count();

            back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
            {
                EntityId = Guid.Empty,
                EntityReference = null,
                TypeDocumentId = Guid.Empty,
                TypeDocumentReference = null,
                TypeDocumentLabel = null,
                StatusReference = "TO_VALIDATE_MANAGER",
                Count = toValidateAsManager
            });

            back.Add(new TakeDocModel.Dto.Stats.StatsDocument()
            {
                EntityId = Guid.Empty,
                EntityReference = null,
                TypeDocumentId = Guid.Empty,
                TypeDocumentReference = null,
                TypeDocumentLabel = null,
                StatusReference = "TO_VALIDATE_BACKOFFICE",
                Count = toValidateAsBackOffice
            });
            return back.OrderBy(x => x.EntityReference).ToList();
        }
    }
}
