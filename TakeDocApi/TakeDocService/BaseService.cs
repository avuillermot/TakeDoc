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

        public void CreateError(string message)
        {
            this.Logger.Error(message);
            throw new Exception(message);
        }
    }

    public static class LoggerService
    {
        public static log4net.ILog Logger
        {
            get
            {
                return Utility.Logger.myLogger.getLogger("AdoNetAppender");
            }
        }

        public static void CreateError(string message)
        {
            LoggerService.Logger.Error(message);
            throw new Exception(message);
        }
    }
}
