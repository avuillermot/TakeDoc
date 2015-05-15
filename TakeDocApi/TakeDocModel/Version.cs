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
    
    public partial class Version
    {
        public Version()
        {
            this.Page = new HashSet<Page>();
            this.MetaData = new HashSet<MetaData>();
            this.Workflow = new HashSet<Workflow>();
        }
    
        public System.Guid VersionId { get; set; }
        public string VersionReference { get; set; }
        public Nullable<System.Guid> VersionStreamId { get; set; }
        public System.Guid VersionDocumentId { get; set; }
        public bool VersionMajor { get; set; }
        public decimal VersionNumber { get; set; }
        public System.DateTimeOffset DateCreateData { get; set; }
        public System.Guid EntityId { get; set; }
        public System.Guid UserCreateData { get; set; }
        public Nullable<System.DateTimeOffset> DateUpdateData { get; set; }
        public Nullable<System.Guid> UserDeleteData { get; set; }
        public Nullable<System.DateTimeOffset> DateDeleteData { get; set; }
        public Nullable<System.Guid> UserUpdateData { get; set; }
        public bool EtatDeleteData { get; set; }
        public System.Guid VersionStatusId { get; set; }
    
        public virtual Document Document { get; set; }
        public virtual ICollection<Page> Page { get; set; }
        public virtual ICollection<MetaData> MetaData { get; set; }
        public virtual Status_Version Status_Version { get; set; }
        public virtual ICollection<Workflow> Workflow { get; set; }
    }
}
