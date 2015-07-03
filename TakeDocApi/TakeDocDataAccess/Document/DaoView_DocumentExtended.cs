﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Document
{
    public class DaoView_DocumentExtended : DaoBase<TakeDocModel.View_DocumentExtended>, Interface.IDaoView_DocumentExtended
    {
        public ICollection<TakeDocModel.View_DocumentExtended> Search(Guid typeDocumentId, ICollection<TakeDocModel.Dto.Document.SearchMetadata> searchs, Guid userId, Guid entityId)
        {
            StringBuilder sqlVersion = new StringBuilder("SELECT VersionId FROM dbo.Version v WHERE v.VersionId = dx.VersionId AND v.EtatDeleteData = 0");
            foreach (TakeDocModel.Dto.Document.SearchMetadata search in searchs)
            {
                StringBuilder sqlMeta = new StringBuilder("SELECT MetaDataId FROM dbo.MetaData m WHERE m.MetaDataVersionId = v.VersionId AND m.EtatDeleteData = 0 ");
                if (search.DataField.DataFieldType.DataFieldInputType.ToUpper() == "ION-TOGGLE")
                    sqlMeta.AppendFormat("AND m.MetaDataName = '{0}' AND m.MetaDataValue LIKE 'TRUE' ", search.MetaDataName, search.MetaDataValue);
                else if (search.Condition.ToUpper() == TakeDocModel.Dto.Document.SearchCondition.Start)
                    sqlMeta.AppendFormat("AND m.MetaDataName = '{0}' AND m.MetaDataValue LIKE '{1}%' ", search.MetaDataName, search.MetaDataValue);
                //if (meta.DataField.DataFieldType.DataFieldTypeId.Equals(System.DateTimeOffset)
                sqlVersion.AppendFormat("AND EXISTS({0})", sqlMeta);
            }
            
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM dbo.View_DocumentExtended dx WHERE EXISTS({0}) ", sqlVersion);

            if (entityId.Equals(Guid.Empty) == false) sql.AppendFormat("AND dx.EntityId = '{0}' ", entityId);
            if (typeDocumentId.Equals(Guid.Empty) == false) sql.AppendFormat("AND EXISTS (SELECT d.DocumentId FROM dbo.Document d WHERE d.DocumentId = dx.DocumentId AND d.DocumentTypeId = '{0}' AND d.EtatDeleteData = 0)", typeDocumentId);

            System.Data.Entity.Infrastructure.DbRawSqlQuery<TakeDocModel.View_DocumentExtended> data = this.ctx.Database.SqlQuery<TakeDocModel.View_DocumentExtended>(sql.ToString());
            return data.ToList<TakeDocModel.View_DocumentExtended>();
        }
    }
}