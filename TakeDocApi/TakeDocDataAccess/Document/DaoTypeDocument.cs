using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document
{
    public class DaoTypeDocument : DaoBase<TakeDocModel.TypeDocument>, Interface.IDaoTypeDocument
    {
        public TakeDocModel.TypeDocument Add(TakeDocModel.TypeDocument typeDocument)
        {
            base.Context.Type_Document.Add(typeDocument);
            base.Context.SaveChanges();
            base.Context.InsertOrUpdateFolderType(typeDocument.TypeDocumentId);
            return typeDocument;
        }

        public new void Update(TakeDocModel.TypeDocument typeDocument)
        {
            base.Update(typeDocument);
            base.Context.InsertOrUpdateFolderType(typeDocument.TypeDocumentId);
        }
    }
}
