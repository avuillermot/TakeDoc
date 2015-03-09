using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocModel
{
    public partial class Document
    {
        public TakeDocModel.Version LastVersion
        {
            get
            {
                if (this.Version == null || this.Version.Count() == 0) return null;
                return this.Version.Where(x => x.EtatDeleteData == false).OrderByDescending(x => x.VersionNumber).First();
            }
        }

        public ICollection<TakeDocModel.MetaData> LastVersionMetadata
        {
            get
            {
                if (this.LastVersion == null) return null;
                return this.LastVersion.MetaData.Where(x => x.EtatDeleteData == false).ToList();
            }
        }
    }
}
