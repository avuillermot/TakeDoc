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
    
    public partial class Customer
    {
        public System.Guid EntityId { get; set; }
        public bool EtatDeleteData { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
    
        public virtual Entity Entity { get; set; }
    }
}
