﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace TakeDocService.Document.Interface
{
    public interface IDocumentService
    {
        TakeDocModel.Document Create(Guid userId, Guid entityId, Guid typeDocumentId, string documentLabel);
        void AddPage(Guid userId, Guid entityId, Guid documentId, string imageString, string extension, int rotation);
        void AddVersionMajor(Guid userId, Guid entityId, Guid documentId);
        void AddVersionMinor(Guid userId, Guid entityId, Guid documentId);
        void SetStatus(Guid userId, string status);
        TakeDocModel.Document GetById(Guid documentId, params Expression<Func<TakeDocModel.Document, object>>[] properties);
    }
}
