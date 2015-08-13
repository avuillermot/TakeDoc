using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MyUnityHelper;

namespace TakeDocService.Document
{
    public class DocumentCompleteService : Interface.IDocumentCompleteService
    {
        TakeDocDataAccess.Security.Interface.IDaoUserTk daoUser = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Security.Interface.IDaoUserTk>();
        TakeDocDataAccess.DaoBase<TakeDocModel.Entity> daoEntity = new TakeDocDataAccess.DaoBase<TakeDocModel.Entity>();
        TakeDocDataAccess.Document.Interface.IDaoView_DocumentExtended daoDocExtended = Utility.MyUnityHelper.UnityHelper.Resolve<TakeDocDataAccess.Document.Interface.IDaoView_DocumentExtended>();

        Interface.IDocumentService servDocument = UnityHelper.Resolve<Interface.IDocumentService>();
        Interface.IMetaDataService servMetaData = UnityHelper.Resolve<Interface.IMetaDataService>();
        Interface.IPageService servPage = UnityHelper.Resolve<Interface.IPageService>();
        Interface.IVersionService servVersion = UnityHelper.Resolve<Interface.IVersionService>();

        public TakeDocModel.Dto.Document.DocumentComplete GetByVersion(Guid versionId, Guid userId, Guid entityId)
        {
            TakeDocModel.Dto.Document.DocumentComplete back = new TakeDocModel.Dto.Document.DocumentComplete();
            back.Document = daoDocExtended.GetBy(x => x.VersionId == versionId && x.EntityId == entityId).First();
            back.MetaDatas = servMetaData.GetByVersion(back.Document.VersionId, back.Document.EntityId);
            back.Pages = servVersion.GetPages(back.Document.VersionId, back.Document.EntityId, userId);

            return back;
        }

        public void Update(Guid userId, Guid entityId, string json, bool startWorkflow)
        {
            Newtonsoft.Json.Linq.JArray data = Newtonsoft.Json.Linq.JArray.Parse(json);
            Newtonsoft.Json.Linq.JObject document = String.IsNullOrEmpty(data[0].ToString()) ? null : (Newtonsoft.Json.Linq.JObject)data[0];
            Newtonsoft.Json.Linq.JArray metadatas = String.IsNullOrEmpty(data[1].ToString()) ? null : (Newtonsoft.Json.Linq.JArray)data[1];
            Newtonsoft.Json.Linq.JArray pages = String.IsNullOrEmpty(data[2].ToString()) ? null :  (Newtonsoft.Json.Linq.JArray)data[2];
            Guid versionId = new Guid(document.Value<string>("versionId"));

            TakeDocModel.Entity entity = daoEntity.GetBy(x => x.EntityId == entityId).First();
            TakeDocModel.Version version = servVersion.GetById(versionId);
            TakeDocModel.UserTk user = daoUser.GetBy(x => x.UserTkId == userId).First();
            
            servDocument.Update(user, entity, version, document, metadatas, startWorkflow);
            
        }
    }
}
