using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocDataAccess.Folder.Interface
{
    public interface IDaoFolder
    {
        TakeDocModel.Folder Create(TakeDocModel.Folder toCreate, Guid userCreateId, Guid entityId);
    }
}
