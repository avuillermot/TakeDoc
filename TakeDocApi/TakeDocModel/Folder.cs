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
    
    public partial class Folder
    {
        public System.Guid FolderId { get; set; }
        public string FolderReference { get; set; }
        public Nullable<System.DateTimeOffset> FolderDateStart { get; set; }
        public Nullable<System.DateTimeOffset> FolderDateEnd { get; set; }
        public string FolderLabel { get; set; }
        public int FolderPriority { get; set; }
        public System.Guid FolderOwnerId { get; set; }
        public System.Guid FolderTypeId { get; set; }
        public System.Guid EntityId { get; set; }
        public System.Guid UserCreateData { get; set; }
        public System.DateTimeOffset DateCreateData { get; set; }
        public Nullable<System.Guid> UserUpdateData { get; set; }
        public Nullable<System.DateTimeOffset> DateUpdateData { get; set; }
        public Nullable<System.Guid> UserDeleteData { get; set; }
        public Nullable<System.DateTimeOffset> DateDeleteData { get; set; }
        public Nullable<bool> EtatDeleteData { get; set; }
        public System.Guid FolderStatusId { get; set; }
        public string FolderDetail { get; set; }
    
        public virtual FolderType FolderType { get; set; }
    }
}