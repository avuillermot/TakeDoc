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
    
    public partial class StatusFolder
    {
        public StatusFolder()
        {
            this.Folder = new HashSet<Folder>();
        }
    
        public System.Guid StatusFolderId { get; set; }
        public string StatusFolderReference { get; set; }
        public string StatusFolderLabel { get; set; }
        public System.Guid EntityId { get; set; }
        public System.Guid UserCreateData { get; set; }
        public Nullable<System.Guid> UserUpdateData { get; set; }
        public Nullable<System.Guid> UserDeleteData { get; set; }
        public System.DateTimeOffset DateCreateData { get; set; }
        public Nullable<System.DateTimeOffset> DateUpdateData { get; set; }
        public Nullable<System.DateTimeOffset> DateDeleteData { get; set; }
        public bool EtatDeleteData { get; set; }
    
        public virtual ICollection<Folder> Folder { get; set; }
    }
}
