//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TakeDocModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Type_Document
    {
        public Type_Document()
        {
            this.Document = new HashSet<Document>();
            this.DataField = new HashSet<DataField>();
        }
    
        public System.Guid TypeDocumentId { get; set; }
        public string TypeDocumentReference { get; set; }
        public string TypeDocumentLabel { get; set; }
        public System.Guid EntityId { get; set; }
        public System.Guid UserCreateData { get; set; }
        public System.DateTimeOffset DateCreateData { get; set; }
        public Nullable<System.Guid> UserUpdateData { get; set; }
        public Nullable<System.DateTimeOffset> DateUpdateData { get; set; }
        public Nullable<System.Guid> UserDeleteData { get; set; }
        public Nullable<System.DateTimeOffset> DateDeleteData { get; set; }
        public bool EtatDeleteData { get; set; }
    
        public virtual ICollection<Document> Document { get; set; }
        public virtual ICollection<DataField> DataField { get; set; }
    }
}
