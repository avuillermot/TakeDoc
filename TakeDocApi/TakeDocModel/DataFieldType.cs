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
    
    public partial class DataFieldType
    {
        public DataFieldType()
        {
            this.DataField = new HashSet<DataField>();
        }
    
        public string DataFieldTypeId { get; set; }
        public string DataFieldInputType { get; set; }
    
        public virtual ICollection<DataField> DataField { get; set; }
    }
}
