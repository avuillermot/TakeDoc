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
    
    public partial class Document
    {
        public Document()
        {
            this.Version = new HashSet<Version>();
            this.MetaData = new HashSet<MetaData>();
        }
    
        public System.Guid DocumentId { get; set; }
        public string DocumentReference { get; set; }
        public System.Guid DocumentSatutId { get; set; }
        public Nullable<System.Guid> DocumentCurrentVersion { get; set; }
        public System.Guid EntityId { get; set; }
        public System.Guid UserCreateData { get; set; }
        public System.DateTimeOffset DateCreateData { get; set; }
        public Nullable<System.Guid> UserUpdateData { get; set; }
        public Nullable<System.DateTimeOffset> DateUpdateData { get; set; }
        public Nullable<System.Guid> UserDeleteData { get; set; }
        public Nullable<System.DateTimeOffset> DateDeleteData { get; set; }
        public bool EtatDeleteData { get; set; }
        public string DocumentLabel { get; set; }
        public System.Guid DocumentTypeId { get; set; }
    
        public virtual Statut_Document Statut_Document { get; set; }
        public virtual ICollection<Version> Version { get; set; }
        public virtual TypeDocument Type_Document { get; set; }
        public virtual ICollection<MetaData> MetaData { get; set; }
    }
}
