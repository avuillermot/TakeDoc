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
    
    public partial class WorkflowType
    {
        public WorkflowType()
        {
            this.Workflow = new HashSet<Workflow>();
        }
    
        public System.Guid WorkflowTypeId { get; set; }
        public string WorkflowReference { get; set; }
        public string WorkflowLabel { get; set; }
    
        public virtual ICollection<Workflow> Workflow { get; set; }
    }
}
