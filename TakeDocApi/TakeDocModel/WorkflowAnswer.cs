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
    
    public partial class WorkflowAnswer
    {
        public WorkflowAnswer()
        {
            this.Workflow = new HashSet<Workflow>();
        }
    
        public System.Guid WorkflowAnswerId { get; set; }
        public string WorkflowAnswerReference { get; set; }
        public string WorkflowAnswerLabel { get; set; }
        public System.Guid WorkflowTypeId { get; set; }
        public bool WorkflowAnswerGoOn { get; set; }
        public string WorkflowAnswerCss { get; set; }
    
        public virtual WorkflowType WorkflowType { get; set; }
        public virtual ICollection<Workflow> Workflow { get; set; }
    }
}
