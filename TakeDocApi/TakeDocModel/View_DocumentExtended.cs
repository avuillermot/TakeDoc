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
    
    public partial class View_DocumentExtended
    {
        public string EntityReference { get; set; }
        public string EntityLabel { get; set; }
        public string DocumentReference { get; set; }
        public string DocumentLabel { get; set; }
        public string TypeDocumentReference { get; set; }
        public string DocumentTypeLabel { get; set; }
        public bool VersionMajor { get; set; }
        public decimal VersionNumber { get; set; }
        public string VersionReference { get; set; }
        public string StatusVersionReference { get; set; }
        public string VersionStatusLabel { get; set; }
        public string DocumentStatusReference { get; set; }
        public string DocumentStatusLabel { get; set; }
        public Nullable<System.Guid> DocumentOwnerId { get; set; }
        public string DocumentOwnerFullName { get; set; }
        public System.DateTimeOffset VersionDateCreateData { get; set; }
        public System.Guid DocumentId { get; set; }
        public System.Guid EntityId { get; set; }
        public System.Guid VersionId { get; set; }
        public string DocumentOwnerReference { get; set; }
        public System.DateTimeOffset DocumentDateCreateData { get; set; }
        public Nullable<System.Guid> DocumentValidateUserId { get; set; }
    }
}
