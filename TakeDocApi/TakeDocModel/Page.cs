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
    
    public partial class Page
    {
        public System.Guid PageId { get; set; }
        public string PageReference { get; set; }
        public Nullable<System.Guid> PageStreamId { get; set; }
        public System.Guid PageVersionId { get; set; }
        public System.DateTimeOffset DateCreateData { get; set; }
        public System.Guid EntityId { get; set; }
        public System.Guid UserCreateData { get; set; }
        public Nullable<System.DateTimeOffset> DateUpdateData { get; set; }
        public Nullable<System.Guid> UserDeleteData { get; set; }
        public Nullable<System.DateTimeOffset> DateDeleteData { get; set; }
        public Nullable<System.Guid> UserUpdateData { get; set; }
        public bool EtatDeleteData { get; set; }
        public int PageNumber { get; set; }
        public int PageRotation { get; set; }
    }
}
