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
    
    public partial class DataFieldValue
    {
        public System.Guid DataFieldId { get; set; }
        public int DataFieldValueIndex { get; set; }
        public string DataFieldValueReference { get; set; }
        public string DataFieldValueKey { get; set; }
        public string DataFieldValueText { get; set; }
        public System.Guid EntityId { get; set; }
        public System.Guid UserCreateData { get; set; }
        public System.DateTimeOffset DateCreateData { get; set; }
        public Nullable<System.Guid> UserUpdateData { get; set; }
        public Nullable<System.DateTimeOffset> DateUpdateData { get; set; }
        public Nullable<System.Guid> UserDeleteData { get; set; }
        public Nullable<System.DateTimeOffset> DateDeleteData { get; set; }
        public bool EtatDeleteData { get; set; }
    
        public virtual DataField DataField { get; set; }
    }
}
