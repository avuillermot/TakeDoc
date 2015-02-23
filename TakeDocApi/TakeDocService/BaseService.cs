using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TakeDocService
{
    public abstract class BaseService
    {
        public log4net.ILog Logger {
            get {
                return Utility.Logger.myLogger.getLogger("AdoNetAppender");
            }
        }
                    
    }
}
