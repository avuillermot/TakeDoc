using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeDocDataAccess.Document.Interface;
using Utility.MyUnityHelper;

namespace TakeDocService.Document
{
    public class TypeDocumentService :BaseService, Interface.ITypeDocumentService
    {
        private IDaoTypeDocument daoTypeDocument = UnityHelper.Resolve<IDaoTypeDocument>();

        public ICollection<TakeDocModel.TypeDocument> Get(Guid userId, Guid entityId)
        {
            return daoTypeDocument.GetBy(x => x.EntityId == entityId, x => x.DataField);
        }
    }
}
