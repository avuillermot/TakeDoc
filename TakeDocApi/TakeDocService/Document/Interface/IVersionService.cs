using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Document.Interface
{
    public interface IVersionService
    {
        TakeDocModel.Version CreateMajor(Guid userId, Guid entityId, Guid versionId, Guid documentId, Guid typeDocumentId);
        TakeDocModel.Version CreateMinor(Guid userId, Guid entityId, Guid versionId, Guid documentId, Guid typeDocumentId);
        TakeDocModel.Version GetById(Guid versionId, params System.Linq.Expressions.Expression<Func<TakeDocModel.Version, object>>[] properties);
        ICollection<TakeDocModel.Version> GetBy(Expression<Func<TakeDocModel.Version, bool>> where, params Expression<Func<TakeDocModel.Version, object>>[] properties);
        void SetStatus(Guid versionId, Guid entityId, string status);
        void GeneratePdf(Guid versionId, Guid entityId);
        void GeneratePdf();
        ICollection<TakeDocModel.Version> PdfToGenerate(Guid entityId);
    }
}
