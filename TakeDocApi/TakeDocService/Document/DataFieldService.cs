using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document
{
    public class DataFieldService : TakeDocService.BaseService, Interface.IDataFieldService
    {
        TakeDocDataAccess.DaoBase<TakeDocModel.DataField> daoDataField = new TakeDocDataAccess.DaoBase<TakeDocModel.DataField>();

        public ICollection<TakeDocModel.DataField> GetDataField(Guid typeDocumentId, Guid entityId)
        {
            ICollection<TakeDocModel.DataField> dataFields = daoDataField.GetBy(x => x.TypeDocumentId == typeDocumentId && x.EntityId == entityId && x.EtatDeleteData == false);
            return dataFields;
        }


        public ICollection<TakeDocModel.DataField> GetDataField(ICollection<string> fields, Guid entityId)
        {
            ICollection<TakeDocModel.DataField> dataFields = 
                daoDataField.GetBy(x => fields.Contains(x.DataFieldReference) && x.EntityId == entityId && x.EtatDeleteData == false);
            return dataFields;
        }
    }
}
