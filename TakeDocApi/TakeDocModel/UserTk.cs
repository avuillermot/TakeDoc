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
    
    public partial class UserTk
    {
        public UserTk()
        {
            this.View_UserEntity = new HashSet<View_UserEntity>();
            this.BackOfficeTypeDocument = new HashSet<BackOfficeTypeDocument>();
        }
    
        public System.Guid UserTkId { get; set; }
        public string UserTkReference { get; set; }
        public string UserTkLastName { get; set; }
        public string UserTkFirstName { get; set; }
        public string UserTkLogin { get; set; }
        public string UserTkPassword { get; set; }
        public bool UserTkExternalAccount { get; set; }
        public string UserTkEmail { get; set; }
        public string UserTkExterneId { get; set; }
        public bool UserTkEnable { get; set; }
        public string UserTkCulture { get; set; }
        public bool UserTkActivate { get; set; }
        public System.DateTimeOffset UserTkDateCreateData { get; set; }
        public System.Guid UserTkGroupId { get; set; }
        public Nullable<System.DateTimeOffset> UserTkDateFirstEnable { get; set; }
        public Nullable<System.Guid> UserTkManagerId { get; set; }
    
        public virtual ICollection<View_UserEntity> View_UserEntity { get; set; }
        public virtual GroupTk GroupTk { get; set; }
        public virtual ICollection<BackOfficeTypeDocument> BackOfficeTypeDocument { get; set; }
    }
}
