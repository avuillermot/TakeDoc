using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects;

namespace TakeDocModel
{
    public partial class TakeDocEntities1
    {
        public string GenerateReference(string entityName)
        {
            ObjectParameter reference = new ObjectParameter("reference", string.Empty);
            ObjectResult<string> retour = this.GetNewReference(entityName, reference);
            return retour.First();
        }
    }
}
