using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document
{
    public class DataFieldService : TakeDocService.BaseService, Interface.IDataFieldService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.View_TypeDocumentDataField> daoDataField = new TakeDocDataAccess.DaoBase<TakeDocModel.View_TypeDocumentDataField>();

        public ICollection<TakeDocModel.View_TypeDocumentDataField> GetDataField(Guid typeDocumentId, Guid entityId)
        {
            ICollection<TakeDocModel.View_TypeDocumentDataField> dataFields = daoDataField.GetBy(x => x.TypeDocumentId == typeDocumentId && x.EntityId == entityId && x.EtatDeleteData == false);
            return dataFields;
        }

        public ICollection<TakeDocModel.View_TypeDocumentDataField> GetDataField(ICollection<string> fields, Guid entityId)
        {
            ICollection<TakeDocModel.View_TypeDocumentDataField> dataFields = 
                daoDataField.GetBy(x => fields.Contains(x.Reference) && x.EntityId == entityId && x.EtatDeleteData == false);
            return dataFields;
        }
    }
}
